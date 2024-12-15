using EmailingSystem.Core.Entities;

namespace EmailingSystemAPI.DTOs
{
    public class LastMessageDto
    {
        public string Content { get; set; }
        public bool IsDraft { get; set; } = false;
    }
}
