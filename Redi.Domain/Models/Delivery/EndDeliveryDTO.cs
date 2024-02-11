using System.ComponentModel.DataAnnotations;

namespace Redi.Domain.Models.Delivery
{
    public class EndDeliveryDTO
    {
        [Required]
        [RegularExpression(@"^R-\d{4}-\d{4}-\d{4}-\d{4}$")]
        public string TrackNumber { get; set; }
    }
}
