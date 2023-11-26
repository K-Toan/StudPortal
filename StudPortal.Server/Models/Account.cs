using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace StudPortal.Server.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required, Length(1, 20)]
        public string FirstName { get; set; }

        [Required, Length(1, 20)]
        public string LastName { get; set; }

        [NotNull, Length(6, 20)]
        public string Username { get; set; } = string.Empty;

        // Hashed password, stored in database
        [NotNull]
        public byte[] PasswordHash { get; set; }

        // A random string added to password hashing process
        [NotNull]
        public byte[] PasswordSalt { get; set; }

        // Each account must and only have one role
        public string Role { get; set; } = string.Empty;
    }
}
