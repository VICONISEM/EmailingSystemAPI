namespace EmailingSystemAPI.DTOs
{
    public class ConversationToReturnDto
    {
        public int Id { get; set; }
        public string Subject { get; set; } = null!;
        public string SenderEmail { get; set; } = null!;
        public string SenderName { get; set; } = null!;
        public string SenderPictureURL { get; set; } = null!;
        public string ReceiverEmail { get; set; } = null!;
        public string ReceiverName { get; set; } = null!;
        public string ReceiverPictureURL { get; set; } = null!;

        public MessageDto DraftMessage { get; set; }

        public IEnumerable<MessageDto> Messages { get; set; } = new List<MessageDto>();

    }
}
