using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TokenManager.Api.Models.DB
{
    public partial class MyTestAuthAppContext : DbContext
    {
        public MyTestAuthAppContext()
        {
        }

        public MyTestAuthAppContext(DbContextOptions<MyTestAuthAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppUser> AppUser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=MyTestAuthApp;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.MoblieNumber).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(300);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });
        }
    }
}
