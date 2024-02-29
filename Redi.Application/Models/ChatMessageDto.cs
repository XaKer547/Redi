namespace Redi.Application.Models
{
    public class ChatMessageDto
    {
        public int Id { get; set; }

        public string Sender { get; set; }

        public string Text { get; set; }

        public bool IsUserSender { get; set; }
    }
}
