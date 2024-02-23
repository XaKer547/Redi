using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Redi.DataAccess.Data.Entities.Users;
using Redi.Domain.Services;
using System.ComponentModel.DataAnnotations;

namespace Redi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

            var chats = await _chatService.GetChatsPreviewsAsync(user.Id);

            return Ok(chats);
        }

        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetChat([Required] Guid chatId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return BadRequest("Пользователь не найден");

            var isChatPart = await _chatService.CheckJoinAsync(user.Id, chatId);

            if (!isChatPart)
            {
                return BadRequest();
            }

            var chat = await _chatService.GetChatAsync(chatId);

            if (chat is null)
                return BadRequest();

            return Ok(chat);
        }
    }
}
