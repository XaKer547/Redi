using Redi.DataAccess.Data.Entities.Users;

namespace Redi.DataAccess.Data.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public UserBase Sender { get; set; }
        public string Message { get; set; }
        public DateTime SentDate { get; set; }
    }
}