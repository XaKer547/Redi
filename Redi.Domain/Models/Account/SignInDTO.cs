using Redi.Domain.DataAnnotation;
using System.ComponentModel.DataAnnotations;

namespace Redi.Domain.Models.Account
{
    public class SignInDTO
    {
        [Required]
        [EmailAddress]
        [LowerCaseOnly(ErrorMessage = "Почта должна быть написана строчными символами")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

//uakmck@gmail.com
//GreatPassword_123
