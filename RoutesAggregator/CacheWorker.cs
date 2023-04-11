using Microsoft.Extensions.Caching.Memory;
using RoutesAggregator.Models;
using Route = RoutesAggregator.Models.Route;

namespace RoutesAggregator;

public class CacheWorker : ICacheWorker
{
    private readonly IMemoryCache _cache;
    private List<Guid> _ids = new();

    public CacheWorker(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void CacheRows(IEnumerable<Route> rawRoutes)
    {
        lock (_ids)
        {
            foreach (var route in rawRoutes)
            {
                _cache.Set(route.Id, route, route.TimeLimit); //PossibleDuplicates
                _ids.Add(route.Id);
            }
        }
    }

    public IEnumerable<Route> Search(SearchRequest request)
    {
        var rawRoutes = GetAllCachedRoutes();
        var requestedRoutes = rawRoutes
            .Where(x => request.OriginDateTime == x.OriginDateTime)
            .Where(x => request.Origin == x.Origin)
            .Where(x => request.Destination == x.Destination);

        return requestedRoutes;
    }

    private IEnumerable<Route> GetAllCachedRoutes()
    {
        lock (_ids)
        {
            var updatedIds = new List<Guid>();
            foreach (var id in _ids)
            {
                if (_cache.TryGetValue(id, out Route route))
                {
                    updatedIds.Add(id);
                    yield return route;
                }
            }

            _ids = updatedIds;
        }
        
    }
}