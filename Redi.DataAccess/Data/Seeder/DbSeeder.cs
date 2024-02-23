using Microsoft.AspNetCore.Identity;
using Redi.DataAccess.Data.Entities.Users;

namespace Redi.DataAccess.Data.Seeder
{
    public partial class DbSeeder
    {
        private readonly RediDbContext _context;
        private readonly UserManager<UserBase> _userManager;
        public DbSeeder(UserManager<UserBase> userManager, RediDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        public async void SeedAll()
        {
            await SeedRolesAsync();

            await SeedDeliverersAsync();
        }

        private class SeedableUser<TUser> where TUser : UserBase
        {
            public TUser User { get; set; }
            public string Password { get; set; }
        }
    }
}
