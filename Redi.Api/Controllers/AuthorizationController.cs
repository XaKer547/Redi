using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Redi.Api.Infrastructure.Interfaces;
using Redi.DataAccess.Data.Entities;
using Redi.Domain.Models.Account;

namespace Redi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtGenerator _jwtService;
        public AuthorizationController(UserManager<User> userManager, IJwtGenerator jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpPost("SignInViaGoogle")]
        public async Task<IActionResult> SignInViaGoogle([FromBody] string token)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var googleUser = await GoogleJsonWebSignature.ValidateAsync(token);

            if (googleUser is null)
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(googleUser.Email);

            if (user is null)
            {
                user = new User(googleUser.Name)
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

            return Ok(_jwtService.CreateToken(user.Id, "user")); // user на JSON 
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUpAsync(SignUpDTO signUp)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(signUp.Email);

            if (user is not null)
                return BadRequest("Эта почта уже занята");

            await _userManager.CreateAsync(new User()
            {
                PhoneNumber = signUp.PhoneNumber,
                Email = signUp.Email,
                NormalizedEmail = signUp.Email.ToLower(),
                UserName = signUp.Fullname,
                NormalizedUserName = signUp.Fullname.ToLower(),
            }, signUp.Password);

            return Ok();
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignInAsync(SignInDTO signIn)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(signIn.Email);

            if (user is null)
                return BadRequest("Пользователь не найден");

            var result = await _userManager.CheckPasswordAsync(user, signIn.Password);

            if (!result)
                return BadRequest();

            return Ok(_jwtService.CreateToken(user.Id, "user"));
        }
    }
}