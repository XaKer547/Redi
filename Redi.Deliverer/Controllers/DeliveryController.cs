using Microsoft.AspNetCore.Authorization;
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
            var deliveries = await _provider.Get<IReadOnlyCollection<DeliveryDto>>("/api/Deliveries/all");

            var model = new DeliveriesViewModel()
            {
                Deliveries = deliveries
            };

            return View(model);
        }

        public async Task<IActionResult> Package(int deliveryId)
        {
            var trackInfo = await _provider.Get<PackageTrackDTO?>($"api/Deliveries/track?deliveryId={deliveryId}");

            if (trackInfo is null)
                return RedirectToAction("Index");

            var statuses = await FillMock(trackInfo.Statuses);

            var viewmodel = new DeliveryViewModel()
            {
                Id = trackInfo.Id,
                TrackNumber = trackInfo.TrackNumber,
                Statuses = statuses
            };

            return View(viewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePackageStatus(int deliveryId)
        {
            await _provider.Head($"api/Deliveries/nextStatus?deliveryId={deliveryId}");

            return await Package(deliveryId);
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
