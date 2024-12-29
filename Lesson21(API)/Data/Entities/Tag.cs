using System.Collections.Generic;

namespace Lesson21_API_.Data.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CourseTag> CourseTags { get; set; }
    }
}
