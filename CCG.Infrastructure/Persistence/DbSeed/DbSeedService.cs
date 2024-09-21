using CCG.Application;
using CCG.Domain.Entities.Identity;
using CCG.Shared.Common.Logger;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CCG.Infrastructure.Persistence.DbSeed
{
    public class DbSeedService(
            RoleManager<IdentityRole> roleManager,
            UserManager<UserEntity> userManager,
            AppDbContext dbContext)
        : IDbSeedService
    {
        public async Task Seed()
        {
            await CreateRoles();
        }

        public Task CleanUp() => Task.CompletedTask;

        public async Task Migrate()
        {
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
                await dbContext.Database.MigrateAsync();
        }

        private async Task CreateRoles()
        {
            try
            {
                var roleNames = new[] {Constants.AdminRole, "Trusted-Player", "Player"};
                foreach (var roleName in roleNames)
                {
                    var existsAsync = await roleManager.RoleExistsAsync(roleName);
                    if (existsAsync)
                        continue;

                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }

                await CreateUser(Constants.AdminEmail, Constants.AdminNick, Constants.AdminPass, Constants.AdminRole);
            }
            catch (Exception e)
            {
                SharedLogger.Error($"Cant populate db, OriginalException : {e}");
            }
        }

        private async Task CreateUser(string email, string username, string password, string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
                return;

            user = new UserEntity
            {
                UserName = username,
                Email = email
            };

            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(user, role);
            await dbContext.SaveChangesAsync();
        }
    }
}