using RoutesAggregator.Models;
using Route = RoutesAggregator.Models.Route;

namespace RoutesAggregator;

public interface ICacheWorker
{
    void CacheRows(IEnumerable<Route> rawRoutes);
    IEnumerable<Route> Search(SearchRequest request);
}