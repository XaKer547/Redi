using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Redi.Api.Hubs;
using Redi.DataAccess.Data.Entities.Users;
using Redi.Domain.Models.Delivery;
using Redi.Domain.Services;
using System.ComponentModel.DataAnnotations;

namespace Redi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DeliveriesController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;
        private readonly UserManager<UserBase> _userManager;
        private readonly IHubContext<DeliveryHub> _hubContext;
        public DeliveriesController(IDeliveryService deliveryService,
            UserManager<UserBase> userManager,
            IHubContext<DeliveryHub> hubContext)
        {
            _deliveryService = deliveryService;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Обновить статус доставки (Для админов)
        /// </summary>
        /// <param name="updateDeliveryStatus"></param>
        /// <returns></returns>
        [HttpPut("updateStatus")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Deliverer")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UpdateStatus(UpdateDeliveryStatusDTO updateDeliveryStatus)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _deliveryService.UpdateDeliveryStatusAsync(updateDeliveryStatus);

            if (!result.Success)
                return BadRequest(result.Errors);

            var clientId = await _deliveryService.GetDeliveryClientAsync(updateDeliveryStatus.DeliveryId);

            var trackNumber = await _deliveryService.GetDeliveryTrackNumberAsync(updateDeliveryStatus.DeliveryId);

            if (trackNumber is null)
                return BadRequest("Доставка не найдена");

            var trackInfo = await _deliveryService.GetDeliveryTrackInfoAsync(trackNumber);

            await _hubContext.Clients.User(clientId).SendAsync("UpdatePackageStatus", trackNumber, trackInfo.Statuses);

            return Ok();
        }

        /// <summary>
        /// Поставить следующий возможный статус доставки (Для админов)
        /// </summary>
        /// <param name="deliveryId">Id доставки</param>
        /// <returns></returns>
        [HttpHead("nextStatus")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Deliverer")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UpdateStatus(int deliveryId)
        {
            var result = await _deliveryService.UpdateDeliveryStatusAsync(deliveryId);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok();
        }

        /// <summary>
        /// Получить все доставки (Для админов)
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Deliverer")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GetAllDeliveries()
        {
            var deliveries = await _deliveryService.GetDeliveriesAsync();

            return Ok(deliveries);
        }

        /// <summary>
        /// Закончить доставку (Для админов)
        /// </summary>
        /// <param name="deliveryId">Id доставки</param>
        /// <returns></returns>
        [HttpHead("close")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Deliverer")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> EndDelivery(int deliveryId)
        {
            var result = await _deliveryService.EndDeliveryAsync(deliveryId);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok();
        }

        /// <summary>
        /// Получить статусы и местоположение активного заказа (Для админов)
        /// </summary>
        /// <param name="deliveryId">Id доставки</param>
        /// <returns></returns>
        [HttpGet("track")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Deliverer")]
        public async Task<IActionResult> GetPackageTrack(int deliveryId)
        {
            var trackNumber = await _deliveryService.GetDeliveryTrackNumberAsync(deliveryId);

            if (trackNumber is null)
                return BadRequest();

            var trackInfo = await _deliveryService.GetDeliveryTrackInfoAsync(trackNumber);

            return Ok(trackInfo);
        }

        /// <summary>
        /// Создать доставку
        /// </summary>
        /// <param name="createDelivery">DTO с информацией о заказе</param>
        /// <returns></returns>
        [HttpPost("new")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Client")]
        public async Task<IActionResult> Create(CreateDeliveryDto createDelivery)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return BadRequest();

            var result = await _deliveryService.CreateDeliveryAsync(user.Id, createDelivery);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok();
        }

        /// <summary>
        /// Закончить доставку
        /// </summary>
        /// <param name="endDelivery">DTO с вложенным трек-номером доставки</param>
        /// <returns></returns>
        [HttpPatch("close")]
        public async Task<IActionResult> EndDelivery(EndDeliveryDTO endDelivery)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return BadRequest("Пользователь не найден");

            var isClient = await _deliveryService.IsDeliveryClientAsync(user.Id, endDelivery.TrackNumber);

            if (!isClient)
                return BadRequest();

            var result = await _deliveryService.EndDeliveryAsync(endDelivery.TrackNumber);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok();
        }

        /// <summary>
        /// Оставить отзыв за доставку
        /// </summary>
        /// <param name="replyDTO"></param>
        /// <returns></returns>
        [HttpPost("feedback")]
        public async Task<IActionResult> ReplyOnDelivery(DeliveryFeedbackDTO replyDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            //ахах, эти данные нигде не используются, значит делать не надо

            return Ok();
        }

        /// <summary>
        /// Получить статусы и местоположение последнего активного заказа
        /// </summary>
        /// <returns></returns>
        [HttpGet("last")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Client")]
        public async Task<IActionResult> GetLastAvaibleDelivery()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return BadRequest();

            var packageTrack = await _deliveryService.GetLastAvaibleDeliveryAsync(user.Id);

            return Ok(packageTrack);
        }

        /// <summary>
        /// Получить подробную информацию о доставке
        /// </summary>
        /// <param name="trackNumber">Трэк-номер заказа</param>
        /// <returns></returns>
        [HttpGet("{trackNumber}")]
        public async Task<IActionResult> GetDeliveryPackageInfo(
        [RegularExpression(@"^R-\d{4}-\d{4}-\d{4}-\d{4}$", ErrorMessage = "Трэк номер должен быть в формате 'R-9999-9999-9999-9999'")]
        string trackNumber)
        {
            var packageInfo = await _deliveryService.GetDeliveryPackageInfoAsync(trackNumber);

            if (packageInfo is null)
                return BadRequest("Заказ не найден");

            return Ok(packageInfo);
        }
    }
}