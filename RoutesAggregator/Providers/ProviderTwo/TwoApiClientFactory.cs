using RoutesAggregator.Providers.Settings;

namespace RoutesAggregator.Providers.ProviderTwo;

public class TwoApiClientFactory : IApiClientFactory
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TwoApiClientFactory(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public string Type => ProviderTypes.TwoProvider;

    public IProviderApiClient Create(ProviderSettings settings)
    {
        var httpClient = _httpClientFactory.CreateClient(settings.Name);
        httpClient.BaseAddress = new Uri(settings.Url);

        return new TwoApiClient(httpClient);
    }
}