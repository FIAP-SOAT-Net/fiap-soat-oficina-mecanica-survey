namespace Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.Services;

public interface IEventProcessorService
{
    Task ProcessEventAsync(string message, CancellationToken cancellationToken = default);
}
