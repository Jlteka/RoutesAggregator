using RoutesAggregator.Providers.Settings;

namespace RoutesAggregator.Providers;

public interface IApiClientFactory
{
    string Type { get; }
    public IProviderApiClient Create(ProviderSettings settings);
}