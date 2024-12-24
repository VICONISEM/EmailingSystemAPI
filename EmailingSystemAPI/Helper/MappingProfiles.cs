using AutoMapper;
using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs.Attachement;
using EmailingSystemAPI.DTOs.College;
using EmailingSystemAPI.DTOs.Conversation;
using EmailingSystemAPI.DTOs.Department;
using EmailingSystemAPI.DTOs.DraftConversation;
using EmailingSystemAPI.DTOs.Message;
using EmailingSystemAPI.DTOs.User;

namespace EmailingSystemAPI.Helper
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        {
            #region ApplicationUser
            CreateMap<RegisterDto, ApplicationUser>();
                

            CreateMap<ApplicationUser, AuthDto>()
                .ForMember(U => U.UserId, O => O.MapFrom(U => U.Id))
                .ForMember(U => U.DepartmentName, O => O.MapFrom(U => U.Department != null ? U.Department.Name : null))
                .ForMember(U => U.CollegeName, O => O.MapFrom(U => U.College != null ? U.College.Name : null))
                .ForMember(U => U.PictureURL, O => O.MapFrom<ProfileImageResolver>())
                .ForMember(U => U.SignatureURL, O => O.MapFrom<SignatureResolver>()).ReverseMap();

            CreateMap<ApplicationUser, UserDto>()
                .ForMember(U => U.CollegeName, O => O.MapFrom(O => O.College != null ? O.College.Name : null))
                .ForMember(U => U.DepartmentName, O => O.MapFrom(O => O.Department != null ? O.Department.Name : null))
                .ForMember(U => U.PictureURL, O => O.MapFrom<UserProfileImageResolver>())
                .ForMember(U => U.SignatureURL, O => O.MapFrom<UserProfileSignatureResolver>());

            #endregion

            #region Conversation
            CreateMap<Conversation, ConversationDto>()
                   .ForMember(C => C.SenderName, O => O.MapFrom(C => C.Sender.Name))
                   .ForMember(C => C.ReceiverName, O => O.MapFrom(C => C.Receiver.Name))

                   .ForMember(C => C.SenderEmail, O => O.MapFrom(C => C.Sender.Email))
                   .ForMember(C => C.ReceiverEmail, O => O.MapFrom(C => C.Receiver.Email))
                   //.ForMember(C => C.LastMessageTime, O => O.MapFrom(C => C.Messages.Max(M => M.SendAt)))
                   .ForMember(C => C.HasDraftMessage, O => O.MapFrom(C => C.Messages.Any(m=>m.IsDraft)))
                   .ForMember(C => C.LastMessage, O => O.MapFrom(C => C.Messages.Where(M => !M.IsDraft).MaxBy(M => M.SendAt)));



            CreateMap<Conversation, ConversationToReturnDto>()
                   .ForMember(C => C.SenderName, O => O.MapFrom(C => C.Sender.Name))
                   .ForMember(C => C.ReceiverName, O => O.MapFrom(C => C.Receiver.Name))

                   .ForMember(C => C.SenderEmail, O => O.MapFrom(C => C.Sender.Email))
                   .ForMember(C => C.ReceiverEmail, O => O.MapFrom(C => C.Receiver.Email))

                   //********************** Start Edits ***************************//
                   .ForMember(C => C.SenderPictureURL, O => O.MapFrom<ProfileImageResolverForConversation>())
                   .ForMember(C => C.ReceiverPictureURL, O => O.MapFrom<ProfileImageResolverForConversation>());
                   //********************** End Edits ***************************//
            #endregion

            #region Messages
            CreateMap<Message, MessageDto>()
                .ForMember(C => C.SenderEmail, O => O.MapFrom(C => C.Sender.Email))
                .ForMember(C => C.ReceiverEmail, O => O.MapFrom(C => C.Receiver.Email))
                .ForMember(m => m.SentAt, O => O.MapFrom(M => M.SendAt))
                
                .ReverseMap();

            CreateMap<Message, LastMessageDto>()
                .ForMember(m => m.SentAt, O => O.MapFrom(M => M.SendAt))
                .ForMember(M => M.Attachements, O => O.MapFrom(M => M.Attachments));
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
                .ForMember(A => A.Name, M => M.MapFrom(O => O.FileName))
                .ForMember(A => A.FileURL, O => O.MapFrom<MessageAttachmentResolver>());
            #endregion

            #region DraftConversation
            CreateMap<DraftConversations, DraftConversationDtoReturn>()
                .ForMember(D => D.Subject, M => M.MapFrom(D => D.Subject))
                .ForMember(D => D.CreatedAt, M => M.MapFrom(D => D.CreatedAt))
                .ForMember(D => D.SenderEmail, M => M.MapFrom(D => D.Sender.Email))
                .ForMember(D => D.ReceiverEmail, M => M.MapFrom(D => (D.Receiver!= null) ? D.Receiver.Email :null ))
                .ForMember(D => D.Body, M => M.MapFrom(D => D.Body))
                .ForMember(D => D.Subject, M => M.MapFrom(D => D.Subject));


            CreateMap<DraftAttachments, AttachementDto>()
                .ForMember(C => C.FileURL, M => M.MapFrom<FileResolver>());
                
               
            #endregion

        }
    }
}