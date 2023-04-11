using RoutesAggregator.Providers.Settings;

namespace RoutesAggregator.Providers.ProviderOne;

public class OneApiClientFactory : IApiClientFactory
{
    private readonly IHttpClientFactory _httpClientFactory;

    public OneApiClientFactory(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public string Type => ProviderTypes.OneProvider;

    public IProviderApiClient Create(ProviderSettings settings)
    {
        var httpClient = _httpClientFactory.CreateClient(settings.Name);
        httpClient.BaseAddress = new Uri(settings.Url);
        
        return new OneApiClient(httpClient);
    }
}