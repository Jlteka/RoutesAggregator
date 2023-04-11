using System.Net;
using RoutesAggregator.Models;
using RoutesAggregator.Providers.ProviderOne.Models;
using Route = RoutesAggregator.Models.Route;

namespace RoutesAggregator.Providers.ProviderOne;

public class OneApiClient : IProviderApiClient
{
    private readonly HttpClient _httpClient;

    public OneApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Route>> SearchAsync(SearchRequest internalRequest, CancellationToken cancellationToken)
    {
        const string requestPath = "search";

        var externalRequest = ConvertToExternalRequestModel(internalRequest);
        
        var jsonContent = JsonContent.Create(externalRequest);
        var httpResponse = await _httpClient.PostAsync(requestPath, jsonContent, cancellationToken);
        var response = await httpResponse.Content.ReadFromJsonAsync<ProviderOneSearchResponse>(cancellationToken: cancellationToken);
        
        return response.Routes.Select(ConvertToInternalRouteModel);
    }

    public async ValueTask<bool> IsAvailableAsync(CancellationToken cancellationToken)
    {
        const string requestPath = "ping";

        var httpResponse = await _httpClient.GetAsync(requestPath, cancellationToken);
        
        return httpResponse.StatusCode switch
        {
            HttpStatusCode.OK => true,
            HttpStatusCode.InternalServerError => false,
            _ => throw new Exception("Wrong response")
        };
    }

    private ProviderOneSearchRequest ConvertToExternalRequestModel(SearchRequest internalRequest) =>
        new()
        {
            From = internalRequest.Origin,
            To = internalRequest.Destination,
            DateFrom = internalRequest.OriginDateTime,
            DateTo = internalRequest.Filters?.DestinationDateTime,
            MaxPrice = internalRequest.Filters?.MaxPrice
        };

    private Route ConvertToInternalRouteModel(ProviderOneRoute route) =>
        new()
        {
            Id = new Guid(),
            Origin = route.From,
            Destination = route.To,
            OriginDateTime = route.DateFrom,
            DestinationDateTime = route.DateTo,
            Price = route.Price,
            TimeLimit = route.TimeLimit
        };
}