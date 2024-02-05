using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Redi.DataAccess.Data.Entities;

namespace Redi.DataAccess.Data
{
    public class RediDbContext : IdentityDbContext<User>
    {
        public RediDbContext(DbContextOptions options) : base(options) { }
    }
}
