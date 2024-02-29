using Geocoding;

namespace Redi.Domain.Models.Delivery
{
    public class PackageTrackDTO
    {
        public int Id { get; set; }
        public string TrackNumber { get; set; }

        public Location OriginPoint { get; set; }
        public Location DestinationPoint { get; set; }

        public IReadOnlyCollection<DeliveryStatus> Statuses { get; set; }
    }
}
