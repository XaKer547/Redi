using Redi.DataAccess.Data.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redi.DataAccess.Data.Entities
{
    public class Delivery
    {
        public int Id { get; set; }
        public string TrackNumber { get; set; }

        public string ClientId { get; set; }
        [ForeignKey(nameof(ClientId))]
        [InverseProperty(nameof(ClientEntity.Deliveries))]
        public ClientEntity Client { get; set; }

        [InverseProperty(nameof(DelivererEntity.Deliveries))]
        public DelivererEntity Deliverier { get; set; }

        public DeliveryTypes DeliveryType { get; set; }

        #region OriginDetails
        public string OriginAddress { get; set; }
        public string OriginStateCountry { get; set; }
        public string OriginPhoneNumber { get; set; }
        public string OriginOthers { get; set; }
        #endregion

        #region DestinationDetails
        public string DestinationAddress { get; set; }
        public string DestinationStateCountry { get; set; }
        public string DestinationPhoneNumber { get; set; }
        public string DestinationOthers { get; set; }
        #endregion

        #region OtherDetails
        public string PackageName { get; set; }
        public string PackageWeight { get; set; }
        public float WorthOfItems { get; set; }
        #endregion

        #region Charges
        public float DeliveryCharges { get; set; }
        public float Taxes { get; set; }
        #endregion

        public ICollection<DeliveryState> OrderStates { get; set; } = new HashSet<DeliveryState>();

        public int Rating { get; set; }
        public bool IsActive { get; set; }

        [NotMapped]
        public float FinalPrice => DeliveryCharges + Taxes + (DeliveryType == DeliveryTypes.Instant ? 300f : 0);
    }
}