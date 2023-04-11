using RoutesAggregator.Models;
using RoutesAggregator.Providers;
using Route = RoutesAggregator.Models.Route;

namespace RoutesAggregator;

public class SearchService : ISearchService
{
    private readonly IClientsProvider _clientsProvider;
    private readonly ISearchResponseFactory _searchResponseFactory;
    private readonly ICacheWorker _cacheWorker;

    public SearchService(IClientsProvider clientsProvider,
                         ISearchResponseFactory searchResponseFactory,
                         ICacheWorker cacheWorker)
    {
        _clientsProvider = clientsProvider;
        _searchResponseFactory = searchResponseFactory;
        _cacheWorker = cacheWorker;
    }

    public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken ct)
    {
        IEnumerable<Route> routes;
        if (request.Filters?.OnlyCached == true)
        {
            routes = _cacheWorker.Search(request).ToList();
        }
        else
        {
            var avaliableClients = await _clientsProvider.GetAvailableClientsAsync(ct); 
            var searchTasks = avaliableClients.Select(x => x.SearchAsync(request, ct));
            routes = (await Task.WhenAll(searchTasks)).SelectMany(s=>s).Distinct();
            _cacheWorker.CacheRows(routes);
        }

        var filters = request.Filters;
        if (filters != null)
        {
            routes = Filter(routes, filters);
        }

        return _searchResponseFactory.Create(routes);
    }

    public async Task<bool> IsAvailableAsync(CancellationToken ct)
    {
        var availableClients = await _clientsProvider.GetAvailableClientsAsync(ct);
        return availableClients.Count != 0;
    }
    
    
    
    private static IEnumerable<Route> Filter(IEnumerable<Route> rawRoutes, SearchFilters filters)
    {
        return rawRoutes
            .Where(x => filters.DestinationDateTime == null ||
                        filters?.DestinationDateTime == x.DestinationDateTime)
            .Where(x => filters.MinTimeLimit == null ||
                        filters?.MinTimeLimit < x.TimeLimit)
            .Where(x => filters.MaxPrice == null ||
                        filters?.MaxPrice > x.Price);
    }
}