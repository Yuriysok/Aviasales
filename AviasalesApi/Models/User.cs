namespace AviasalesApi.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string PasswordHash { get; set; }
    }
}
