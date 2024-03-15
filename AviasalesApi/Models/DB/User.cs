namespace AviasalesApi.Models.DB
{
    [Index(nameof(Name), IsUnique = true)]
    [Index(nameof(PassportSerialNumber), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string PasswordHash { get; set; }
        public required string PassportSerialNumber { get; set; }
    }
}
