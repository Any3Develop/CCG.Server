using CCG.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace CCG.Application.Contracts.Persistence
{
    public interface IAppDbContext
    {
        Task SaveChangesAsync();
        Task MigrateAsync();
        Task<IEnumerable<string>> GetPendingMigrationsAsync();
        DbSet<UserEntity> Users { get; }
    }
}