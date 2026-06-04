using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WpfApp1.Models;

public partial class PhoneBookDbSatinEi2307b2Context : DbContext
{
    public PhoneBookDbSatinEi2307b2Context()
    {
    }

    public PhoneBookDbSatinEi2307b2Context(DbContextOptions<PhoneBookDbSatinEi2307b2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Contact> Contacts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=dbsrv\\ov2025;Initial Catalog=PhoneBookDB_SatinEI_2307b2;Integrated Security=true;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contacts__3214EC0707C64D03");

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
