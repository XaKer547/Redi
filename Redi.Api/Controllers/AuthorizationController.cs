using Microsoft.AspNetCore.Mvc;
using Redi.Domain.Models.Account;
using Redi.Domain.Services;

namespace Redi.Api.Controllers
{
    [ApiController]
    [Route("/[action]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IAccountService _accountService;
        public AuthorizationController(IAuthorizationService authorizationService, IAccountService accountService)
        {
            _authorizationService = authorizationService;
            _accountService = accountService;
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

            return Ok(result.Token);
        }
    }
}
