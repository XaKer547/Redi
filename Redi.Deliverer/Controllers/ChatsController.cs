using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{chatId}")]
        public async Task<IActionResult> Index(Guid chatId)
        {
            var chat = await _provider.Get<IReadOnlyCollection<ChatPreview>>($"api/chats/{chatId}");

            return View(chatId);
        }
    }
}
