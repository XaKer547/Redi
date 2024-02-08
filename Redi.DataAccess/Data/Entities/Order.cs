using System.ComponentModel.DataAnnotations.Schema;

using Redi.DataAccess.Data.Entities.Users;

namespace Redi.DataAccess.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public Delivery Delivery { get; set; }

        [InverseProperty(nameof(ClientEntity.Orders))]
        public ClientEntity Client { get; set; }

        public ICollection<DeliveryState> OrderStates { get; set; } = new HashSet<DeliveryState>();
    }
}