using Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.Services;

public class EmailService(IOptions<EmailConfiguration> options, IEmailSender emailSender) : IEmailService
{
    private readonly EmailConfiguration _emailConfiguration = options.Value;

    public async Task SendEmailAsync(string to, string subject, string bodyHtml)
    {
        var fromAddress = new MailAddress(_emailConfiguration.SenderAddress, _emailConfiguration.SenderName);
        var toAddress = new MailAddress(to);
        await emailSender.SendEmailAsync(fromAddress, toAddress, subject, bodyHtml, true);
    }
}
