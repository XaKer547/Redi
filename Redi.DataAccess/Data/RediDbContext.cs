using Microsoft.EntityFrameworkCore;

namespace Redi.DataAccess.Data
{
    public class RediDbContext : DbContext
    {
        public RediDbContext(DbContextOptions options) : base(options) { }
    }
}
