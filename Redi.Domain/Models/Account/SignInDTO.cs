using Redi.Domain.DataAnnotation;
using System.ComponentModel.DataAnnotations;

namespace Redi.Domain.Models.Account
{
    public class SignInDTO
    {
        [Required]
        [EmailAddress]
        [LowerCaseOnly]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

//uakmck@gmail.com
//GreatPassword_123
