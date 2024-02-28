using Redi.Domain.Models.Chats;

namespace Redi.Deliverer.Models
{
    public class ChatsViewModel
    {
        public IReadOnlyCollection<ChatPreview> Chats { get; set; }
    }
}
