using Microsoft.EntityFrameworkCore;

namespace POLYGLOT.Project.Security.application
{
    public class SecurityContext : DbContext
    {
        public SecurityContext(DbContextOptions<SecurityContext> options) : base(options)
        {
        }

        //public DbSet<Models.Account> Account => Set<Models.Account>();
        //public DbSet<Models.Customer> Customer => Set<Models.Customer>();

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Models.Account>().ToTable("Account");
        //    modelBuilder.Entity<Models.Customer>().ToTable("Customer");
        //}
    }
}
