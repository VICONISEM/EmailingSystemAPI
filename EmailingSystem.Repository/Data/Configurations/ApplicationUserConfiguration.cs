using EmailingSystem.Core.Entities;
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
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            #region Id
            builder.HasKey(U => U.Id);
            builder.Property(U => U.Id).UseIdentityColumn(1,1);
            #endregion

            #region Name
            builder.Property(U => U.Name).HasMaxLength(100);
            builder.Property(U => U.NormalizedName).HasMaxLength(100).HasField("NormalizedNameAttribute");
            #endregion

            #region NationalId
            builder.Property(U => U.NationalId).HasMaxLength(14);

            builder.HasIndex(u => u.NationalId)
                   .IsUnique();
            #endregion

            #region Relation 1-1 With Department
            builder.HasOne(U => U.Department)
                    .WithOne(D => D.User)
                    .HasForeignKey<ApplicationUser>(U => U.DepartmentId).OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(U => U.DepartmentId).IsUnique();
            #endregion

            #region Relation 1-1 With Signature
            builder.HasOne(U => U.Signature)
                   .WithOne(D => D.User)
                   .HasForeignKey<ApplicationUser>(U => U.SignatureId).OnDelete(DeleteBehavior.Cascade).IsRequired(false);

            builder.Property(U => U.SignatureId).IsRequired(false);
            #endregion

            #region Index
            builder.HasIndex(U => U.NormalizedName);
            #endregion

        }
    }
}
