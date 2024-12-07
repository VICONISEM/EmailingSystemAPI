using EmailingSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Repository.Data.Configurations
{
    public class UserInboxConfiguration : IEntityTypeConfiguration<UserInbox>
    {
        public void Configure(EntityTypeBuilder<UserInbox> builder)
        {
            #region Composite primary key
            builder.HasKey(ucs => new { ucs.UserId, ucs.ConversationId });
            #endregion

            #region Relation With Conversation
            builder.HasOne(ucs => ucs.Conversation)
                   .WithMany(c => c.UserInboxes)
                   .HasForeignKey(ucs => ucs.ConversationId)
                   .OnDelete(DeleteBehavior.Cascade);

            
            builder.HasOne(ucs => ucs.User)
                   .WithMany(c => c.UserInboxes)
                   .HasForeignKey(ucs => ucs.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion

        }
    }
}
