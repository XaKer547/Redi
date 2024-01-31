using System.ComponentModel.DataAnnotations;

namespace Redi.Domain.Models.Account
{
    public class ResetPasswordDTO
    {
        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string OtpCode { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
