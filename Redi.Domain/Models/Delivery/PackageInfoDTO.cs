namespace Redi.Domain.Models.Delivery
{
    public class PackageInfoDTO
    {
        public string TrackNumber { get; set; }

        public string OriginAddress { get; set; }
        public string OriginStateCountry { get; set; }
        public string OriginPhoneNumber { get; set; }
        public string OriginOthers { get; set; }

        public string DestinationAddress { get; set; }
        public string DestinationStateCountry { get; set; }
        public string DestinationPhoneNumber { get; set; }
        public string DestinationOthers { get; set; }

        public string PackageName { get; set; }
        public string PackageWeight { get; set; }
        public float WorthOfItems { get; set; }

        public float DeliveryCharges { get; set; }
        public float? InstantDelivery { get; set; }
        public float Taxes { get; set; }
    }
}
