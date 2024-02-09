using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Redi.DataAccess.Data;
using Redi.DataAccess.Data.Entities;
using Redi.DataAccess.Data.Entities.Users;
using Redi.Domain.Models.Delivery;
using Redi.Domain.Services;

namespace Redi.Application.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly RediDbContext _rediDbContext;
        private readonly UserManager<UserBase> _userManager;
        public DeliveryService(UserManager<UserBase> userManager, RediDbContext rediDbContext)
        {
            _userManager = userManager;
            _rediDbContext = rediDbContext;
        }

        public async Task CreateDelivery(CreateDeliveryDto deliveryDto)
        {
            var order = await _rediDbContext.Orders.SingleOrDefaultAsync(x => x.Id == deliveryDto.OrderId);

            var delivery = await GetDelivererWithMinCountDelivery();

            _rediDbContext.Chats.Add(new Chat
            {
                Id = Guid.NewGuid(),
                Deliverer = (DelivererEntity)delivery,
                Client = order.Client
            });

            _rediDbContext.Deliveries.Add(new Delivery
            {
                OrderId = order.Id,
                Deliverer = delivery,
                DeliveryType = DeliveryTypes.Instant,
                OrderStates = new List<DeliveryState>()
                {
                    new DeliveryState()
                    {
                        CreatedDate = DateTime.Now,
                        Status  = DeliveryStatuses.Requested
                    }
                },
                DestinationAddress = deliveryDto.DestinationAddress,
                DestinationOthers = deliveryDto.DestinationOthers,
                DestinationPhoneNumber = deliveryDto.DestinationPhoneNumber,
                DestinationStateCountry = deliveryDto.DestinationStateCountry,
                WorthOfItems = deliveryDto.WorthOfItems,
                PackageWeight = deliveryDto.PackageWeight,
                PackageName = deliveryDto.PackageName,
                OriginStateCountry = deliveryDto.OriginStateCountry,
                OriginPhoneNumber = deliveryDto.OriginPhoneNumber,
                OriginAddress = deliveryDto.OriginAddress,
                OriginOthers = deliveryDto.OriginOthers,
            });

            await _rediDbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<DeliveryDto>> GetDeliveries(string userId)
        {

        }

        public Task<DeliveryDto> GetDelivery(int id)
        {
            throw new NotImplementedException();
        }

        private async Task<UserBase> GetDelivererWithMinCountDelivery()
        {
            var users = await _userManager.GetUsersInRoleAsync(Roles.Deliverer.ToString()) ?? throw new Exception("Я не могу создать заказ! Доставщиков нет");

            var deliveries = await _rediDbContext.Deliveries.GroupBy(x => x.Deliverer.Id)
                .ToArrayAsync();

            string delivererWithMinCountDelivery = string.Empty;
            int min = int.MaxValue;
            foreach (var delivery in deliveries)
            {
                var count = delivery.Count();
                if (min > count)
                {
                    min = count;
                    delivererWithMinCountDelivery = delivery.Key;
                }
            }

            return await _userManager.FindByIdAsync(delivererWithMinCountDelivery);
        }
    }
}
