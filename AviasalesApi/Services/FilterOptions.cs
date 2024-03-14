using AviasalesApi.Models;

namespace AviasalesApi.Services
{
    public record struct FilterOptions(
        float PriceFrom,
        float PriceTo,
        TimeOnly DepartureTimeFrom,
        TimeOnly DepartureTimeTo,
        TimeOnly ArrivalTimeFrom,
        TimeOnly ArrivalTimeTo,
        List<Airline>? Airlines
        );
}