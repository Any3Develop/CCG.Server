using CCG.Application.Contracts.Persistence;
using CCG.Domain.Contracts;
using CCG.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CCG.Infrastructure.Persistence
{
    internal class AppDbContext : IdentityDbContext<UserEntity>, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
        
        public Task SaveChangesAsync()
        {
            UpdateTimestamps();
            return base.SaveChangesAsync();
        }

        public Task MigrateAsync()
        {
            return Database.MigrateAsync();
        }

        public Task<IEnumerable<string>> GetPendingMigrationsAsync()
        {
            return Database.GetPendingMigrationsAsync();
        }

        private void UpdateTimestamps()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(e => e.Entity is IEntity<string> && 
                            e.State is EntityState.Added or EntityState.Modified);

            foreach (var entry in modifiedEntries)
            {
                var entity = (IEntity<string>)entry.Entity;
                entity.Updated = DateTime.UtcNow;
            }
        }
    }
}