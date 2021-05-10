using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShortLink.Models
{
    public class LinkDBContext : DbContext
    {
        public LinkDBContext()
        {
        }

        public LinkDBContext(DbContextOptions<LinkDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Link> Links { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Link>(entity =>
            {
                entity.ToTable("Tbl_Link");
            });

        }

    }
}
