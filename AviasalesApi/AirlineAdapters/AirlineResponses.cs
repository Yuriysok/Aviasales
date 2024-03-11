namespace AviasalesApi.AirlineAdapters
{
    public record struct AeroflotResponse(float PriceDollars, DateTime DepartureTime, DateTime ArrivalTime);
    public record struct LufthansaResponse(float PriceUsd, DateTime TakeoffTime, DateTime LandingTime);
    public record struct UzAirlineResponse(float Dollars, DateTime StartTime, DateTime FinishTime);
}
