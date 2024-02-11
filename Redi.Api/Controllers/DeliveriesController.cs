using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Redi.Api.Hubs;
using Redi.DataAccess.Data.Entities.Users;
using Redi.Domain.Models.Delivery;
using Redi.Domain.Services;

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
            IHubContext<ChatHub> hubContext)
        {
            _deliveryService = deliveryService;
            _userManager = userManager;
            this._hubContext = hubContext;
        }

        [HttpPut("updateStatus")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Deliverer")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UpdateStatus(UpdateDeliveryStatusDTO updateDeliveryStatus)
        {
            var result = await _deliveryService.UpdateDeliveryStatusAsync(updateDeliveryStatus);

            if (!result.Success)
                return BadRequest(result.Errors);

            var clientId = await _deliveryService.GetDeliveryClientAsync(updateDeliveryStatus.DeliveryId);

            await _hubContext.Clients.Client(clientId).SendAsync("Receive", updateDeliveryStatus.Status.ToString());

            return Ok();
        }

        [HttpGet("all")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Deliverer")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GetAllDeliveries()
        {
            var deliveries = await _deliveryService.GetDeliveriesAsync();

            return Ok(deliveries);
        }

        [HttpHead("close")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Deliverer")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> EndDelivery(int deliveryId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return BadRequest("Пользователь не найден");

            var result = await _deliveryService.EndDeliveryAsync(deliveryId);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok();
        }



        [HttpPost("new")]
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

        [HttpPatch("close")]
        public async Task<IActionResult> EndDelivery(EndDeliveryDTO endDelivery)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return BadRequest("Пользователь не найден");

            var isClient = await _deliveryService.IsDeliveryClientAsync(user.Id, endDelivery.TrackNumber);

            if (isClient)
                return BadRequest();

            var result = await _deliveryService.EndDeliveryAsync(endDelivery.TrackNumber);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok();
        }

        [HttpPost("feedback")]
        public async Task<IActionResult> ReplyOnDelivery(DeliveryFeedbackDTO replyDTO)
        {
            //ахах, эти данные нигде не используются, значит делать не надо

            return Ok();
        }

        [HttpGet("last")]
        public async Task<IActionResult> GetLastAvaibleDelivery()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return BadRequest();

            var packageTrack = await _deliveryService.GetLastAvaibleDeliveryAsync(user.Id);

            return Ok(packageTrack);
        }

        [HttpGet("track")]
        public async Task<IActionResult> GetDeliveryPackageInfo([FromQuery] string trackNumber)
        {
            var exists = await _deliveryService.ExistsAsync(trackNumber);

            if (exists)
                return BadRequest("Заказ не найден");

            var packageInfo = await _deliveryService.GetDeliveryPackageInfoAsync(trackNumber);

            return Ok(packageInfo);
        }
    }
}