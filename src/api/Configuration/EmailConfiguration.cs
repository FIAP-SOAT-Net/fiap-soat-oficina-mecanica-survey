namespace Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.Configuration;

public class EmailConfiguration
{
    public string SenderAddress { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string SmtpHost { get; set; } = string.Empty;
    public string SmtpUsername { get; set; } = string.Empty;
    public string SmtpPassword { get; set; } = string.Empty;
    public bool EnableSsl { get; set; }
    public int SmtpPort { get; set; }
}
