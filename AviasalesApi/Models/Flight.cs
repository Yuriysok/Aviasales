using System.Reflection;

namespace AviasalesApi.Models
{
    public record Flight
    {
        public Airline Airline { get; set; }
        public float PriceUsd { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
    }
}
