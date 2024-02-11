using Redi.Domain.Services.Response;

namespace Redi.Domain.Services
{
    public interface IAccountManager
    {
        Task<ServiceResult> UpdateUserProfileAsync(string userId, string imagePath);
    }
}
