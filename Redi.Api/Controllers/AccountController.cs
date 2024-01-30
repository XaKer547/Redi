using Microsoft.AspNetCore.Mvc;
using Redi.Domain.Models.Account;
using Redi.Domain.Services;

namespace Redi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("PasswordRecovery")]
        public async Task<IActionResult> RequestPasswordRecoveryAsync(PasswordRecoveryRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _accountService.VerifyEmailExist(request.Email))
            {
                return BadRequest("Такой почты не существует");
            }

            await _accountService.RequestPasswordRecoveryAsync(request);

            return Ok();
        }

        [HttpPost("OtpVerification")]
        public async Task<IActionResult> VerifyOtpAsync(OtpVerificationDTO verification)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _accountService.VerifyOtpCodeAsync(verification); //get userId or token

            if (!result)
                return BadRequest();

            return Ok();
        }

        [HttpPatch("ChangePassword")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDTO changePassword)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //getUserId

            await _accountService.ChangePasswordAsync(1, changePassword.Password);

            return Ok();
        }
    }
}