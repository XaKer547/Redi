using Microsoft.AspNetCore.SignalR;
using Redi.Domain.Models.Delivery;

namespace Redi.Api.Hubs
{
    public class DeliveryHub : Hub
    {
        public async Task UpdatePackageStatus(string userId, string trackNumber, IReadOnlyCollection<DeliveryStatus> statuses)
        {
            await Clients.User(userId).SendAsync("UpdatePackageStatus", trackNumber, statuses);
        }
    }
}
