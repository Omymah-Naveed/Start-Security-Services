using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Star_Security_Service.Models;

namespace Star_Security_Service.Data
{
    public partial class Star_security_ServiceContext : DbContext
    {
        public Star_security_ServiceContext()
        {
        }

        public Star_security_ServiceContext(DbContextOptions<Star_security_ServiceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdminRegisteration> AdminRegisterations { get; set; } = null!;
        public virtual DbSet<AdminRegisterationRole> AdminRegisterationRoles { get; set; } = null!;
        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<Contact> Contacts { get; set; } = null!;
        public virtual DbSet<EmployeeInformation> EmployeeInformations { get; set; } = null!;
        public virtual DbSet<EmployeeService> EmployeeServices { get; set; } = null!;
        public virtual DbSet<MannedGuarding> MannedGuardings { get; set; } = null!;
        public virtual DbSet<Network> Networks { get; set; } = null!;
        public virtual DbSet<RegisterationUser> RegisterationUsers { get; set; } = null!;
        public virtual DbSet<Testimonial> Testimonials { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Initial Catalog=Star_security_Service;Persist Security Info=False;User ID=sa;Password=aptech;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminRegisteration>(entity =>
            {
                entity.ToTable("admin_registeration");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.AdminRegisterations)
                    .HasForeignKey(d => d.Role)
                    .HasConstraintName("FK__admin_regi__Role__34C8D9D1");
            });

            modelBuilder.Entity<AdminRegisterationRole>(entity =>
            {
                entity.ToTable("admin_registeration_role");

                entity.Property(e => e.Role)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Booking");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BookingDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("booking_datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.ServiceId).HasColumnName("service_id");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Booking__employe__35BCFE0A");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Booking__service__36B12243");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contact");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Message)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("message");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Phonenumber)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("phonenumber");

                entity.Property(e => e.Subject)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("subject");
            });

            modelBuilder.Entity<EmployeeInformation>(entity =>
            {
                entity.ToTable("employee_information");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Achievements)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("achievements");

                entity.Property(e => e.Action)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("action");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("address");

                entity.Property(e => e.Client)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("client");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Grade)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("grade");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Phonenumber)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("phonenumber");

                entity.Property(e => e.Qualification)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("qualification");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.EmployeeInformations)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK__employee___Servi__37A5467C");
            });

            modelBuilder.Entity<EmployeeService>(entity =>
            {
                entity.ToTable("Employee_Service");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeServices)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__Employee___Emplo__38996AB5");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.EmployeeServices)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK__Employee___Servi__398D8EEE");
            });

            modelBuilder.Entity<MannedGuarding>(entity =>
            {
                entity.ToTable("Manned_Guarding");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Image_Path");

                entity.Property(e => e.Items)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("items");

                entity.Property(e => e.Title)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Network>(entity =>
            {
                entity.ToTable("Network");

                entity.Property(e => e.Cell)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Location)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RegisterationUser>(entity =>
            {
                entity.ToTable("Registeration_User");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Testimonial>(entity =>
            {
                entity.ToTable("Testimonial");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Remarks)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
