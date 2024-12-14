using EmailingSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmailingSystem.Repository.Data.Configurations
{
    public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            #region Id
            builder.HasKey(C => C.Id);
            builder.Property(C => C.Id).UseIdentityColumn(1, 1);
            #endregion

            #region Subject
            builder.Property(C => C.Subject).IsRequired();
            #endregion

            #region Relations 1-M With User
            builder.HasOne(C => C.Sender)
                   .WithMany(U => U.ConversationsSender)
                   .HasForeignKey(C => C.SenderId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(C => C.Receiver)
                   .WithMany(U => U.ConversationsReceiver)
                   .HasForeignKey(C => C.ReceiverId).OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region Relation 1-1 With LastMessage
            builder.HasOne(C => C.LastMessage)
                   .WithOne()
                   .HasForeignKey<Conversation>(C => C.LastMessageId);
            #endregion

            #region Relation 1-M With Messages
            builder.HasMany(C => C.Messages)
           .WithOne(C => C.Conversation)
           .HasForeignKey(M => M.ConversationId).OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Index
            builder.HasIndex(C => C.SendAt);
            #endregion
        }
    }
}
