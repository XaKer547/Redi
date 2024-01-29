using System.ComponentModel.DataAnnotations;

namespace Redi.Domain.Models.Account
{
    public class ChangePasswordDTO
    {
        [Required]
        public string Password { get; set; }
    }
}
