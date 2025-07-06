using Microsoft.EntityFrameworkCore;

namespace POLYGLOT.Project.Pay.application.Models;

public partial class DbOperationContext : DbContext
{
    public DbOperationContext()
    {
    }

    public DbOperationContext(DbContextOptions<DbOperationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Operation> Operations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Operation>(entity =>
        {
            entity.HasKey(e => e.IdOperation).HasName("PRIMARY");

            entity.ToTable("operation");

            entity.Property(e => e.IdOperation).HasColumnName("idOperation");
            entity.Property(e => e.Amount)
                .HasPrecision(20, 6)
                .HasColumnName("amount");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.IdInvoice).HasColumnName("idInvoice");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
