using System;
using System.Collections.Generic;

namespace Lesson21_API_.Api.Manage.Dtos.CourseDtos
{
    public class CourseDetailedDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<TagInCourseDetailedDto> Tags { get; set; }
    }

    public class TagInCourseDetailedDto
    {
        public int TagId { get; set; }
        public string Name { get; set; }
    }
}
