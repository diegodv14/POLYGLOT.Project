using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using POLYGLOT.Project.Security.application.Models;

namespace POLYGLOT.Project.Security.application.Models
{
    public class DbSecurityContextFactory : IDesignTimeDbContextFactory<DbSecurityContext>
    {
        public DbSecurityContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbSecurityContext>();
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=db_security;User Id=sa;Password=Pass@word;TrustServerCertificate=true");

            return new DbSecurityContext(optionsBuilder.Options);
        }
    }
} 