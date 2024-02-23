using System.ComponentModel.DataAnnotations;

namespace Redi.Domain.Models.Delivery
{
    public class DeliveryFeedbackDTO
    {
        [Required]
        [RegularExpression(@"^R-\d{4}-\d{4}-\d{4}-\d{4}$", ErrorMessage = "Трэк номер должен быть в формате 'R-9999-9999-9999-9999'")]
        public string TrackNumber { get; set; }
        public int Rating { get; set; }
        public string Feedback { get; set; }
    }
}
