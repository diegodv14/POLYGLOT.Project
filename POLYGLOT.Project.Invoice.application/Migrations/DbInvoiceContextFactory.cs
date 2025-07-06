using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using POLYGLOT.Project.Invoice.application.Models;

namespace POLYGLOT.Project.Invoice.application.Migrations
{
    public class DbInvoiceContextFactory : IDesignTimeDbContextFactory<DbInvoiceContext>
    {
        public DbInvoiceContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbInvoiceContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=db_invoice;Username=postgres;Password=password");

            return new DbInvoiceContext(optionsBuilder.Options);
        }
    }
} 