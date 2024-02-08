using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

using Redi.Api.Models.ChatModels;
using Redi.DataAccess.Data.Entities.Users;
using Redi.Domain.Services;

namespace Redi.Api.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly UserManager<UserBase> _userManager;
        private readonly IChatService _chatService;

        public ChatHub(
            IChatService chatService, 
            UserManager<UserBase> userManager)
        {
            _chatService = chatService;
            _userManager = userManager;
        }

        public async Task Send(PersonChat chat)
        {
            await Clients.GroupExcept(chat.ChatToken, Context.ConnectionId).SendAsync("Receive", chat.Text);

            var user = await _userManager.GetUserAsync(Context.User);

            await _chatService.AddNewMessage(Guid.Parse(chat.ChatToken), user.Id, chat.Text);
        }
    }
}
