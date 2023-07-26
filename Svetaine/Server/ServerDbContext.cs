using Microsoft.EntityFrameworkCore;
using Svetaine.Shared.DbModels;

namespace Svetaine.Server
{
    public class ServerDbContext : DbContext
    {

        public ServerDbContext() { }

        public DbSet<User> User { get; set; }
        public DbSet<ReserveTime> ReserveTime { get; set; }
        public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options) 
        {

        }
    }
}
