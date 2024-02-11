using Redi.Domain.Models.Delivery.Enums;

namespace Redi.Domain.Models.Delivery
{
    public class UpdateDeliveryStatusDTO
    {
        public int DeliveryId { get; set; }
        public DeliveryStatuses Status { get; set; }
    }
}
