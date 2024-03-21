using Microsoft.EntityFrameworkCore;

namespace DEMLEYCDAMMAGSMC20240321.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Components> Components { get; set; }
        public DbSet<Computers> Computers { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Users> Users { get; set; }
    }

}
