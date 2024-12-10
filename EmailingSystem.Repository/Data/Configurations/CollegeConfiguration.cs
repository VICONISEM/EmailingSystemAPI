using EmailingSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmailingSystem.Repository.Data.Configurations
{
    public class CollegeConfiguration : IEntityTypeConfiguration<College>
    {
        public void Configure(EntityTypeBuilder<College> builder)
        {
            #region Name
            builder.HasIndex(C => C.Name).IsUnique();
            builder.Property(C => C.Name).HasMaxLength(255);
            #endregion

            #region Relation 1-M With Department
            builder.HasMany(C => C.Departments)
                       .WithOne(D => D.College)
                       .HasForeignKey(D => D.CollegeId); 
            #endregion

        }
    }
}
