using Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.Configuration;
using Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.Repositories;

public class SurveyRepository : ISurveyRepository
{
    private readonly IMongoCollection<SurveyResponse> _collection;
    private readonly ILogger<SurveyRepository> _logger;

    public SurveyRepository(
        IOptions<MongoDbConfiguration> mongoConfig,
        ILogger<SurveyRepository> logger)
    {
        _logger = logger;

        var config = mongoConfig.Value;
        var connectionString = config.GetConnectionStringWithCredentials();

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(config.DatabaseName);
        _collection = database.GetCollection<SurveyResponse>(config.CollectionName);

        _logger.LogInformation("MongoDB repository initialized for database: {DatabaseName}, collection: {CollectionName}",
            config.DatabaseName, config.CollectionName);
    }

    public async Task SaveAsync(SurveyResponse survey)
    {
        try
        {
            await _collection.InsertOneAsync(survey);
            _logger.LogInformation("Survey saved with ID: {Id}", survey.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving survey to MongoDB");
            throw;
        }
    }

    public async Task<SurveyResponse?> GetByIdAsync(string id)
    {
        try
        {
            var filter = Builders<SurveyResponse>.Filter.Eq(s => s.Id, id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving survey {Id} from MongoDB", id);
            throw;
        }
    }

    public async Task<IEnumerable<SurveyResponse>> GetAllAsync(int page, int pageSize)
    {
        try
        {
            var skip = (page - 1) * pageSize;
            return await _collection
                .Find(_ => true)
                .SortByDescending(s => s.ReceivedAt)
                .Skip(skip)
                .Limit(pageSize)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing surveys from MongoDB");
            throw;
        }
    }
}
