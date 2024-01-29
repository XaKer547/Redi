using System.ComponentModel.DataAnnotations;

namespace Redi.Domain.Models.Account
{
    public class PasswordRecoveryRequestDTO
    {
        [Required]
        public string Email { get; set; }
    }
}
