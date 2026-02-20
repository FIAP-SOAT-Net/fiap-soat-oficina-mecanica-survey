namespace Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.Services;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string bodyHtml);
}
