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
            CreateMap<ApplicationUser, AuthDto>().ForMember(U => U.DepartmentName, O => O.MapFrom(U => U.Department.Name)).ReverseMap();
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(U => U.CollegeName, O => O.MapFrom(O => O.College.Name))
                .ForMember(U => U.DepartmentName, O => O.MapFrom(O => O.Department.Name));

            #endregion

            #region MappingConversation
            CreateMap<Conversation, ConversationDto>()
                   .ForMember(C => C.SenderName, O => O.MapFrom(C => C.Sender.Name))
                   .ForMember(C => C.ReceiverName, O => O.MapFrom(C => C.Receiver.Name))

                   .ForMember(C => C.SenderEmail, O => O.MapFrom(C => C.Sender.Email))
                   .ForMember(C => C.ReceiverEmail, O => O.MapFrom(C => C.Receiver.Email))


                   .ReverseMap();


            CreateMap<Conversation, ConversationToReturnDto>()
                   .ForMember(C => C.SenderName, O => O.MapFrom(C => C.Sender.Name))
                   .ForMember(C => C.ReceiverName, O => O.MapFrom(C => C.Receiver.Name))

                   .ForMember(C => C.SenderEmail, O => O.MapFrom(C => C.Sender.Email))
                   .ForMember(C => C.ReceiverEmail, O => O.MapFrom(C => C.Receiver.Email))

                    //********************** Start Edits ***************************//
                   .ForMember(C => C.SenderPictureURL, O => O.MapFrom(C => C.Sender.PicturePath))
                   .ForMember(C => C.ReceiverPictureURL, O => O.MapFrom(C => C.Receiver.PicturePath))
                    //********************** End Edits ***************************//

                .ReverseMap();
            #endregion




            #region MappingMessages
            CreateMap<Message, MessageDto>()
                .ForMember(C => C.SenderEmail, O => O.MapFrom(C => C.Sender.Email))
                .ForMember(C => C.ReceiverEmail, O => O.MapFrom(C => C.Receiver.Email))
                .ReverseMap();

            #endregion





    }


    }
}