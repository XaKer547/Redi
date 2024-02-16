using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Redi.Api.Helpers;
using Redi.Api.Infrastructure.Interfaces;
using Redi.DataAccess.Data.Entities.Users;
using Redi.Domain.Models.Account;
using Redi.Domain.Services;

namespace Redi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<UserBase> _userManager;
        private readonly IMailService _mailService;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IAccountManager _accountService;
        public AccountController(UserManager<UserBase> userManager,
            IMailService mailService, IWebHostEnvironment appEnvironment,
            IAccountManager accountService)
        {
            _userManager = userManager;
            _mailService = mailService;
            _appEnvironment = appEnvironment;
            _accountService = accountService;
        }

        [HttpGet("check")]
        public async Task<IActionResult> Check()
        {
            return Ok("Ае");
        }

        //ngrok http https://localhost:44394/ --host-header="localhost:44394"

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetProfileData()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return BadRequest();

            var adsPath = _appEnvironment.WebRootPath + "/ads/";

            var adPaths = Directory.GetFiles(adsPath);

            var ads = adPaths.Select(a => new Advertisment()
            {
                Image = a
            }).ToArray();

            var profileInfo = new ProfileDTO()
            {
                Id = user.Id,
                UserName = user.UserName,
                Picture = user.Picture,
                Advertisments = ads,
            };

            if (user is ClientEntity client)
            {
                profileInfo.Balance = client.Balance;
            }

            return Ok();
        }

        [HttpPost("UpdateProfile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUserProfile(IFormFile image)
        {
            if (image is null)
                return BadRequest();

            if (!image.IsFileAnImage())
                return BadRequest("Файл не является поддерживаемым изображением");

            var imagePath = _appEnvironment.WebRootPath + "/profiles/" + Guid.NewGuid();

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return BadRequest("Пользователь не найден");

            var result = await _accountService.UpdateUserProfileAsync(user.Id, imagePath);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfileData(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return BadRequest("Пользователь не найден");

            return Ok(new ProfilePreview()
            {
                Id = user.Id,
                UserName = user.UserName,
                Picture = user.Picture,
            });
        }

        [HttpPost("RequestPasswordReset")]
        public async Task<IActionResult> RequestPasswordResetAsync(PasswordRecoveryRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return BadRequest("Пользователь не найден");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _mailService.SendOtpCodeAsync(user.Email, token, new()
            {
                RequestInfo = await HttpContext.GetRequestInfo(),
                UserName = user.UserName
            });

            return Ok();
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordDTO verification)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(verification.Email);

            if (user is null)
            {
                return BadRequest("Пользователь не найден");
            }

            var result = await _userManager.ResetPasswordAsync(user, verification.OtpCode, verification.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }
    }
}