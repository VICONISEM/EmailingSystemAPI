using EmailingSystemAPI.DTOs.Attachement;

namespace EmailingSystemAPI.DTOs.Message
{
    public class MessageTobeSentDto
    {
        public string? Content { get; set; }
        public int ReceiverId { get; set; }
        public long? ParentMessageId { get; set; }

        public IEnumerable<IFormFile>? Attachements { get; set; } = new List<IFormFile>();


    }
}

