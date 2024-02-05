namespace Redi.DataAccess.Data.Entities
{
    public class Chat
    {
        public int Id { get; set; }
        public ICollection<User> Users { get; set; } = new HashSet<User>();
        public ICollection<ChatMessage> Messages { get; set; } = new HashSet<ChatMessage>();
    }
}
