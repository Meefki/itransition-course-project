using System.ComponentModel.DataAnnotations;

namespace IdentityService.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        public bool RememberLogin { get; set; }
        public string RedirectUrl { get; set; } = null!;
    }
}