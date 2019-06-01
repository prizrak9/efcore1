using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace efcore1.Models
{
    public partial class providerContext : DbContext
    {
        public providerContext()
        {
        }

        public providerContext(DbContextOptions<providerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bills> Bills { get; set; }
        public virtual DbSet<Services> Services { get; set; }
        public virtual DbSet<UserGroups> UserGroups { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UsersToServices> UsersToServices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Server=172.18.0.1;Port=11;Database=provider;User Id=postgres;Password=admin;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Bills>(entity =>
            {
                entity.ToTable("bills");

                entity.HasIndex(e => e.Id)
                    .HasName("bills_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasColumnType("numeric");

                entity.Property(e => e.DueDate)
                    .HasColumnName("due_date")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.IsPaid).HasColumnName("is_paid");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("bills_users_id_fk");
            });

            modelBuilder.Entity<Services>(entity =>
            {
                entity.ToTable("services");

                entity.HasIndex(e => e.Id)
                    .HasName("services_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasColumnType("numeric");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<UserGroups>(entity =>
            {
                entity.ToTable("user_groups");

                entity.HasIndex(e => e.Id)
                    .HasName("user-groups_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('\"user-groups_id_seq\"'::regclass)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Id)
                    .HasName("users_id_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.Nickname)
                    .HasName("users_nickname_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FullName)
                    .HasColumnName("full_name")
                    .HasMaxLength(255);

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.Nickname)
                    .IsRequired()
                    .HasColumnName("nickname")
                    .HasMaxLength(45);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(255);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("users_user-groups_id_fk");
            });

            modelBuilder.Entity<UsersToServices>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.ServiceId })
                    .HasName("users_to_services_user_id_service_id_pk");

                entity.ToTable("users_to_services");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.ServiceId).HasColumnName("service_id");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.UsersToServices)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("users_to_services_services_id_fk");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UsersToServices)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("users_to_services_users_id_fk");
            });

            modelBuilder.HasSequence<int>("user-groups_id_seq");
        }
    }
}
