using AutoMapper;
using Domain;
using WebPresentation.Dto;

namespace WebPresentation.Profiles
{
    public class FriendRequestProfile : Profile
    {
        public FriendRequestProfile()
        {
            CreateMap<FriendRequestPutPostDto, FriendRequests>();
            CreateMap<FriendRequests, FriendRequestGetDto>();
        }
    }
}
