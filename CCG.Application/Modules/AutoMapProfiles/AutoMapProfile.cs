using AutoMapper;
using CCG.Domain.Entities.Identity;
using CCG.Domain.Entities.Lobby;
using CCG.Shared.Api;
using CCG.Shared.Game.Context.Session;

namespace CCG.Application.Modules.AutoMapProfiles
{
    public class AutoMapProfile : Profile
    {
        public AutoMapProfile(IServiceProvider serviceProvider)
        {
            CreateMap<UserEntity, UserData>();
            CreateMap<UserEntity, LobbyPlayerEntity>()
                .ForMember(d => d.UserId, o => o.MapFrom(x => x.Id))
                .ForMember(d => d.User, o => o.MapFrom(x => x))
                .ForMember(d => d.Created, o => o.Ignore())
                .ForMember(d => d.Updated, o => o.Ignore());

            CreateMap<LobbyPlayerEntity, SessionPlayerModel>()
                .ForMember(d => d.DeckId, o => o.MapFrom(x => x.DeckId))
                .ForMember(d => d.HeroId, o => o.MapFrom(x => x.Deck.UserId))
                .ForMember(d => d.DeckCards, o => o.MapFrom(x => x.Deck.CardIds));
        }
    }
}