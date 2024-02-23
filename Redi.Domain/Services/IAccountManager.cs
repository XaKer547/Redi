using Redi.Domain.Models.Account;
using Redi.Domain.Services.Response;

namespace Redi.Domain.Services
{
    public interface IAccountManager
    {
        Task<ServiceResult> UpdateUserProfileAsync(string userId, string imagePath);
        Task IncreaseBalanceAsync(string userId);
        Task IncreaseBalanceAsync(float money);
        Task<IReadOnlyCollection<TransactionDTO>?> GetTransactionHistoryAsync(string id);
    }
}
