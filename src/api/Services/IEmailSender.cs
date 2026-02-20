using System.Net.Mail;

namespace Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.Services;

public interface IEmailSender
{
    Task SendEmailAsync(MailAddress from, MailAddress to, string subject, string body, bool isHtml = false);
}
