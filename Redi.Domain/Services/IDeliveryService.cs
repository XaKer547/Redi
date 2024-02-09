using Redi.Domain.Models.Delivery;

namespace Redi.Domain.Services
{
    public interface IDeliveryService
    {
        Task CreateDelivery(CreateDeliveryDto deliveryDto);

        Task<IReadOnlyCollection<DeliveryDto>> GetDeliveries(string userId);

        Task<DeliveryDto> GetDelivery(int id);
    }
}
