using System.ComponentModel.DataAnnotations.Schema;

namespace Redi.DataAccess.Data.Entities.Users
{
    public class DelivererEntity : UserBase
    {
        public DelivererEntity() { }

        public DelivererEntity(string userName) : base(userName) { }

        [InverseProperty(nameof(Chat.Deliverier))]
        public ICollection<Chat> Chats { get; set; } = new HashSet<Chat>();

        [InverseProperty(nameof(Delivery.Deliverier))]
        public ICollection<Delivery> Deliveries { get; set; } = new HashSet<Delivery>();
    }
}
