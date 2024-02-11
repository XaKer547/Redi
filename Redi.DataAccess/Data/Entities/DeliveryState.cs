namespace Redi.DataAccess.Data.Entities
{
    public class DeliveryState
    {
        public int Id { get; set; }
        public DeliveryStatuses Status { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}