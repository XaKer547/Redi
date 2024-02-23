using Redi.DataAccess.Data.Entities.Users;

namespace Redi.DataAccess.Data.Seeder
{
    public partial class DbSeeder
    {
        public async Task SeedDeliverersAsync()
        {
            var role = Roles.Deliverer.ToString();

            var users = _userManager.GetUsersInRoleAsync(role).GetAwaiter().GetResult();

            if (users.Any())
                return;

            var deliverers = new List<SeedableUser<DelivererEntity>>
            {
                new()
                {
                    User = new DelivererEntity
                    {
                       UserName = "Erminie Capini",
                       Email = "ecapini0@japanpost.jp",
                       Picture = @"\profiles\2eaabacf-5b97-4a07-b58d-2206e801d545.png",
                       PhoneNumber = "+3534668223089"
                    },
                    Password = "qKPGUj"
                },
                new()
                {
                    User = new DelivererEntity
                    {
                       UserName = "Dalston Batteson",
                       Email = "dbattesong@state.tx.us",
                       Picture = @"\profiles\32c3b22a-1dad-4fc8-b960-92ffcb931e05.png",
                       PhoneNumber = "+559519728215"
                    },
                    Password = "thMgNwS)ZyB"
                },
                new()
                {
                    User = new DelivererEntity
                    {
                       UserName = "Herrick Exroll",
                       Email = "hexrollp@uol.com.br",
                       Picture = @"\profiles\4e957435-741d-47dd-b3ae-eb7dec330674.png",
                       PhoneNumber = "+626544388496"
                    },
                    Password = "v1AQnaed{H0"
                },
                new()
                {
                    User = new DelivererEntity
                    {
                       UserName = "Klement Ever",
                       Email = "kever10@marketwatch.com",
                       Picture = @"\profiles\2aavbacf-1b9f-4a07-b58d-220ae801d545.png",
                       PhoneNumber = "+865238652768"
                    },
                    Password = "Vq4Ue%pDI"
                },
            };

            foreach (var deliverer in deliverers)
            {
                var user = deliverer.User;

                _userManager.CreateAsync(user, deliverer.Password).Wait();

                _userManager.AddToRoleAsync(user, role).Wait();
            }

            await _context.SaveChangesAsync();
        }
    }
}
