using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Redi.Api.Hubs.Interfaces;
using Redi.Domain.Services;

namespace Redi.Api.Hubs
{
    [Authorize]
    public class ChatHub : Hub<IChatClient>
    {
        private readonly IChatService _chatService;
        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async override Task OnConnectedAsync()
        {
            var history = 0;

            await Clients.Caller.ChatHistory("ae");

            await base.OnConnectedAsync();
        }

        //https://github.com/arslanaybars/SignalR.WebApi.Demo/tree/master
        //https://stackoverflow.com/questions/69392576/signalr-c-sharp-both-client-and-hub-strongly-typed-example
        public async Task Connect(string toUserId)
        {
            var chatId = await _chatService.GetChatId(Context.UserIdentifier, toUserId);

            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }

        public async Task Disconnnect()
        {

        }

        //create group, add users,
    }
}
