using AviasalesApi.Models;

namespace AviasalesApi.Services
{
    public record struct FilterOptions(
        float PriceFrom,
        float PriceTo,
        DateTime DepartureTimeFrom,
        DateTime DepartureTimeTo,
        DateTime ArrivalTimeFrom,
        DateTime ArrivalTimeTo,
        List<Airline>? Airlines
        );
}