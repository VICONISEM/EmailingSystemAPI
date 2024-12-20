namespace EmailingSystemAPI.DTOs
{
    public class MessageTobeSentDto
    {
        public string ? Content { get; set; }
        public int  ReceiverId { get; set; }
        public long ? ParentMessage { get; set; }

        public IEnumerable<AttachmentToSentDto>? Attachements { get; set; } = new List<AttachmentToSentDto>();


    }
}

