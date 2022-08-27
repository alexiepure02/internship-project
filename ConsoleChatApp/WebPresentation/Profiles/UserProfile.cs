using AutoMapper;
using Domain;
using WebPresentation.Dto;

namespace WebPresentation.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserPutPostDto, User>();
            CreateMap<User, UserGetDto>();
        }
    }
}
