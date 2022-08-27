using AutoMapper;
using Domain;
using WebPresentation.Dto;

namespace WebPresentation.Profiles
{
    public class FriendProfile : Profile
    {
        public FriendProfile()
        {
            CreateMap<Friends, FriendGetDto>();
        }
    }
}
