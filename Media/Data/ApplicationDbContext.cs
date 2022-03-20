using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Media.Models.Entity;

namespace Media.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Models.Entity.File>(x =>
            {
                x.HasKey(p => p.Id);
                x.Property(p=>p.Title).HasMaxLength(50);
                x.Property(p=>p.Format).HasMaxLength(10);
                x.HasIndex(p => p.Format);

            });
            builder.Entity<Folder>(x =>
            {
                x.HasIndex(p => new { p.ParentId, p.Title }).IsUnique();
            });
        }
        public DbSet<Models.Entity.File> Files { get; set; }
        public DbSet<FileType> FileTypes { get; set; }
        public DbSet<Folder> Folders { get; set; }
    }
}