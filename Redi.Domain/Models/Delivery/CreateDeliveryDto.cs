namespace Redi.Domain.Models.Delivery
{
    public class CreateDeliveryDto
    {
        public int OrderId { get; set; }

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
    }
}
