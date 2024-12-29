using System;

namespace Lesson21_API_.Api.Manage.Dtos.CourseDtos
{
    public class CourseListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }        //-  Categorinin detailine kecmek ucun    1-ci usul
        public string CatgName { get; set; }//CourseC-de Category include et.Ancaq CatName olsa match ede bilmir   //-  Categorinin adi
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
    }
    //public class CategoryInCourseItemDto//2-ci usul   bu daha yaxsi usuldur cunki memberler coxala biler
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}
