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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=localhost;port=3306;database=db_operation;user=securityPrueba;password=security;sslmode=Preferred");

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
