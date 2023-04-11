using System.ComponentModel.DataAnnotations;
using RoutesAggregator.Shared;

namespace RoutesAggregator.Models;

public class SearchFilters
{
    // Optional
    // End date of route
    [FutureDate(ErrorMessage = "Date cannot be in the past")]
    public DateTime? DestinationDateTime { get; set; }
    
    // Optional
    // Maximum price of route
    [Range(0, Double.MaxValue, ErrorMessage = "Price should be greater than 0")]
    public decimal? MaxPrice { get; set; }
    
    // Optional
    // Minimum value of timelimit for route
    [FutureDate(ErrorMessage = "Date cannot be in the past")]
    public DateTime? MinTimeLimit { get; set; }
    
    // Optional
    // Forcibly search in cached data
    public bool? OnlyCached { get; set; }
}