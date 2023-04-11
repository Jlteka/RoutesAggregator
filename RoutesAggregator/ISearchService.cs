using RoutesAggregator.Models;

namespace RoutesAggregator;

public interface ISearchService
{
    Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken ct);
    Task<bool> IsAvailableAsync(CancellationToken ct);
}