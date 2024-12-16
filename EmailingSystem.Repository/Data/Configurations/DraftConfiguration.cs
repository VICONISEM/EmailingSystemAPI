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
    public class DraftConfiguration : IEntityTypeConfiguration<DraftConversations>
    {
        public void Configure(EntityTypeBuilder<DraftConversations> builder)
        {
            #region Id
            builder.HasKey(D => D.Id);
            builder.Property(D => D.Id).UseIdentityColumn(1, 1);
            #endregion

            #region 1-M Relation With Attach
            builder.HasMany(D => D.DraftAttachments)
                .WithOne(C => C.draftConversations)
                .HasForeignKey(D=>D.DraftConversationId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion


        }
    }
}
