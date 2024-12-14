using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EmailingSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmailingSystem.Repository.Data.Configurations
{
    public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> builder)
        {
            #region Relation 1-M With Messages
            builder.HasOne(A => A.Message)
                       .WithMany(M => M.Attachments)
                       .HasForeignKey(A => A.MessageId).OnDelete(DeleteBehavior.Cascade); 
            #endregion
        }
    }
}
