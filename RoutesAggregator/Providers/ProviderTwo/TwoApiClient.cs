using System.Net;
using RoutesAggregator.Models;
using RoutesAggregator.Providers.ProviderTwo.Models;
using Route = RoutesAggregator.Models.Route;


namespace RoutesAggregator.Providers.ProviderTwo;

public class TwoApiClient : IProviderApiClient
{
    private readonly HttpClient _httpClient;

    public TwoApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Route>> SearchAsync(SearchRequest internalRequest, CancellationToken cancellationToken)
    {
        const string requestPath = "search";

        var request = ConvertToExternalRequestModel(internalRequest);

        var content = JsonContent.Create(request);
        var httpResponse = await _httpClient.PostAsync(requestPath, content, cancellationToken);
        var response = await httpResponse.Content.ReadFromJsonAsync<ProviderTwoSearchResponse>(cancellationToken: cancellationToken);

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

    private static ProviderTwoSearchRequest ConvertToExternalRequestModel(SearchRequest internalRequest) =>
        new()
        {
            Departure = internalRequest.Origin,
            Arrival = internalRequest.Destination,
            DepartureDate = internalRequest.OriginDateTime,
            MinTimeLimit = internalRequest.Filters?.MinTimeLimit
        };

    private Route ConvertToInternalRouteModel(ProviderTwoRoute externalRoute) =>
        new()
        {
            Id = Guid.NewGuid(),
            Origin = externalRoute.Departure.Point,
            OriginDateTime = externalRoute.Departure.Date,
            Destination = externalRoute.Arrival.Point,
            DestinationDateTime = externalRoute.Arrival.Date,
            Price = externalRoute.Price,
            TimeLimit = externalRoute.TimeLimit
        };
}