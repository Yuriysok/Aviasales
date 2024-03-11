namespace AviasalesApi.AirlineAdapters
{
    public record struct AeroflotResponse(float PriceDollars, DateTime DepartureTime, DateTime ArrivalTime);
    public record struct LufthansaResponse(float PriceDollars, DateTime DepartureTime, DateTime ArrivalTime);
    public record struct UzAirlineResponse(float PriceDollars, DateTime DepartureTime, DateTime ArrivalTime);
}
