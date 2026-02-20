namespace Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.Models;

public class SurveyRequest
{
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty;
    public DateTime ServiceDate { get; set; }
}
