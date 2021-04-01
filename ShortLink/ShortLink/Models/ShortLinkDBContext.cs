using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ShortLink.Models
{
    public partial class ShortLinkDBContext : DbContext
    {
        public ShortLinkDBContext()
        {
        }

        public ShortLinkDBContext(DbContextOptions<ShortLinkDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Page> Pages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Arabic_CI_AS");

            modelBuilder.Entity<Page>(entity =>
            {
                entity.ToTable("Page");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Link).IsRequired();

                entity.Property(e => e.ShortKey)
                    .IsRequired()
                    .HasMaxLength(6);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
