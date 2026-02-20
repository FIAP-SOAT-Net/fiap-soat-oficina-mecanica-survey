using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.Models;

public class SurveyResponse
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("customerName")]
    public string CustomerName { get; set; } = string.Empty;

    [BsonElement("customerEmail")]
    public string CustomerEmail { get; set; } = string.Empty;

    [BsonElement("responses")]
    public Dictionary<string, int> Responses { get; set; } = new();

    [BsonElement("comments")]
    public string? Comments { get; set; }

    [BsonElement("submittedAt")]
    public DateTime SubmittedAt { get; set; }

    [BsonElement("receivedAt")]
    public DateTime ReceivedAt { get; set; }
}
