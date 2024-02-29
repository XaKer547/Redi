namespace Redi.Application.Models
{
    public class ChatDto
    {
        public Guid Id { get; set; }

        public IReadOnlyCollection<ChatMessageDto> Messages { get; set; }
    }
}
