using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Redi.Api.Infrastructure.Interfaces;
using Redi.DataAccess.Data;
using Redi.DataAccess.Data.Entities.Users;
using Redi.Domain.Models.Account;

namespace Redi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly UserManager<UserBase> _userManager;
        private readonly IJwtGenerator _jwtService;
        public AuthorizationController(UserManager<UserBase> userManager, IJwtGenerator jwtService)
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
                user = new ClientEntity(googleUser.Name)
                {
                    Email = googleUser.Email,
                    EmailConfirmed = googleUser.EmailVerified,
                    NormalizedEmail = googleUser.Email.ToLower(),
                    NormalizedUserName = googleUser.Name.ToLower(),
                    Picture = googleUser.Picture
                };

                var result = await _userManager.CreateAsync(user);

                await _userManager.AddToRoleAsync(user, Roles.Client.ToString());
            }

            var logins = await _userManager.GetLoginsAsync(user);

            if (!logins.Any(l => l.LoginProvider == "google"))
            {
                var userInfo = new UserLoginInfo("google", googleUser.Email, googleUser.Name);

                await _userManager.AddLoginAsync(user, userInfo);
            }

            return Ok(_jwtService.CreateToken(user.Id, Roles.Client.ToString()));
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUpAsync(SignUpDTO signUp)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(signUp.Email);

            if (user is not null)
                return BadRequest("Эта почта уже занята");

            var result = await _userManager.CreateAsync(new ClientEntity()
            {
                PhoneNumber = signUp.PhoneNumber,
                Email = signUp.Email,
                UserName = signUp.Fullname,
                Balance = 4000,
            }, signUp.Password);

            await _userManager.AddToRoleAsync(user, Roles.Client.ToString());

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignInAsync(SignInDTO signIn)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(signIn.Email.ToLower());

            if (user is null)
                return BadRequest("Пользователь не найден");

            var result = await _userManager.CheckPasswordAsync(user, signIn.Password);

            if (!result)
                return BadRequest();

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(_jwtService.CreateToken(user.Id, roles));
        }
    }
}