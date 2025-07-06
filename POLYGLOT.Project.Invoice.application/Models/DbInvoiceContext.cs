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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.IdInvoice).HasName("invoices_pkey");

            entity.ToTable("invoices");

            entity.Property(e => e.IdInvoice)
                .HasDefaultValueSql("nextval('invoices_idinvoice_seq'::regclass)")
                .HasColumnName("idInvoice");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Paid).HasColumnName("paid");
            entity.Property(e => e.Secuencial).HasColumnName("secuencial");
            entity.Property(e => e.State).HasColumnName("state");
        });
        modelBuilder.HasSequence("invoices_idinvoice_seq");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
