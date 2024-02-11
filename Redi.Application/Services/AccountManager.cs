using Microsoft.AspNetCore.Identity;
using Redi.DataAccess.Data;
using Redi.DataAccess.Data.Entities.Users;
using Redi.Domain.Services;
using Redi.Domain.Services.Response;

namespace Redi.Application.Services
{
    public class AccountManager : IAccountManager
    {
        private readonly UserManager<UserBase> _userManager;
        private readonly RediDbContext _context;
        public AccountManager(RediDbContext context, UserManager<UserBase> userManager)
        {
            _context = context;
            _userManager = userManager;
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

            user.Picture = imagePath;

            _context.Users.Update(user);

            await _context.SaveChangesAsync();

            return result;
        }
    }
}
