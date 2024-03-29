﻿using Redi.Domain.Models.Delivery;
using Redi.Domain.Services.Response;

namespace Redi.Domain.Services
{
    public interface IDeliveryService
    {
        Task<ServiceResult> CreateDeliveryAsync(string clientId, CreateDeliveryDto deliveryDto);
        Task<ServiceResult> EndDeliveryAsync(string trackNumber);
        Task<ServiceResult> EndDeliveryAsync(int deliveryId);
        Task<bool> IsDeliveryClientAsync(string clientId, string trackNumber);
        Task<bool> ExistsAsync(string trackNumber);
        Task<PackageTrackDTO?> GetLastAvaibleDeliveryAsync(string userId);
        Task<PackageTrackDTO?> GetDeliveryTrackInfoAsync(string trackNumber);
        Task<PackageInfoDTO?> GetDeliveryPackageInfoAsync(string trackNumber);
        Task<ServiceResult> UpdateDeliveryStatusAsync(UpdateDeliveryStatusDTO updateDelivery);
        Task<ServiceResult> UpdateDeliveryStatusAsync(int deliveryId);
        Task<IReadOnlyCollection<DeliveryDto>> GetDeliveriesAsync();
        Task<string?> GetDeliveryClientAsync(int deliveryId);
        Task<string?> GetDeliveryTrackNumberAsync(int deliveryId);
        Task<IReadOnlyCollection<DeliveryStatus>> GetDeliveryStatuses(string trackNumber);
    }
}
