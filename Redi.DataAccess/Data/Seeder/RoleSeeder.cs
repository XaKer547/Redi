using Microsoft.AspNetCore.Identity;

namespace Redi.DataAccess.Data.Seeder
{
    public partial class DbSeeder
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public async Task SeedRolesAsync()
        {
            if (_roleManager.Roles.Any())
                return;

            var roles = Enum.GetValues(typeof(Roles));

            foreach (var role in roles)
            {
                var roleName = role.ToString();

                _roleManager.CreateAsync(new IdentityRole(roleName)).Wait();
            }

            await _context.SaveChangesAsync();
        }
    }
}
