namespace AviasalesApi.Models
{
    public record struct BookFlightDto(
        Airline Airline,
        Guid FlightId,
        string Name,
        string PassportSerialNumber
        );
}
