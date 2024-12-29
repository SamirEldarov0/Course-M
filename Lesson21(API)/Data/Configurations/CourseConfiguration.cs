using Lesson21_API_.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lesson21_API_.Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(100);//HasDefaultValue ardiyca IsRequiredde yaza bilirik
            builder.Property(x => x.Name).IsRequired(true);
            builder.Property(x=>x.Desc).HasMaxLength(1500);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.HasOne(x => x.Category).WithMany(x => x.Courses).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
