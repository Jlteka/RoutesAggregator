using System.ComponentModel.DataAnnotations;
using RoutesAggregator.Shared;

namespace RoutesAggregator.Models;

public class SearchRequest : IValidatableObject
{
    // Mandatory
    // Start point of route, e.g. Moscow 
    public string Origin { get; set; }
    
    // Mandatory
    // End point of route, e.g. Sochi
    public string Destination { get; set; }
    
    // Mandatory
    // Start date of route
    [FutureDate(ErrorMessage = "Date cannot be in the past")]
    public DateTime OriginDateTime { get; set; }
    
    // Optional
    public SearchFilters Filters { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Filters?.DestinationDateTime != null && Filters.DestinationDateTime < OriginDateTime)
        {
            yield return new ValidationResult("Destination date should be greater than origin date",
                new List<string> { "Filters.DestinationDateTime" });
        }
    }
}