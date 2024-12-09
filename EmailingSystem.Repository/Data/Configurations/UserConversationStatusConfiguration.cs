using EmailingSystem.Core.Entities;
using EmailingSystem.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Repository.Data.Configurations
{
    public class UserConversationStatusConfiguration : IEntityTypeConfiguration<UserConversationStatus>
    {
        public void Configure(EntityTypeBuilder<UserConversationStatus> builder)
        {
            #region Status Conversion
            builder.Property(S => S.Status).HasConversion(x => x.ToString(), x => (ConversationStatus)Enum.Parse(typeof(ConversationStatus), x));
            #endregion

            #region Composite primary key
            builder.HasKey(ucs => new { ucs.UserId, ucs.ConversationId });
            #endregion

            #region Relation With Conversation
            builder.HasOne(ucs => ucs.Conversation)
                   .WithMany(c => c.UserConversationStatuses)
                   .HasForeignKey(ucs => ucs.ConversationId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ucs => ucs.User)
                   .WithMany(c => c.UserConversationStatuses)
                   .HasForeignKey(ucs => ucs.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Index
            builder.HasIndex(C => C.Status);
            #endregion
        }
    }
}
