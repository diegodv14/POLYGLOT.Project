using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace POLYGLOT.Project.Invoice.application.Models;

public partial class DbInvoiceContext : DbContext
{
    public DbInvoiceContext()
    {
    }

    public DbInvoiceContext(DbContextOptions<DbInvoiceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Invoice> Invoices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=db_invoice;Username=invoicePrueba;Password=prueba;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.IdInvoice).HasName("invoices_pkey");

            entity.ToTable("invoices");

            entity.Property(e => e.IdInvoice)
                .ValueGeneratedNever()
                .HasColumnName("idInvoice");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.State)
                .HasDefaultValueSql("NULL::\"bit\"")
                .HasColumnType("bit(1)")
                .HasColumnName("state");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
