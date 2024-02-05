namespace Redi.DataAccess.Data.Entities
{
    public class DeliveryState
    {
        public int Id { get; set; }
        public DeliveryStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}