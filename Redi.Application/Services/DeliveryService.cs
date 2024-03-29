﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Redi.Application.Helpers;
using Redi.DataAccess.Data;
using Redi.DataAccess.Data.Entities;
using Redi.DataAccess.Data.Entities.Users;
using Redi.Domain.Models.Delivery;
using Redi.Domain.Services;
using Redi.Domain.Services.Response;

namespace Redi.Application.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly RediDbContext _context;
        private readonly UserManager<UserBase> _userManager;
        public DeliveryService(UserManager<UserBase> userManager, RediDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<ServiceResult> CreateDeliveryAsync(string clientId, CreateDeliveryDto deliveryDto)
        {
            var result = new ServiceResult();

            var client = (ClientEntity)await _userManager.FindByIdAsync(clientId);

            if (client is null)
            {
                result.Errors.Add("Пользователь не найден");
                return result;
            }

            var price = deliveryDto.GetFinalPrice();

            if (client.Balance < price)
            {
                result.Errors.Add("Недостаточно средств для оплаты");

                return result;
            }

            var deliverer = await GetDelivererWithMinCountDelivery();

            _context.Chats.Add(new Chat
            {
                Id = Guid.NewGuid(),
                Deliverier = (DelivererEntity)deliverer,
                Client = client
            });

            var delivery = new Delivery
            {
                Client = client,
                TrackNumber = deliveryDto.TrackNumber,
                Deliverier = (DelivererEntity)deliverer,
                DeliveryType = deliveryDto.IsInstantDelivery ? DeliveryTypes.Instant : DeliveryTypes.Scheduled,
                OrderStates = new List<DeliveryState>()
                {
                    new()
                    {
                        CreatedDate = DateTime.Now,
                        Status = DeliveryStatuses.Requested
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
                DeliveryCharges = deliveryDto.DeliveryCharges,
                Taxes = deliveryDto.Taxes,
                IsActive = true,
            };

            _context.Deliveries.Add(delivery);

            client.Balance -= price;
            var packagePrewiew = delivery.PackageName.Length > 10 ? delivery.PackageName[..10] + "..." : delivery.PackageName;

            client.Transactions.Add(new Transaction()
            {
                Date = DateTime.Now,
                Money = delivery.FinalPrice,
                Name = $"{packagePrewiew} delivery",
            });

            _context.Users.Update(client);

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<ServiceResult> EndDeliveryAsync(string trackNumber)
        {
            var result = new ServiceResult();

            var delivery = await _context.Deliveries.SingleOrDefaultAsync(d => d.TrackNumber == trackNumber);

            if (delivery is null)
            {
                result.Errors.Add("Доставка не найдена");
                return result;
            }

            delivery.IsActive = false;

            _context.Deliveries.Update(delivery);

            await _context.SaveChangesAsync();

            return result;
        }
        public async Task<ServiceResult> EndDeliveryAsync(int deliveryId)
        {
            var result = new ServiceResult();

            var delivery = await _context.Deliveries.SingleOrDefaultAsync(d => d.Id == deliveryId);

            if (delivery is null)
            {
                result.Errors.Add("Доставка не найдена");
                return result;
            }

            delivery.IsActive = false;

            _context.Deliveries.Update(delivery);

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<bool> IsDeliveryClientAsync(string clientId, string trackNumber)
        {
            return await _context.Deliveries.AnyAsync(d => d.ClientId == clientId && d.TrackNumber == trackNumber);
        }

        public async Task<bool> ExistsAsync(string trackNumber)
        {
            return await _context.Deliveries.AnyAsync(d => d.TrackNumber == trackNumber);
        }

        public async Task<PackageTrackDTO?> GetLastAvaibleDeliveryAsync(string userId)
        {
            var delivery = await _context.Deliveries.Include(d => d.OrderStates)
                .Where(d => d.ClientId == userId && d.IsActive).OrderBy(d => d.Id).LastOrDefaultAsync();

            if (delivery is null)
                return null;

            return await GetDeliveryTrackInfoAsync(delivery.TrackNumber);
        }

        public async Task<PackageInfoDTO?> GetDeliveryPackageInfoAsync(string trackNumber)
        {
            var delivery = await _context.Deliveries.SingleOrDefaultAsync(d => d.TrackNumber == trackNumber);

            if (delivery is null)
                return null;

            var packageInfo = new PackageInfoDTO()
            {
                TrackNumber = delivery.TrackNumber,
                DestinationAddress = delivery.DestinationAddress,
                DestinationOthers = delivery.DestinationOthers,
                DestinationPhoneNumber = delivery.DestinationPhoneNumber,
                DestinationStateCountry = delivery.DestinationStateCountry,
                WorthOfItems = delivery.WorthOfItems,
                PackageWeight = delivery.PackageWeight,
                PackageName = delivery.PackageName,
                OriginStateCountry = delivery.OriginStateCountry,
                OriginPhoneNumber = delivery.OriginPhoneNumber,
                OriginAddress = delivery.OriginAddress,
                OriginOthers = delivery.OriginOthers,
                InstantDelivery = delivery.DeliveryType == DeliveryTypes.Instant ? 300 : null,
                DeliveryCharges = delivery.DeliveryCharges,
                Taxes = delivery.Taxes,
            };

            return packageInfo;
        }

        private async Task<UserBase> GetDelivererWithMinCountDelivery()
        {
            var users = await _userManager.GetUsersInRoleAsync(Roles.Deliverer.ToString()) ?? throw new Exception("Я не могу создать заказ! Доставщиков нет");

            var delivererWithMinCountDelivery = users.OrderBy(u => ((DelivererEntity)u).Deliveries.Count())
                .Last().Id;

            //var deliveries = await _context.Deliveries.GroupBy(x => x.Deliverier.Id)
            //    .ToArrayAsync();

            //string delivererWithMinCountDelivery = string.Empty;
            //int min = int.MaxValue;
            //foreach (var delivery in deliveries)
            //{
            //    var count = delivery.Count();
            //    if (min > count)
            //    {
            //        min = count;
            //        delivererWithMinCountDelivery = delivery.Key;
            //    }
            //}

            return await _userManager.FindByIdAsync(delivererWithMinCountDelivery);
        }

        public async Task<ServiceResult> UpdateDeliveryStatusAsync(UpdateDeliveryStatusDTO updateDelivery)
        {
            var result = new ServiceResult();

            var delivery = await _context.Deliveries.SingleOrDefaultAsync(d => d.Id == updateDelivery.DeliveryId);

            if (delivery is null)
            {
                result.Errors.Add("Доставка не найдена");
                return result;
            }

            delivery.OrderStates.Add(new DeliveryState()
            {
                Status = (DeliveryStatuses)updateDelivery.Status,
                CreatedDate = DateTime.UtcNow,
            });

            _context.Deliveries.Update(delivery);

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<IReadOnlyCollection<DeliveryDto>> GetDeliveriesAsync()
        {
            return await _context.Deliveries.Select(d => new DeliveryDto()
            {
                Id = d.Id,
                TrackNumber = d.TrackNumber,
            }).ToArrayAsync();
        }

        public async Task<string?> GetDeliveryClientAsync(int deliveryId)
        {
            var delivery = await _context.Deliveries.SingleOrDefaultAsync(d => d.Id == deliveryId);

            return delivery?.ClientId;
        }

        public async Task<ServiceResult> UpdateDeliveryStatusAsync(int deliveryId)
        {
            var result = new ServiceResult();

            var delivery = await _context.Deliveries.Include(d => d.OrderStates).SingleOrDefaultAsync(d => d.Id == deliveryId);

            if (delivery is null)
            {
                result.Errors.Add("Доставка не найдена");
                return result;
            }

            var currentStatus = delivery.OrderStates.Last().Status;

            var newStatus = currentStatus.Next();

            if (currentStatus == newStatus)
            {
                result.Errors.Add("Заказ уже находится на последнем статусе. Завершите его");
                return result;
            }

            delivery.OrderStates.Add(new DeliveryState()
            {
                Status = newStatus,
                CreatedDate = DateTime.Now,
            });

            _context.Deliveries.Update(delivery);

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<PackageTrackDTO?> GetDeliveryTrackInfoAsync(string trackNumber)
        {
            var delivery = await _context.Deliveries.SingleOrDefaultAsync(d => d.TrackNumber == trackNumber);

            if (delivery is null)
                return null;

            var destinationPoint = await AddressExtensions.GetLocationAsync($"{delivery.DestinationAddress}, {delivery.DestinationStateCountry}");
            var originPoint = await AddressExtensions.GetLocationAsync($"{delivery.OriginAddress}, {delivery.OriginStateCountry}");

            var statuses = await GetDeliveryStatuses(delivery.TrackNumber);

            var trackInfo = new PackageTrackDTO()
            {
                Id = delivery.Id,
                TrackNumber = delivery.TrackNumber,
                DestinationPoint = destinationPoint,
                OriginPoint = originPoint,
                Statuses = statuses
            };

            return trackInfo;
        }

        public async Task<string?> GetDeliveryTrackNumberAsync(int deliveryId)
        {
            var delivery = await _context.Deliveries.SingleOrDefaultAsync(d => d.Id == deliveryId);

            return delivery?.TrackNumber;
        }

        public async Task<IReadOnlyCollection<DeliveryStatus>> GetDeliveryStatuses(string trackNumber)
        {
            var delivery = await _context.Deliveries.Include(d => d.OrderStates)
                .SingleOrDefaultAsync(d => d.TrackNumber == trackNumber);

            if (delivery is null)
                return null;

            var statuses = delivery.OrderStates.Select(s => new DeliveryStatus()
            {
                CreatedDate = s.CreatedDate,
                Name = s.Status.Description(),
            }).ToArray();

            return statuses;
        }
    }
}