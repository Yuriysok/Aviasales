namespace AviasalesApi.Models
{
    public record struct BookFlightDto(
        Airline Airline,
        string FlightId,
        string Name,
        string PassportSerialNumber
        );
}
