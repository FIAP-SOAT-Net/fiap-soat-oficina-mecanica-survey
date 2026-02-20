using Fiap.Soat.SmartMechanicalWorkshop.Survey.Api;
using Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.Configuration;
using Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.Models;
using Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbConfiguration>(builder.Configuration.GetSection("MongoDB"));
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("Email"));
builder.Services.Configure<SurveyConfiguration>(builder.Configuration.GetSection("Survey"));

builder.Services.AddSingleton<ISurveyRepository, SurveyRepository>();
builder.Services.AddScoped<IEventProcessorService, EventProcessorService>();
builder.Services.AddScoped<IRabbitMqConsumerService, RabbitMqConsumerService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmailSender, EmailSender>(opt =>
{
    var emailSettings = opt.GetRequiredService<IOptions<EmailConfiguration>>().Value;
    var smtpClient = new SmtpClient
    {
        Host = emailSettings.SmtpHost,
        Port = emailSettings.SmtpPort,
        EnableSsl = emailSettings.EnableSsl,
        Credentials = new System.Net.NetworkCredential(emailSettings.SmtpUsername, emailSettings.SmtpPassword)
    };
    return new EmailSender(smtpClient);
});

builder.Services.AddHostedService<Worker>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.MapHealthChecks("/health");
app.MapPost("/api/survey", async (
        [FromBody] SurveyResponse surveyResponse,
        ISurveyRepository repository,
        ILogger<Program> logger) =>
    {
        surveyResponse.Id = Guid.NewGuid().ToString();
        surveyResponse.ReceivedAt = DateTime.UtcNow;

        await repository.SaveAsync(surveyResponse);

        return Results.Created($"/api/survey/{surveyResponse.Id}", new { id = surveyResponse.Id, message = "Survey saved successfully" });
    })
    .WithName("SubmitSurvey")
    .WithOpenApi()
    .Produces<object>(StatusCodes.Status201Created)
    .Produces<object>(StatusCodes.Status400BadRequest)
    .Produces<object>(StatusCodes.Status500InternalServerError);

// GET endpoint to retrieve a survey by ID
app.MapGet("/api/survey/{id}", async (
        string id,
        ISurveyRepository repository,
        ILogger<Program> logger) =>
    {
        var survey = await repository.GetByIdAsync(id);
        return survey == null ? Results.NotFound(new { error = "Survey not found" }) : Results.Ok(survey);
    })
    .WithName("GetSurvey")
    .WithOpenApi()
    .Produces<SurveyResponse>(StatusCodes.Status200OK)
    .Produces<object>(StatusCodes.Status404NotFound)
    .Produces<object>(StatusCodes.Status500InternalServerError);

app.MapGet("/api/survey", async (
        ISurveyRepository repository,
        ILogger<Program> logger,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10) =>
    {
        var surveys = await repository.GetAllAsync(page, pageSize);
        return Results.Ok(surveys);
    })
    .WithName("ListSurveys")
    .WithOpenApi()
    .Produces<IEnumerable<SurveyResponse>>(StatusCodes.Status200OK)
    .Produces<object>(StatusCodes.Status500InternalServerError);

await app.RunAsync();
