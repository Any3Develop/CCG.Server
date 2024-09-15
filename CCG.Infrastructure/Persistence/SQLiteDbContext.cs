using CCG.Application.Contracts.Persistence;
using CCG.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CCG.Infrastructure.Persistence
{
    public class SqLiteDbContext : IdentityDbContext<UserEntity>, IAppDbContext
    {
        public Task SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}