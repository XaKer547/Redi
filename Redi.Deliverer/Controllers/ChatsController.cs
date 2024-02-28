using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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
        public IActionResult Index()
        {
            var viewModel = new ChatsViewModel()
            {
                Chats = new List<ChatPreview>()
                {
                    new()
                    {
                        Id = new Guid(),
                        LastMessage = "Сап",
                        InterlocutorFullname = "Сясь промежсон1"
                    },
                    new()
                    {
                        Id = new Guid(),
                        LastMessage = "Двач",
                        InterlocutorFullname = "Сясь промежсон2"
                    },
                    new()
                    {
                        Id = new Guid(),
                        LastMessage = "Мур мур мур",
                        InterlocutorFullname = "Сясь промежсон3"
                    },
                    new()
                    {
                        Id = new Guid(),
                        LastMessage = "Я ламповая няшв",
                        InterlocutorFullname = "Сясь промежсон4"
                    },
                }
            };

            return View(viewModel);
        }


        [HttpGet("{chatId}")]
        public async Task<IActionResult> Index(Guid chatId)
        {
            //redirect to current chat
            return Ok(chatId);
        }
    }
}
