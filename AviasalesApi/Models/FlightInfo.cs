namespace AviasalesApi.Models
{
    public record struct FlightInfo
    {
        public required string FromCity { get; set; }
        public required string ToCity { get; set; }
        public required DateTime Date { get; set; }
    }
}
