using Microsoft.AspNetCore.Mvc;

namespace Redi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryController : ControllerBase
    {
        [HttpPost("CalculateTariffList")]
        public async Task<IActionResult> CalculateTariffList()
        {

            return Ok();
        }








    }
}
