namespace AviasalesApi.Models
{
    public record struct FilterOptions(
        float PriceFrom,
        float PriceTo,
        TimeOnly DepartureTimeFrom,
        TimeOnly DepartureTimeTo,
        TimeOnly ArrivalTimeFrom,
        TimeOnly ArrivalTimeTo,
        int TransitFlightsMax,
        List<Airline>? Airlines
        );
}