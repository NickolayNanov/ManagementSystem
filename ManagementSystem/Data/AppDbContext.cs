using ManagementSystem.Models;
using ManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ManagementSystem.Data
{
    public class AppDbContext : IdentityDbContext<DbUser, DbRole, string>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            var entries = this.ChangeTracker.Entries().Where(x => x.Entity is IDeletable && x.State == EntityState.Deleted).ToList();

            foreach (EntityEntry entry in entries)
            {
                var entity = (IDeletable)entry.Entity;

                entity.DateDeleted = DateTime.Now;
                entity.IsDeleted = true;
                entry.State = EntityState.Modified;
            }

            return await base.SaveChangesAsync();
        }
    }
}
