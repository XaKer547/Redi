using Redi.DataAccess.Data;
using Redi.Domain.Models.Delivery;

namespace Redi.Deliverer.Models
{
    public class DeliveryViewModel
    {
        public int Id { get; set; }
        public string TrackNumber { get; set; }
        public IReadOnlyCollection<DeliveryStatus> Statuses { get; set; }
        public bool CanUpdateStatus => Statuses.Where(s => s.CreatedDate.HasValue).Count() != Enum.GetValues(typeof(DeliveryStatuses)).Length;
    }
}
