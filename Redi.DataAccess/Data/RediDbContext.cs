using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Redi.DataAccess.Data
{
    public class RediDbContext : IdentityDbContext
    {
        public RediDbContext(DbContextOptions options) : base(options) { }
    }
}
