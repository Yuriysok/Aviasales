using Microsoft.Extensions.Options;

namespace AviasalesApi.Services
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