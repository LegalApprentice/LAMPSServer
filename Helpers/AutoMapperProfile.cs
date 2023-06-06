using AutoMapper;
using LAMPSServer.Models;

namespace LAMPSServer.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserEntity, UserModel>();
            CreateMap<UserData, UserEntity>();

            CreateMap<TeamEntity, TeamModel>();
            CreateMap<TeamData, TeamEntity>();
        }
    }
}