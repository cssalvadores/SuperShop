using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        [Required]
        [MinLength(5)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
