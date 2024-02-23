using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Redi.DataAccess.Data;
using Redi.DataAccess.Data.Entities.Users;
using Redi.Domain.Models.Account;
using Redi.Domain.Services;
using Redi.Domain.Services.Response;

namespace Redi.Application.Services
{
    public class AccountManager : IAccountManager
    {
        private readonly RediDbContext _context;
        private readonly UserManager<UserBase> _userManager;
        public AccountManager(RediDbContext context, UserManager<UserBase> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IReadOnlyCollection<TransactionDTO>?> GetTransactionHistoryAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            var isInRole = await _userManager.IsInRoleAsync(user, Roles.Client.ToString());

            if (!isInRole)
                return null;

            var client = (ClientEntity)await _context.Users.Include(u => ((ClientEntity)u).Transactions)
                .SingleAsync(u => u.Id == user.Id);

            var transactions = client.Transactions.Select(t => new TransactionDTO()
            {
                Id = t.Id,
                Name = t.Name,
                Date = t.Date,
                Money = t.Money,
            }).ToArray();

            return transactions;
        }

        public async Task IncreaseBalanceAsync(string userId)
        {
            var users = await _userManager.GetUsersInRoleAsync(Roles.Client.ToString());

            var user = (ClientEntity)users.SingleOrDefault(u => u.Id == userId);

            if (user is null)
            {
                return;
            }

            user.Balance += 250;

            _context.Update(user);

            await _context.SaveChangesAsync();
        }

        public async Task IncreaseBalanceAsync(float money)
        {
            var users = await _userManager.GetUsersInRoleAsync(Roles.Client.ToString());

            foreach (var user in users.Cast<ClientEntity>())
                user.Balance += money;

            _context.Update(users);

            await _context.SaveChangesAsync();
        }

        public async Task<ServiceResult> UpdateUserProfileAsync(string userId, string imagePath)
        {
            var result = new ServiceResult();

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                result.Errors.Add("Пользователь не найден");
                return result;
            }

            if (user.Picture != null)
                File.Delete(user.Picture);

            user.Picture = imagePath;

            _context.Users.Update(user);

            await _context.SaveChangesAsync();

            return result;
        }
    }
}
