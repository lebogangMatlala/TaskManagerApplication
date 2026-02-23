using Microsoft.EntityFrameworkCore;
using TaskManagerApplication.Models;

namespace TaskManagerApplication.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure ProjectStatus enum to be stored as string
            modelBuilder.Entity<Project>()
                .Property(p => p.Status)
                .HasConversion<string>();

            modelBuilder.Entity<TaskItem>()
               .Property(t => t.Priority)
               .HasConversion<string>();

            // Store Status as string
            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Status)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }
    }
}