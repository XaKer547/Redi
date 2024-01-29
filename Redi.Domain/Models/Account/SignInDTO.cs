using System.ComponentModel.DataAnnotations;

namespace Redi.Domain.Models.Account
{
    public class SignInDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
