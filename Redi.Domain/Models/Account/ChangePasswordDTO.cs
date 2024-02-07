using System.ComponentModel.DataAnnotations;

namespace Redi.Domain.Models.Account
{
    public class ChangePasswordDTO
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
