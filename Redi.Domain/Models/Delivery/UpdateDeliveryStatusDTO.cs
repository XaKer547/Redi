using Redi.Domain.Models.Delivery.Enums;
using System.ComponentModel.DataAnnotations;

namespace Redi.Domain.Models.Delivery
{
    public class UpdateDeliveryStatusDTO
    {
        [Required]
        public int DeliveryId { get; set; }

        [Required]
        public DeliveryStatuses Status { get; set; }
    }
}
