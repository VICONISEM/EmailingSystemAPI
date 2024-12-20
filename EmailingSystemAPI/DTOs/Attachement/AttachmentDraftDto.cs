using EmailingSystem.Core.Entities;

namespace EmailingSystemAPI.DTOs.Attachement
{
    public class AttachmentDraftDto
    {
        public string FileName { get; set; } = null!;
        public IFormFile File { get; set; } = null!;

    }
}
