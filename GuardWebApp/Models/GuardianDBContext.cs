﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace GuardWebApp.Models
{
    public partial class GuardianDBContext : DbContext
    {
        public GuardianDBContext()
        {
        }

        public GuardianDBContext(DbContextOptions<GuardianDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Check> Checks { get; set; }
        public virtual DbSet<CheckLocation> CheckLocations { get; set; }
        public virtual DbSet<CheckLocationVisittime> CheckLocationVisittimes { get; set; }
        public virtual DbSet<Climate> Climates { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<GuardArea> GuardAreas { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LocationDetail> LocationDetails { get; set; }
        public virtual DbSet<Plan> Plans { get; set; }
        public virtual DbSet<Rhythm> Rhythms { get; set; }
        public virtual DbSet<RhythmDetail> RhythmDetails { get; set; }
        public virtual DbSet<Run> Runs { get; set; }
        public virtual DbSet<RunDetail> RunDetails { get; set; }
        public virtual DbSet<Shift> Shifts { get; set; }
        public virtual DbSet<ShiftAllocation> ShiftAllocations { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<SubmittedLocation> SubmittedLocations { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Violation> Violations { get; set; }
        public virtual DbSet<ViolationConsequence> ViolationConsequences { get; set; }
        public virtual DbSet<ViolationNature> ViolationNatures { get; set; }
        public virtual DbSet<ViolationType> ViolationTypes { get; set; }
        public virtual DbSet<Visittime> Visittimes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=GuardianDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Check>(entity =>
            {
                entity.ToTable("Check");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CheckLocation>(entity =>
            {
                entity.ToTable("CheckLocation");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Check)
                    .WithMany(p => p.CheckLocations)
                    .HasForeignKey(d => d.CheckId)
                    .HasConstraintName("FK_CheckLocation_Check");

                entity.HasOne(d => d.Climate)
                    .WithMany(p => p.CheckLocations)
                    .HasForeignKey(d => d.ClimateId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_CheckLocation_Climate");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.CheckLocations)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_CheckLocation_Location");
            });

            modelBuilder.Entity<CheckLocationVisittime>(entity =>
            {
                entity.ToTable("CheckLocationVisittime");

                entity.HasOne(d => d.CheckLocation)
                    .WithMany(p => p.CheckLocationVisittimes)
                    .HasForeignKey(d => d.CheckLocationId)
                    .HasConstraintName("FK_CheckLocationVisittime_CheckLocation");

                entity.HasOne(d => d.Visittime)
                    .WithMany(p => p.CheckLocationVisittimes)
                    .HasForeignKey(d => d.VisittimeId)
                    .HasConstraintName("FK_CheckLocationVisittime_Visittime");
            });

            modelBuilder.Entity<Climate>(entity =>
            {
                entity.ToTable("Climate");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.ToTable("Device");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<GuardArea>(entity =>
            {
                entity.ToTable("GuardArea");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Nfc)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("NFC");

                entity.Property(e => e.Qr)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("QR");

                entity.HasOne(d => d.GuardArea)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.GuardAreaId)
                    .HasConstraintName("FK_Location_GuardArea");
            });

            modelBuilder.Entity<LocationDetail>(entity =>
            {
                entity.ToTable("LocationDetail");

                entity.HasOne(d => d.CheckLocation)
                    .WithMany(p => p.LocationDetails)
                    .HasForeignKey(d => d.CheckLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LocationDetail_CheckLocation");

                entity.HasOne(d => d.Climate)
                    .WithMany(p => p.LocationDetails)
                    .HasForeignKey(d => d.ClimateId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_LocationDetail_Climate");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.LocationDetails)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_LocationDetail_Location");
            });

            modelBuilder.Entity<Plan>(entity =>
            {
                entity.ToTable("Plan");

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Plans)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Plan_Location");

                entity.HasOne(d => d.Shift)
                    .WithMany(p => p.Plans)
                    .HasForeignKey(d => d.ShiftId)
                    .HasConstraintName("FK_Plan_Shift");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Plans)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Plan_User");
            });

            modelBuilder.Entity<Rhythm>(entity =>
            {
                entity.ToTable("Rhythm");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.GuardArea)
                    .WithMany(p => p.Rhythms)
                    .HasForeignKey(d => d.GuardAreaId)
                    .HasConstraintName("FK_Rhythm_GuardArea");
            });

            modelBuilder.Entity<RhythmDetail>(entity =>
            {
                entity.ToTable("RhythmDetail");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.RhythmDetails)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RhythmDetail_Location");

                entity.HasOne(d => d.Rhythm)
                    .WithMany(p => p.RhythmDetails)
                    .HasForeignKey(d => d.RhythmId)
                    .HasConstraintName("FK_RhythmDetail_Rhythm");
            });

            modelBuilder.Entity<Run>(entity =>
            {
                entity.ToTable("Run");

                entity.HasOne(d => d.Approver)
                    .WithMany(p => p.RunApprovers)
                    .HasForeignKey(d => d.ApproverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Run_User1");

                entity.HasOne(d => d.Plan)
                    .WithMany(p => p.Runs)
                    .HasForeignKey(d => d.PlanId)
                    .HasConstraintName("FK_Run_Plan");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Runs)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_Run_Status");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RunUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Run_User");
            });

            modelBuilder.Entity<RunDetail>(entity =>
            {
                entity.ToTable("RunDetail");

                entity.HasOne(d => d.LocationDetail)
                    .WithMany(p => p.RunDetails)
                    .HasForeignKey(d => d.LocationDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RunDetail_LocationDetail");

                entity.HasOne(d => d.Run)
                    .WithMany(p => p.RunDetails)
                    .HasForeignKey(d => d.RunId)
                    .HasConstraintName("FK_RunDetail_Run");
            });

            modelBuilder.Entity<Shift>(entity =>
            {
                entity.ToTable("Shift");

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.HasOne(d => d.GuardArea)
                    .WithMany(p => p.Shifts)
                    .HasForeignKey(d => d.GuardAreaId)
                    .HasConstraintName("FK_Shift_GuardArea");

                entity.HasOne(d => d.Rhythm)
                    .WithMany(p => p.Shifts)
                    .HasForeignKey(d => d.RhythmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shift_Rhythm");
            });

            modelBuilder.Entity<ShiftAllocation>(entity =>
            {
                entity.ToTable("ShiftAllocation");

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.HasOne(d => d.GuardArea)
                    .WithMany(p => p.ShiftAllocations)
                    .HasForeignKey(d => d.GuardAreaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShiftAllocation_GuardArea");

                entity.HasOne(d => d.Rhythm)
                    .WithMany(p => p.ShiftAllocations)
                    .HasForeignKey(d => d.RhythmId)
                    .HasConstraintName("FK_ShiftAllocation_Rhythm");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SubmittedLocation>(entity =>
            {
                entity.ToTable("SubmittedLocation");

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.SubmittedLocations)
                    .HasForeignKey(d => d.DeviceId)
                    .HasConstraintName("FK_SubmittedLocation_Device");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.SubmittedLocations)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_SubmittedLocation_Location");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SubmittedLocations)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_SubmittedLocation_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(3000);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(3000);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Violation>(entity =>
            {
                entity.ToTable("Violation");

                entity.Property(e => e.ApproveDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.RegisterDate).HasColumnType("datetime");

                entity.HasOne(d => d.Approver)
                    .WithMany(p => p.ViolationApprovers)
                    .HasForeignKey(d => d.ApproverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Violation_User1");

                entity.HasOne(d => d.Registrar)
                    .WithMany(p => p.ViolationRegistrars)
                    .HasForeignKey(d => d.RegistrarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Violation_User");

                entity.HasOne(d => d.Run)
                    .WithMany(p => p.Violations)
                    .HasForeignKey(d => d.RunId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Violation_Run");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Violations)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_Violation_Status");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ViolationUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Violation_User2");

                entity.HasOne(d => d.ViolationType)
                    .WithMany(p => p.Violations)
                    .HasForeignKey(d => d.ViolationTypeId)
                    .HasConstraintName("FK_Violation_ViolationType");
            });

            modelBuilder.Entity<ViolationConsequence>(entity =>
            {
                entity.ToTable("ViolationConsequence");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.ViolationType)
                    .WithMany(p => p.ViolationConsequences)
                    .HasForeignKey(d => d.ViolationTypeId)
                    .HasConstraintName("FK_ViolationConsequence_ViolationType");
            });

            modelBuilder.Entity<ViolationNature>(entity =>
            {
                entity.ToTable("ViolationNature");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ViolationType>(entity =>
            {
                entity.ToTable("ViolationType");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.ViolationNature)
                    .WithMany(p => p.ViolationTypes)
                    .HasForeignKey(d => d.ViolationNatureId)
                    .HasConstraintName("FK_ViolationType_ViolationNature");
            });

            modelBuilder.Entity<Visittime>(entity =>
            {
                entity.ToTable("Visittime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
