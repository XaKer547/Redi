using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Redi.Domain.Services;

namespace Redi.Deliverer.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class ChatsController : Controller
    {
        private readonly IRediApiProvider _provider;
        public ChatsController(IRediApiProvider provider)
        {
            _provider = provider;
        }

        public IActionResult Index()
        {



            return View();
        }
    }
}
