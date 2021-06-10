using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Server.Models;

#nullable disable

namespace Server.Data
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Factory> Factorys { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=somePassword;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<Factory>(entity =>
            {
                entity.ToTable("factorys");

                entity.Property(e => e.FactoryId).HasColumnName("factory_id");

                entity.Property(e => e.Factoryname)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("factoryname");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.IntegrationService).HasColumnName("integration_service");

                entity.Property(e => e.InterfaceService).HasColumnName("interface_service");

                entity.Property(e => e.Rolename)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("rolename");

                entity.Property(e => e.TagService).HasColumnName("tag_service");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FactoryId).HasColumnName("factory_id");

                entity.Property(e => e.Passwordhash)
                    .IsRequired()
                    .HasColumnName("passwordhash");

                entity.Property(e => e.Passwordsalt)
                    .IsRequired()
                    .HasColumnName("passwordsalt");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("username");

                entity.HasOne(d => d.Factory)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.FactoryId)
                    .HasConstraintName("factorys_fk");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("users_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
