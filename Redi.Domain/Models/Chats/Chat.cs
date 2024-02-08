namespace Redi.Domain.Models.Chats
{
    public class Chat
    {
        public Guid Id { get; set; }

        public IReadOnlyCollection<Message> Messages { get; set; }
    }
}
