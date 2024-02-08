using Redi.Domain.Models.Chats;

namespace Redi.Domain.Services
{
    public interface IChatService
    {
        Task AddNewMessage(Guid chatId, string userId, string message);

        Task<IReadOnlyCollection<ChatPreview>> GetChatsPreviews(string userId);

        Task<Chat> GetChatAsync(Guid chatId);

        Task<bool> CheckJoin(string userId, Guid chatId);
    }
}