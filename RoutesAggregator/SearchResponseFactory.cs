using RoutesAggregator.Models;
using Route = RoutesAggregator.Models.Route;

namespace RoutesAggregator;

public class SearchResponseFactory : ISearchResponseFactory
{
    public SearchResponse Create(IEnumerable<Route> routes)
    {
        var routesArr = routes.ToArray();

        var res = routesArr.Aggregate(
            new 
            {
                MaxPrice = decimal.MinValue,
                MinPrice = decimal.MaxValue,
                MinMinutesRoute = int.MaxValue,
                MaxMinutesRoute = int.MinValue
            },

            (res, o) => new
            {
                MaxPrice = Math.Max(o.Price, res.MaxPrice),
                MinPrice = Math.Min(o.Price, res.MinPrice),
                MinMinutesRoute = Math.Min(CalcMinutesTimeSpan(o.DestinationDateTime, o.OriginDateTime),
                    res.MinMinutesRoute),
                MaxMinutesRoute = Math.Max(CalcMinutesTimeSpan(o.DestinationDateTime, o.OriginDateTime),
                    res.MaxMinutesRoute)
            });

        return new SearchResponse
        {
            Routes = routesArr,
            MaxPrice = res.MaxPrice,
            MinPrice = res.MinPrice,
            MaxMinutesRoute = res.MaxMinutesRoute,
            MinMinutesRoute = res.MinMinutesRoute
        };
    }

    private int CalcMinutesTimeSpan(DateTime destinationDateTime, DateTime originDateTime)
    {
        return Convert.ToInt32((destinationDateTime - originDateTime).TotalMinutes);
    }
}