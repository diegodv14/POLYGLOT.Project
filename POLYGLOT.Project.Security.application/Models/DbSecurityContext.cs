using Microsoft.EntityFrameworkCore;

namespace POLYGLOT.Project.Security.application.Models;

public partial class DbSecurityContext : DbContext
{
    public DbSecurityContext()
    {
    }

    public DbSecurityContext(DbContextOptions<DbSecurityContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__users__3717C98218772A37");

            entity.ToTable("users");

            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
