using Redi.Domain.Models.Chats;
using Redi.Domain.Services.Response;

namespace Redi.Domain.Services
{
    public interface IChatService
    {
        Task<ServiceResult> AddNewMessageAsync(Guid chatId, string userId, string message);

        Task<IReadOnlyCollection<ChatPreview>> GetChatsPreviewsAsync(string userId);

        Task<Chat> GetChatAsync(Guid chatId);

        Task<bool> CheckJoinAsync(string userId, Guid chatId);
    }
}