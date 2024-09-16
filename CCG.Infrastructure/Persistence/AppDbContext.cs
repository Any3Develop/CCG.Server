using CCG.Application.Contracts.Persistence;
using CCG.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CCG.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<UserEntity>, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
        
        public Task SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}