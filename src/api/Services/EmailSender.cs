using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;

namespace Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.Services;

public sealed class EmailSender(SmtpClient smtpClient) : IEmailSender, IDisposable
{
    private bool _isDisposed;

    public async Task SendEmailAsync(string from, string to, string subject, string body, bool isHtml = false)
    {
        using var mailMessage = new MailMessage(from, to, subject, body);
        mailMessage.IsBodyHtml = isHtml;
        await smtpClient.SendMailAsync(mailMessage);
    }

    public async Task SendEmailAsync(MailAddress from, MailAddress to, string subject, string body, bool isHtml = false)
    {
        using var mailMessage = new MailMessage(from, to);
        mailMessage.Subject = subject;
        mailMessage.Body = body;
        mailMessage.IsBodyHtml = isHtml;
        await smtpClient.SendMailAsync(mailMessage);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_isDisposed) return;

        _isDisposed = true;
        if (disposing)
        {
            smtpClient.Dispose();
        }
    }

    ~EmailSender()
    {
        Dispose(false);
    }
}

