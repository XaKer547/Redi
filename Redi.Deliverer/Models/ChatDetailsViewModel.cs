namespace Redi.Deliverer.Models
{
    public class ChatDetailsViewModel
    {
        public Guid ChatId { get; set; }

        public IReadOnlyCollection<ChatMessageViewModel> Messages { get; set; }
    }

    public class ChatMessageViewModel
    {
        public string Sender { get; set; }

        public string Text { get; set; }

        public bool IsUserSender { get; set; }
    }
}
