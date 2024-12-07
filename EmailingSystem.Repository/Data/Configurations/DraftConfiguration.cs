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
    public class DraftConfiguration : IEntityTypeConfiguration<Draft>
    {
        public void Configure(EntityTypeBuilder<Draft> builder)
        {
            #region Id
            builder.HasKey(D => D.Id);
            builder.Property(D => D.Id).UseIdentityColumn(1, 1);
            #endregion

            #region Relations 1-M With User
            builder.HasOne(C => C.Sender)
                   .WithMany(U => U.DraftsSender)
                   .HasForeignKey(C => C.SenderId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(C => C.Receiver)
                   .WithMany(U => U.DraftsReceiver)
                   .HasForeignKey(C => C.ReceiverId).OnDelete(DeleteBehavior.Restrict);
            #endregion


        }
    }
}
