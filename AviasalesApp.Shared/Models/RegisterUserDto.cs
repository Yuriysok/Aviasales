namespace AviasalesApi.Models
{
    public record RegisterUserDto
    {
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string PassportSerialNumber { get; set; }
    }
}
