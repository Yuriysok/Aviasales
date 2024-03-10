namespace AviasalesApi.Models
{
    public record UserDto
    {
        public required string Name { get; set; }
        public required string Password { get; set; }
    }
}
