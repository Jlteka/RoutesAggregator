using System.ComponentModel.DataAnnotations;

namespace RoutesAggregator.Shared;

public class FutureDateAttribute : RangeAttribute
{
    public FutureDateAttribute()
        : base(typeof(DateTime), DateTime.Now.ToShortDateString(), DateTime.MaxValue.ToShortDateString())
    {
    }
}