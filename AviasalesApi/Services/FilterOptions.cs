using AviasalesApi.Models;

namespace AviasalesApi.Services
{
    //public record struct FilterOptions(
    //    float PriceFrom,
    //    float PriceTo,
    //    DateTime DepartureTimeFrom,
    //    DateTime DepartureTimeTo,
    //    DateTime ArrivalTimeFrom,
    //    DateTime ArrivalTimeTo,
    //    List<Airline>? Airlines
    //    );
    public class FilterOptions
    {
        public FilterOptions()
        {
        }

        public float PriceFrom { get; set; } = default;
        public float PriceTo { get; set; } = default;
        public DateTime DepartureTimeFrom { get; set; } = default;
        public DateTime DepartureTimeTo { get; set; } = default;
        public DateTime ArrivalTimeFrom { get; set; } = default;
        public DateTime ArrivalTimeTo { get; set; } = default;
        public List<Airline>? Airlines { get; set; } = default;
    }
}