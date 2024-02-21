using Redi.DataAccess.Data.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redi.DataAccess.Data.Entities
{
    public class Chat
    {
        public Guid Id { get; set; }

        public string ClientId { get; set; } = null!;
        [ForeignKey(nameof(ClientId))]
        public ClientEntity Client { get; set; } = null!;

        public string? DeliverierId { get; set; }

        [ForeignKey(nameof(DeliverierId))]
        public DelivererEntity? Deliverier { get; set; }

        public ICollection<ChatMessage> Messages { get; set; } = new HashSet<ChatMessage>();
    }
}
