using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Redi.Api.Infrastructure.Interfaces;
using Redi.DataAccess.Data.Entities;
using Redi.Domain.Models.Account;

namespace Redi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMailService _mailService;
        public AccountController(UserManager<User> userManager, IMailService mailService)
        {
            _userManager = userManager;
            _mailService = mailService;
        }

        [HttpPost("Ae")]
        public async Task<IActionResult> Check()
        {
            var ip = Request.HttpContext.Connection.RemoteIpAddress;
            //await _mailService.SendOtpCodeAsync("FIFA228Nothack@gmail.com", "Лох");

            return Ok();
        }

        [HttpPost("RequestPasswordReset")]
        public async Task<IActionResult> RequestPasswordResetAsync(PasswordRecoveryRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return BadRequest("Такой почты не существует");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _mailService.SendOtpCodeAsync(user.Email, token);

            return Ok();
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordDTO verification)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(verification.Email);

            var result = await _userManager.ResetPasswordAsync(user, verification.OtpCode, verification.NewPassword);

            if (!result.Succeeded)
                return BadRequest();

            return Ok();
        }
    }
}