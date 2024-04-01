using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication52.Models;

public partial class MyDBContext : DbContext
{
    public MyDBContext()
    {
    }

    public MyDBContext(DbContextOptions<MyDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<FixReason> FixReasons { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Solution> Solutions { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<VDevice> VDevices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
    { 
    
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>(entity =>
        {
            entity.ToTable("Device");

            entity.Property(e => e.EmpId)
                .HasMaxLength(7)
                .HasColumnName("EmpID");
            entity.Property(e => e.FixEmpId)
                .HasMaxLength(7)
                .HasColumnName("FixEmpID");
            entity.Property(e => e.Remark).HasMaxLength(50);
        });

        modelBuilder.Entity<FixReason>(entity =>
        {
            entity.ToTable("FixReason");

            entity.Property(e => e.FixReasonDesc).HasMaxLength(50);
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK_Category");

            entity.ToTable("Location");

            entity.Property(e => e.LocationDesc).HasMaxLength(50);
        });

        modelBuilder.Entity<Solution>(entity =>
        {
            entity.ToTable("Solution");

            entity.Property(e => e.SolutionDesc).HasMaxLength(50);
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("Status");

            entity.Property(e => e.StatusDesc).HasMaxLength(50);
        });

        modelBuilder.Entity<VDevice>(entity =>
        {
            entity
                .ToView("v_Device");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.備註).HasMaxLength(50);
            entity.Property(e => e.問題原因).HasMaxLength(50);
            entity.Property(e => e.報修人員工號).HasMaxLength(7);
            entity.Property(e => e.報修原因).HasMaxLength(50);
            entity.Property(e => e.維修人員工號).HasMaxLength(7);
            entity.Property(e => e.維修進度).HasMaxLength(50);
            entity.Property(e => e.設備位置).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
