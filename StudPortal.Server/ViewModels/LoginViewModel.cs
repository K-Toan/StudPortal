using System.ComponentModel.DataAnnotations;

namespace StudPortal.Server.ViewModels
{
    public class LoginViewModel
    {
        [Required, Length(6, 20, ErrorMessage="Username must be between 1 and 20 char")]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
