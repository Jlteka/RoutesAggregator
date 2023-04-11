using Microsoft.AspNetCore.Mvc;
using RoutesAggregator.Models;

namespace RoutesAggregator.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet]
    public async Task<IActionResult> IsAvailable()
    {
        var cancelTokenSource = new CancellationTokenSource();
        return await _searchService.IsAvailableAsync(cancelTokenSource.Token) 
            ? Ok() 
            : Problem();
    }

    [HttpPost]
    public async Task<SearchResponse> Search(SearchRequest request)
    {
        var cancelTokenSource = new CancellationTokenSource();
        return await _searchService.SearchAsync(request, cancelTokenSource.Token);
    }
}