using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Redi.Application.Models;
using Redi.DataAccess.Data.Entities.Users;
using Redi.Deliverer.Models;
using Redi.Domain.Models.Chats;
using Redi.Domain.Services;

namespace Redi.Deliverer.Controllers
{
    [Route("[controller]")]
    public class ChatsController : Controller
    {
        private readonly IRediApiProvider _provider;

        public ChatsController(IRediApiProvider provider)
        {
            _provider = provider;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var chats = await _provider.Get<IReadOnlyCollection<ChatPreview>>("api/chats");

            var viewModel = new ChatsViewModel()
            {
                Chats = chats
            };

            return View(viewModel);
        }

        [HttpGet("ChatDetails")]
        public async Task<IActionResult> ChatDetails(Guid chatId)
        {
            var chat = await _provider.Get<ChatDto>($"api/chats/{chatId}");

            var test = User;
            return PartialView(new ChatDetailsViewModel
            {
                ChatId = chatId,
                Messages = chat.Messages.Select(x => new ChatMessageViewModel
                {
                    Sender = x.Sender,
                    Text = x.Text,
                    IsUserSender = x.IsUserSender,
                }).ToArray()
            });
        }
    }
}
