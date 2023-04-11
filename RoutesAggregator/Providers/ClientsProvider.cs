using Microsoft.Extensions.Options;
using RoutesAggregator.Providers.Settings;

namespace RoutesAggregator.Providers;

public class ClientsProvider : IClientsProvider
{
    private readonly List<IProviderApiClient> _clients = new();

    public ClientsProvider(IOptions<ProvidersSettings> providersSettings, IServiceProvider serviceProvider)
    {
        var apiClientFactories = serviceProvider.GetServices<IApiClientFactory>();
        var settings = providersSettings.Value.ProviderSettings;

        _clients.AddRange(settings.Select(s => apiClientFactories
            .Single(f => f.Type == s.Type)
            .Create(s)));
    }
    
    public async Task<List<IProviderApiClient>> GetAvailableClientsAsync(CancellationToken ct)
    {
        return await _clients.ToAsyncEnumerable()
            .WhereAwait(async x => await x.IsAvailableAsync(ct))
            .ToListAsync(cancellationToken: ct);
    }
}