using System.Collections.Generic;

namespace Lesson21_API_.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public List<Course> Courses { get; set; }
    }
}
