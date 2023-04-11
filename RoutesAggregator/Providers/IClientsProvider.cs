namespace RoutesAggregator.Providers;

public interface IClientsProvider
{
    Task<List<IProviderApiClient>> GetAvailableClientsAsync(CancellationToken ct);
}