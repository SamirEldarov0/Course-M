using Lesson21_API_.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lesson21_API_.Data.Configurations
{
    public class CourseTagConfiguration : IEntityTypeConfiguration<CourseTag>
    {
        public void Configure(EntityTypeBuilder<CourseTag> builder)
        {
            //builder.HasOne(x => x.Course).WithMany(x => x.CourseTags).OnDelete(DeleteBehavior.NoAction);  course silinse course tagda silinsin
        }
    }
}
