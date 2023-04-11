using RoutesAggregator.Models;
using Route = RoutesAggregator.Models.Route;

namespace RoutesAggregator.Providers;

public interface IProviderApiClient
{
    Task<IEnumerable<Route>> SearchAsync(SearchRequest request, CancellationToken cancellationToken);
    ValueTask<bool> IsAvailableAsync(CancellationToken cancellationToken);
}