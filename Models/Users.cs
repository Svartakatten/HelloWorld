using System.ComponentModel.DataAnnotations;

namespace HelloWorld.Models
{
    public class Users
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Category { get; set; } = "default";

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? PasswordHash { get; set; }

        public string? Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}