using Redi.Domain.Models.Delivery;

namespace Redi.Deliverer.Models
{
    public class DeliveriesViewModel
    {
        public IReadOnlyCollection<DeliveryDto> Deliveries { get; set; }
    }
}
