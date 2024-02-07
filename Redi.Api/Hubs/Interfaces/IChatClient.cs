namespace Redi.Api.Hubs.Interfaces
{
    public interface IChatClient
    {
        Task ReceiveMessage(string message);
        Task UserChatHistory(object chattingHistory);
        Task SendPrivateMessage(string toUserId, string message);
        Task ChatHistory(string toUserId);
        Task RefreshData();
    }
}