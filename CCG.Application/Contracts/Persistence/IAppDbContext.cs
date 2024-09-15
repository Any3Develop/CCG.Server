using CCG.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace CCG.Application.Contracts.Persistence
{
    public interface IAppDbContext
    {
        Task SaveChangesAsync();
        DbSet<UserEntity> Users { get; }
    }
}