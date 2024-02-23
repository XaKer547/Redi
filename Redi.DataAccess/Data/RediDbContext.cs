using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Redi.DataAccess.Data.Entities;
using Redi.DataAccess.Data.Entities.Users;

namespace Redi.DataAccess.Data
{
    public class RediDbContext : IdentityDbContext<UserBase>
    {
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<DeliveryState> DeliveryStates { get; set; }

        public RediDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Chat>()
                .HasOne(x => x.Deliverier)
                .WithMany(x => x.Chats)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Chat>()
                .HasOne(x => x.Client)
                .WithMany(x => x.Chats)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Delivery>()
                .HasOne(x => x.Client)
                .WithMany(x => x.Deliveries)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Delivery>()
                .HasOne(x => x.Deliverier)
                .WithMany(x => x.Deliveries)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }
    }
}
