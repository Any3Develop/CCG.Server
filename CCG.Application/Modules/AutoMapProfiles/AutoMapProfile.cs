using AutoMapper;
using CCG.Domain.Entities.Identity;
using CCG.Shared.Api;

namespace CCG.Application.Modules.AutoMapProfiles
{
    public class AutoMapProfile : Profile
    {
        public AutoMapProfile(IServiceProvider serviceProvider)
        {
            CreateMap<UserEntity, UserData>();
        }
    }
}