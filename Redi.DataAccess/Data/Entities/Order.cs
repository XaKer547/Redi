namespace Redi.DataAccess.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public Delivery Delivery { get; set; }
        public ICollection<OrderStatus> OrderStatuses { get; set; } = new HashSet<OrderStatus>();
    }
}
