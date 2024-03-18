using System.Reflection;

namespace AviasalesApi.Models
{
    public record Flight
    {
        public required string FlightId { get; set; }
        public Airline Airline { get; set; }
        public float PriceUsd { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public int NumberOfFlights { get; set; }
    }
}
