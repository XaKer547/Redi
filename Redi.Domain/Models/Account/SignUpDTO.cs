using System.ComponentModel.DataAnnotations;

namespace Redi.Domain.Models.Account
{
    public class SignUpDTO
    {
        [Required]
        public string Fullname { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
