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
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            #region Relation 1-M With User
            builder.HasOne(M => M.Sender)
           .WithMany(U => U.MessagesSender)
           .HasForeignKey(M => M.SenderId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(M => M.Receiver)
                   .WithMany(U => U.MessagesReceiver)
                   .HasForeignKey(M => M.ReceiverId).OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region Index
            builder.HasIndex(M => M.SendAt);
            #endregion
        }
    }
}
