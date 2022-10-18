using AutoMapper;
using Domain;
using WebPresentation.Dto;

namespace WebPresentation.Profiles
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<MessagePutPostDto, Message>();
            CreateMap<Message, MessageGetDto>();
        }
    }
}
