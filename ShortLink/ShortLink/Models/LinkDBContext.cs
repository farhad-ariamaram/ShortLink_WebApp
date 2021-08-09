using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ShortLink.Models
{
    public partial class LinkDbContext : DbContext
    {
        public LinkDbContext()
        {
        }

        public LinkDbContext(DbContextOptions<LinkDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblLink> TblLinks { get; set; }
        public virtual DbSet<TblSmsSent> TblSmsSents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=EmployDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Persian_100_CI_AI");

            modelBuilder.Entity<TblLink>(entity =>
            {
                entity.ToTable("Tbl_Link");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .UseCollation("Persian_100_CI_AS");

                entity.Property(e => e.ExpireDate).HasColumnType("datetime");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .UseCollation("Persian_100_CI_AS");
            });

            modelBuilder.Entity<TblSmsSent>(entity =>
            {
                entity.ToTable("Tbl_SmsSent");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Message)
                    .HasMaxLength(1000)
                    .UseCollation("Persian_100_CI_AS");

                entity.Property(e => e.Phone)
                    .HasMaxLength(1000)
                    .UseCollation("Persian_100_CI_AS");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
