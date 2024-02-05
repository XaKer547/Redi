namespace Redi.DataAccess.Data.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public User Sender { get; set; }
        public string Message { get; set; }
        public DateTime SentDate { get; set; }
    }
}