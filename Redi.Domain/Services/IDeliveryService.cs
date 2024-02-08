using Redi.Domain.Models.Delivery;

namespace Redi.Domain.Services
{
    public interface IDeliveryService
    {
        Task CreateDelivery(CreateDeliveryDto deliveryDto);

        Task<IReadOnlyCollection<DeliveryDto>> Get(string userId);

        Task<DeliveryDto> Get(int id);
    }
}
