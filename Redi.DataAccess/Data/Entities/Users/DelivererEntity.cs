namespace Redi.DataAccess.Data.Entities.Users
{
    public class DelivererEntity : UserBase
    {
        public DelivererEntity() { }

        public DelivererEntity(string userName) : base(userName) { }
        public ICollection<Delivery> Orders { get; set; } = new HashSet<Delivery>();
    }
}
