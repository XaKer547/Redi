namespace Redi.Domain.Services
{
    public interface IChatService
    {
        Task<string> GetChatId(string fromUserId, string toUserId);
        Task<string> GetChatHistory();
    }
}