using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Redi.DataAccess.Data.Entities;
using Redi.DataAccess.Data.Entities.Users;

namespace Redi.DataAccess.Data
{
    public class RediDbContext : IdentityDbContext<UserBase>
    {
        public DbSet<Chat> Chats { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Delivery> Deliveries { get; set; }

        public DbSet<DeliveryType> DeliveryTypes { get; set; }

        public DbSet<DeliveryState> DeliveryStates { get; set; }

        public RediDbContext(DbContextOptions options) : base(options) { }
    }
}
