using RoutesAggregator.Models;
using Route = RoutesAggregator.Models.Route;

namespace RoutesAggregator;

public interface ISearchResponseFactory
{
    SearchResponse Create(IEnumerable<Route> routes);
}