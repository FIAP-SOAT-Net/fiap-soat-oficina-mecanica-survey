using Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.Models;

namespace Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.Repositories;

public interface ISurveyRepository
{
    Task SaveAsync(SurveyResponse survey);
    Task<SurveyResponse?> GetByIdAsync(string id);
    Task<IEnumerable<SurveyResponse>> GetAllAsync(int page, int pageSize);
}
