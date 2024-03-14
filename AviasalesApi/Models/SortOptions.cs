using Microsoft.Extensions.Options;

namespace AviasalesApi.Models
{
    public enum SortOptions
    {
        None,
        OrderByPriceAsc,
        OrderByPriceDesc,
        OrderByDepartureDateAsc,
        OrderByDepartureDateDesc,
        OrderByArrivalDateAsc,
        OrderByArrivalDateDesc,
        OrderByAirlineAsc,
        OrderByAirlineDesc,
    }
}