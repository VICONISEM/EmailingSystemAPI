using AutoMapper;
using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs;

namespace EmailingSystemAPI.Helper
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        {
            #region ApplicationUser
            CreateMap<ApplicationUser, RegisterDto>().ReverseMap();
            CreateMap<ApplicationUser, AuthDto>().ForMember(U => U.DepartmentName, O => O.MapFrom(U => U.Department != null ? U.Department.Name : null)).ReverseMap();
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(U => U.CollegeName, O => O.MapFrom(O => O.College != null ? O.College.Name : null))
                .ForMember(U => U.DepartmentName, O => O.MapFrom(O => O.Department != null ? O.Department.Name : null));
            #endregion

            #region Conversation
            CreateMap<Conversation, ConversationDto>()
                   .ForMember(C => C.SenderName, O => O.MapFrom(C => C.Sender.Name))
                   .ForMember(C => C.ReceiverName, O => O.MapFrom(C => C.Receiver.Name))

                   .ForMember(C => C.SenderEmail, O => O.MapFrom(C => C.Sender.Email))
                   .ForMember(C => C.ReceiverEmail, O => O.MapFrom(C => C.Receiver.Email))
                   .ForMember(C => C.LastMessageTime, O => O.MapFrom(C => C.Messages.Max(M => M.SendAt)))
                   .ForMember(C => C.LastMessage, O => O.MapFrom(C => C.Messages.MaxBy(M => M.SendAt)));



            CreateMap<Conversation, ConversationToReturnDto>()
                   .ForMember(C => C.SenderName, O => O.MapFrom(C => C.Sender.Name))
                   .ForMember(C => C.ReceiverName, O => O.MapFrom(C => C.Receiver.Name))

                   .ForMember(C => C.SenderEmail, O => O.MapFrom(C => C.Sender.Email))
                   .ForMember(C => C.ReceiverEmail, O => O.MapFrom(C => C.Receiver.Email))

                   //********************** Start Edits ***************************//
                   .ForMember(C => C.SenderPictureURL, O => O.MapFrom(C => C.Sender.PicturePath))
                   .ForMember(C => C.ReceiverPictureURL, O => O.MapFrom(C => C.Receiver.PicturePath));
            //********************** End Edits ***************************//


            #endregion

            #region Messages
            CreateMap<Message, MessageDto>()
                .ForMember(C => C.SenderEmail, O => O.MapFrom(C => C.Sender.Email))
                .ForMember(C => C.ReceiverEmail, O => O.MapFrom(C => C.Receiver.Email))
                .ReverseMap();

            CreateMap<Message, LastMessageDto>();

            #endregion

            #region Department
            CreateMap<Department, DepartmentDto>();

            CreateMap<Department, DepartmentWithUserDto>()
                .ForMember(D => D.CollegeName, O => O.MapFrom(O => O.College.Name))
                .ForMember(D => D.userId, O => O.MapFrom(O => O.User.Id));
            #endregion

            #region College
            CreateMap<College, CollegesDto>()
                .ForMember(C => C.Name, M => M.MapFrom(C => C.Name))
                .ForMember(C => C.Abbreviation, M => M.MapFrom(C => C.Abbreviation))
                .ForMember(C => C.Id, M => M.MapFrom(C => C.Id))
                .ForMember(C => C.Departments, M => M.MapFrom(C => C.Departments)).ReverseMap();

            CreateMap<College, CollegeAddDto>()
                .ForMember(C => C.Name, M => M.MapFrom(C => C.Name))
                .ForMember(C => C.Abbreviation, M => M.MapFrom(C => C.Abbreviation)).ReverseMap();

            #endregion

            #region Attachment
            CreateMap<Attachment, AttachementDto>()
                .ForMember(A => A.Name, M => M.MapFrom(O => O.FileName));
            #endregion


        }
    }
}