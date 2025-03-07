﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MathApp.Models;

public partial class MathDbContext : DbContext
{
    public MathDbContext()
    {
    }

    public MathDbContext(DbContextOptions<MathDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MathCalculation> MathCalculations { get; set; }

    /*
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=EBRAHIM-LAPTOP;Database=Math_DB;Trusted_Connection=True;TrustServerCertificate=True;");
    */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MathCalculation>(entity =>
        {
            entity.HasKey(e => e.CalculationId).HasName("PK__MathCalc__57C05F66C31C449B");

            entity.Property(e => e.CalculationId).HasColumnName("CalculationID");
            entity.Property(e => e.FirstNumber).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Result).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SecondNumber).HasColumnType("decimal(18, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
