using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Redi.Domain.Services;
using Microsoft.AspNetCore.Identity;
using Redi.DataAccess.Data.Entities.Users;

namespace Redi.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly UserManager<UserBase> _userManager;
        public ChatsController(IChatService chatService, UserManager<UserBase> userManager)
        {
            _chatService = chatService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetChats()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return BadRequest();

            var chats = await _chatService.GetChatsPreviews(user.Id);

            return Ok(chats);
        }

        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetChat(string chatId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return BadRequest();

            var isChatPart = await _chatService.CheckJoin(user.Id, chatId);

            if (!isChatPart)
            {
                return BadRequest();
            }

            var chat = await _chatService.GetChatAsync(chatId);

            return Ok(chat);
        }
    }
}
