using Microsoft.AspNetCore.Mvc;
using Redi.Application.Helpers;
using Redi.Deliverer.Models;
using Redi.Domain.Models.Delivery;
using Redi.Domain.Models.Delivery.Enums;
using Redi.Domain.Services;

namespace Redi.Deliverer.Controllers
{
    public class DeliveryController : Controller
    {
        private readonly IRediApiProvider _provider;
        public DeliveryController(IRediApiProvider provider)
        {
            _provider = provider;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DeliveriesViewModel()
            {
                Deliveries = new List<DeliveryDto>()
                {
                    new()
                    {
                        Id = 1,
                        TrackNumber ="aeaeae"
                    },
                     new()
                    {
                        Id = 1,
                        TrackNumber ="aeaeae"
                    },
                      new()
                    {
                        Id = 1,
                        TrackNumber ="aeaeae"
                    },
                       new()
                    {
                        Id = 1,
                        TrackNumber ="aeaeae"
                    },
                        new()
                    {
                        Id = 1,
                        TrackNumber ="aeaeae"

                    },
                }.ToArray()
            };

            return View(model);
        }

        public async Task<IActionResult> Package(string trackNumber)
        {
            IReadOnlyCollection<DeliveryStatus> statuses = new List<DeliveryStatus>()
            {
                new()
                {
                    Name = "Requested",
                    CreatedDate = DateTime.Now
                }
            };

            statuses = await FillMock(statuses);

            var viewmodel = new DeliveryViewModel()
            {
                Id = 1,
                TrackNumber = trackNumber,
                Statuses = statuses
            };

            return View(viewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePackageStatus(int deliveryId)
        {
            //await _provider.Head("");

            return Ok();
        }

        private async Task<IReadOnlyCollection<DeliveryStatus>> FillMock(IReadOnlyCollection<DeliveryStatus> owned)
        {
            var mock = Enum.GetValues<DeliveryStatuses>();

            var statuses = new List<DeliveryStatus>(owned);

            var missing = mock.Select(s => new DeliveryStatus
            {
                Name = s.Description()
            }).Where(s => !owned.Any(p => p.Name == s.Name)).ToArray();

            statuses.AddRange(missing);

            return statuses;
        }
    }
}
