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

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetProfileData()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return BadRequest();

            var profileInfo = new ProfileDTO()
            {
                Id = user.Id,
                UserName = user.UserName,
            };

            if (user.Picture is not null)
            {
                profileInfo.Picture = $"https://{HttpContext.Request.Host.Value}/{user.Picture}";
            }

            return Ok(profileInfo);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfileData(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return BadRequest("Пользователь не найден");

            var profileInfo = new ProfileDTO()
            {
                Id = user.Id,
                UserName = user.UserName,
            };

            if (user.Picture is not null)
            {
                profileInfo.Picture = $"https://{HttpContext.Request.Host.Value}/{user.Picture}";
            }

            return Ok(profileInfo);
        }

        [HttpGet("ads")]
        public async Task<IActionResult> GetAdsAsync()
        {
            var adPaths = Directory.GetFiles(_appEnvironment.WebRootPath + "/ads/");

            var ads = adPaths.Select(a => new Advertisment()
            {
                ImageUrl = $"https://{a.Replace(_appEnvironment.WebRootPath, HttpContext.Request.Host.Value)}"
            }).ToArray();

            return Ok(ads);
        }


        /// <summary>
        /// Получить информацию о своем кошельке
        /// </summary>
        /// <returns>Баланс и история транзакций</returns>
        [HttpGet("wallet")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Client")]
        public async Task<IActionResult> GetWalletData()
        {
            var user = (ClientEntity)await _userManager.GetUserAsync(User);

            if (user is null)
                return BadRequest();

            var transactions = await _accountService.GetTransactionHistoryAsync(user.Id);

            var walletInfo = new WalletInfoDTO()
            {
                Balance = user.Balance,
                Transactions = transactions
            };

            return Ok(walletInfo);
        }

        /// <summary>
        /// Повысить свой баланс на 250
        /// </summary>
        /// <returns></returns>
        [HttpGet("HESOYAM")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddMoney()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return BadRequest();

            await _accountService.IncreaseBalanceAsync(user.Id);

            return Ok();
        }

        /// <summary>
        /// Устроить раздачу для всех клиентов (Только для админов)
        /// </summary>
        /// <param name="money">Сумма денег</param>
        /// <returns></returns>
        [HttpGet("Charity")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Deliverer")]
        public async Task<IActionResult> AddMoney(float money = 300)
        {
            await _accountService.IncreaseBalanceAsync(money);

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

            var imagePath = _appEnvironment.WebRootPath + "/profiles/" + Guid.NewGuid() + ".png";

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return BadRequest("Пользователь не найден");

            var result = await _accountService.UpdateUserProfileAsync(user.Id, imagePath.Replace(_appEnvironment.WebRootPath, ""));

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok();
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