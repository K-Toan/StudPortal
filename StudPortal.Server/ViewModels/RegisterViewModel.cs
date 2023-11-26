using System.ComponentModel.DataAnnotations;

namespace StudPortal.Server.ViewModels
{
    public class RegisterViewModel
    {
        [Required, Length(1, 20, ErrorMessage="First name must be between 1 and 20 char")]
        public string FirstName { get; set; } = string.Empty;

        [Required, Length(1, 20, ErrorMessage="Last name must be between 1 and 20 char")]
        public string LastName { get; set; } = string.Empty;

        [Required, Length(6, 20, ErrorMessage="Username must be between 1 and 20 char")]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string RepeatPassword { get; set; } = string.Empty;
    }
}
