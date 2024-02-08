using Redi.DataAccess.Data.Entities.Users;

namespace Redi.DataAccess.Data.Entities
{
    public class Chat
    {
        public Guid Id { get; set; }

        public DelivererEntity Deliverer { get; set; }

        public ClientEntity Client { get; set; }

        public ICollection<ChatMessage> Messages { get; set; } = new HashSet<ChatMessage>();
    }
}
