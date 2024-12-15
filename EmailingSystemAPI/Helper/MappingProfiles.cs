using AutoMapper;
using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs;

namespace EmailingSystemAPI.Helper
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        {
            #region MappingApplicationUser
            CreateMap<ApplicationUser, RegisterDto>().ReverseMap();
            CreateMap<ApplicationUser, UserDto>().ForMember(U => U.DepartmentName, O => O.MapFrom(U => U.Department.Name)).ReverseMap();
            #endregion

            #region MappingConversation
            CreateMap<Conversation, ConversationDto>()
                   .ForMember(C => C.SenderName, O => O.MapFrom(C => C.Sender.Name))
                   .ForMember(C => C.ReceiverName, O => O.MapFrom(C => C.Receiver.Name))

                   .ForMember(C => C.SenderEmail, O => O.MapFrom(C => C.Sender.Email))
                   .ForMember(C => C.ReceiverEmail, O => O.MapFrom(C => C.Receiver.Email))

                   .ForMember(C => C.LastMessageTime, O => O.MapFrom(C => C.LastMessage.SendAt))
                   .ReverseMap(); 
            #endregion








        }


    }
}
