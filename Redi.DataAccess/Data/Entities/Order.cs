namespace Redi.DataAccess.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public Delivery Delivery { get; set; }
        public ICollection<DeliveryState> OrderStates { get; set; } = new HashSet<DeliveryState>();
    }
}