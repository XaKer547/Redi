using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Redi.Deliverer.Models;
using Redi.Domain.Services;
using System.Security.Claims;

namespace Redi.Deliverer.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly ILogger<AuthorizationController> _logger;
        private readonly IRediApiProvider _provider;
        public AuthorizationController(ILogger<AuthorizationController> logger, IRediApiProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        public IActionResult Index()
        {
            var deliverers = new List<DelivererPreview>
            {
                new()
                {
                    Fullname = "Erminie Capini",
                    Email = "ecapini0@japanpost.jp",
                    Password = "qKPGUj"
                },
                new()
                {
                    Fullname = "Dalston Batteson",
                    Email = "dbattesong@state.tx.us",
                    Password = "thMgNwS)ZyB"
                },
                new()
                {
                    Fullname = "Herrick Exroll",
                    Email = "hexrollp@uol.com.br",
                    Password = "v1AQnaed{H0"
                },
                new()
                {
                    Fullname = "Klement Ever",
                    Email = "kever10@marketwatch.com",
                    Password = "Vq4Ue%pDI"
                },
            };

            var viewModel = new AuthorizationViewModel()
            {
                Deliverers = deliverers,
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Authorize(string email, string password)
        {
            var model = new Domain.Models.Account.SignInDTO()
            {
                Email = email,
                Password = password
            };

            var response = await _provider.Authorize(model);

            if (!response.Success)
                return RedirectToAction(nameof(Index));

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, "Deliverer"),
                new Claim(ClaimTypes.Email, email),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToActionPermanent(nameof(Index), "Home");
        }
    }
}
