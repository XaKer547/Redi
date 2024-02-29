using Redi.DataAccess.Data.Entities;
using Redi.DataAccess.Data.Entities.Users;

namespace Redi.DataAccess.Data.Seeder
{
    public partial class DbSeeder
    {
        public async Task SeedClientsAsync()
        {
            var clients = new List<SeedableUser<ClientEntity>>()
            {
                new()
                {
                    User = new ClientEntity()
                    {
                        UserName = "Joleen Yankishin",
                        Email = "jyankishin1@merriam-webster.com",
                        Balance = 554.14,
                        Cards = new List<Card>()
                        {
                            new()
                            {
                                Code = "6375808342823810"
                            }
                        },
                        PhoneNumber = "+627988981861",
                    },
                    Password="5PBAhB"
                }
            };

            var role = Roles.Client.ToString();
            foreach (var client in clients)
            {
                var user = client.User;

                await _userManager.CreateAsync(user, client.Password);

                await _userManager.AddToRoleAsync(user, role);
            }

            await _context.SaveChangesAsync();
        }
    }
}
