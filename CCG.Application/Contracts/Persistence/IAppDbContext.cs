using CCG.Domain.Entities.Game;
using CCG.Domain.Entities.Identity;
using CCG.Domain.Entities.Lobby;
using Microsoft.EntityFrameworkCore;

namespace CCG.Application.Contracts.Persistence
{
    public interface IAppDbContext
    {
        Task SaveChangesAsync();
        Task MigrateAsync();
        Task<IEnumerable<string>> GetPendingMigrationsAsync();
        DbSet<UserEntity> Users { get; }
        DbSet<DeckEntity> Decks { get; }
        DbSet<DuelEntity> Duels { get; }
        DbSet<LobbyPlayerEntity> Players { get; }
        DbSet<SessionEntity> Sessions { get; }
    }
}