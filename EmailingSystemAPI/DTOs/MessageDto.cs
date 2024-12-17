namespace EmailingSystemAPI.DTOs
{
    public class MessageDto
    {
        public long Id { get; set; }
        public string? Content { get; set; }
        public string SenderEmail { get; set; } = null!;
        public string ReceiverEmail { get; set; } = null!;
        public MessageDto? ParentMessage { get; set; }

        public IEnumerable<AttachementDto> Attachements { get; set; } = new List<AttachementDto>();


    }
}
