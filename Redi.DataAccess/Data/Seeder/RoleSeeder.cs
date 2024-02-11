using Microsoft.AspNetCore.Identity;

namespace Redi.DataAccess.Data.Seeder
{
    public partial class DbSeeder
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public async Task SeedRolesAsync()
        {
            var roles = Enum.GetValues(typeof(Roles));

            foreach (var role in roles)
            {
                var roleName = role.ToString();

                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = roleName,
                    NormalizedName = roleName.ToLower()
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
