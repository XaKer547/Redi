namespace Redi.Domain.Models.Chats
{
    public class ChatPreview
    {
        public Guid Id { get; set; }
        public string InterlocutorFullname {  get; set; }
        public string? InterlocutorPhoto { get; set; }
        public string LastMessage { get; set; }
    }
}