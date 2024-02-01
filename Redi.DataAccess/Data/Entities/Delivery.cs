namespace Redi.DataAccess.Data.Entities
{
    public class Delivery
    {
        public int Id { get; set; }
        public DeliveryType DeliveryType { get; set; }

        public PlaceInfo OriginDetails { get; set; }
        public PlaceInfo DestinationDetails { get; set; }

        public string PackageName { get; set; }
        public string PackageWeight { get; set; }
        public float WorthOfItems { get; set; }
    }
}