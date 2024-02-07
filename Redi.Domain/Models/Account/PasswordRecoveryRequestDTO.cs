using System.ComponentModel.DataAnnotations;

namespace Redi.Domain.Models.Account
{
    public class PasswordRecoveryRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
