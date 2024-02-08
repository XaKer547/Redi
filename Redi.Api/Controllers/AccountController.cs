﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Redi.Api.Helpers;
using Redi.Api.Infrastructure.Interfaces;
using Redi.DataAccess.Data.Entities.Users;
using Redi.Domain.Models.Account;

namespace Redi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ClientEntity> _userManager;
        private readonly IMailService _mailService;
        public AccountController(UserManager<ClientEntity> userManager, IMailService mailService)
        {
            _userManager = userManager;
            _mailService = mailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfileData()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return NotFound();

            return Ok(new ProfileDTO()
            {
                Id = user.Id,
                UserName = user.UserName,
                Picture = user.Picture,
                Balance = user.Balance,
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfileData(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound();

            return Ok(new ProfileDTO()
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
            {
                return BadRequest("Такой почты не существует");
            }

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

            var result = await _userManager.ResetPasswordAsync(user, verification.OtpCode, verification.NewPassword);

            if (!result.Succeeded)
                return BadRequest();

            return Ok();
        }
    }
}