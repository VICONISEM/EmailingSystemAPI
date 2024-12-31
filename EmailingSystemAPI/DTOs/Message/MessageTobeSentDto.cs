using EmailingSystemAPI.DTOs.Attachement;

namespace EmailingSystemAPI.DTOs.Message
{
    public class MessageTobeSentDto
    {

        public long ? Id {get; set;}
        public string? Content { get; set; }
       
        public long? ParentMessageId { get; set; } = null;

        public bool IsDraft { get; set; } = false;

        public IEnumerable<IFormFile>? Attachements { get; set; } = new List<IFormFile>();


    }
}

