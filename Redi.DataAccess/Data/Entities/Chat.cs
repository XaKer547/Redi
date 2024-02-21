using Redi.DataAccess.Data.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redi.DataAccess.Data.Entities
{
    public class Chat
    {
        public Guid Id { get; set; }

        public string ClientId { get; set; }
        [ForeignKey(nameof(ClientId))]
        [InverseProperty(nameof(ClientEntity.Chats))]
        public ClientEntity Client { get; set; }

        public string DeliverierId { get; set; }
        [ForeignKey(nameof(DeliverierId))]
        [InverseProperty(nameof(DelivererEntity.Chats))]
        public DelivererEntity Deliverier { get; set; }

        public ICollection<ChatMessage> Messages { get; set; } = new HashSet<ChatMessage>();
    }
}
