using Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.Configuration;
using Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.Factories;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.Services;

public class EventProcessorService(
    IEmailService emailService,
    IOptions<SurveyConfiguration> options,
    ILogger<EventProcessorService> logger) : IEventProcessorService
{
    public async Task ProcessEventAsync(string message, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Processing event message {@Message}", message);

            using var jsonDocument = JsonDocument.Parse(message);
            var root = jsonDocument.RootElement;

            string email = string.Empty;
            if (root.TryGetProperty("ClientEmail", out var clientEmail))
            {
                email = clientEmail.TryGetProperty("Address", out var emailAddress) ? emailAddress.GetString() ?? string.Empty : string.Empty;
            }
            string name = root.TryGetProperty("ClientName", out var clientName) ? clientName.GetString() ?? string.Empty : string.Empty;
            string title = root.TryGetProperty("Title", out var emailTitle) ? emailTitle.GetString() ?? string.Empty : string.Empty;
            var mailDetails = MailFactory.Create(email, name, title, options?.Value.WebAppUrl ?? "http://localhost:8080");

            await emailService.SendEmailAsync(mailDetails.To, mailDetails.Subject, mailDetails.BodyHtml);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing event");
            throw;
        }
    }
}
