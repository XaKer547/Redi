using System.ComponentModel.DataAnnotations;

namespace Redi.Domain.Models.Account
{
    public class OtpVerificationDTO
    {
        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Code { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
