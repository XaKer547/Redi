using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Redi.Domain.Models.Account;
using Redi.Domain.Services;
using IAuthorizationService = Redi.Domain.Services.IAuthorizationService;

namespace Redi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IAccountService _accountService;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthorizationController(IAuthorizationService authorizationService, IAccountService accountService, UserManager<IdentityUser> userManager)
        {
            _authorizationService = authorizationService;
            _accountService = accountService;
            _userManager = userManager;
        }

        [HttpPost("SignInViaGoogle/{id}")]
        public async Task<IActionResult> SignInViaGoogle(string id)
        {
            var googleUser = await GoogleJsonWebSignature.ValidateAsync(id);

            if (googleUser is null)
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(googleUser.Email);

            if (user is null)
            {
                user = new IdentityUser(googleUser.Name)
                {
                    Email = googleUser.Email,
                    EmailConfirmed = googleUser.EmailVerified,
                    NormalizedEmail = googleUser.Email.ToLower(),
                    NormalizedUserName = googleUser.Name.ToLower(),
                    //Profile = googleUser.Picture
                };

                //что с паролем?
                var result = await _userManager.CreateAsync(user);
            }

            var logins = await _userManager.GetLoginsAsync(user);

            if (!logins.Any(l => l.LoginProvider == "google"))
            {
                //поменяй часть с JWT
                var userInfo = new UserLoginInfo("google", googleUser.JwtId, googleUser.Name);

                await _userManager.AddLoginAsync(user, userInfo);
            }

            return Ok(); // user на JSON 
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUpAsync(SignUpDTO signUp)
        {
            if (await _accountService.VerifyEmailExist(signUp.Email))
            {
                return BadRequest("Эта почта уже занята");
            }

            await _authorizationService.SignUpAsync(signUp);

            return Ok();
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignInAsync(SignInDTO signIn)
        {
            var result = await _authorizationService.SingInAsync(signIn);

            if (!result.Success)
                return BadRequest();
            return Ok();

            return Ok(result.Token);
        }
    }
}
