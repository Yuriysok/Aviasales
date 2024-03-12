using AviasalesApi.Services;

namespace AviasalesApi.Models
{
    public record GetFlightsDto
    {
        public required string FromCity { get; set; }
        public required string ToCity { get; set; }
        public required DateTime Date { get; set; }
        public SortOptions SortOptions { get; set; }
    }
}
