using AutoMapper;
using Lesson21_API_.Api.Manage.Dtos.CategoryDtos;
using Lesson21_API_.Api.Manage.Dtos.CourseDtos;
using Lesson21_API_.Api.Manage.Dtos.TagDtos;
using Lesson21_API_.Data.Entities;
using System.Linq;

namespace Lesson21_API_.Api.Manage
{
    public class ManageMapper:Profile//Profile classi auto mapperden miras alir
    {
        public ManageMapper()
        {
            CreateMap<CategoryCreateDto, Category>();//Burada automapper ozu avtomatik olaraq CategoryCreateDto obyektinin icinde olub category modelinin icin
            //icinde olan propertilerin value-lerini bir birine beraberlesdirir
            //Bunun ucun startupda bu konfiqurasiyasinida vermeiyem ki auto mapperi isledirem
            //Bu konf vermeyin bir nece usulu var 
            CreateMap<Category,CategoryDetailedDto>();//Categoriden -> detaile
            CreateMap<Category, CategoryListItemDto>().ForMember(destinationMember=>destinationMember.CoursesCount,from=>from.MapFrom(source=>source.Courses.Count));
            CreateMap<Category, CategoryItemDto>();

            CreateMap<CourseTag, TagInCourseDetailedDto>().ForMember(destinationMember => destinationMember.TagId, from => from.MapFrom(x => x.TagId))
                .ForMember(destinationMember => destinationMember.Name, from => from.MapFrom(x => x.Tag.Name));
            CreateMap<CourseCreateDto, Course>(); //for member deyerek elave configurasiyalar vere bilirem meselen useri userdto-a cevirirem 
            //UserDto-da fullname yazmisam userde name and surname yazmisam userdto yaradib propertileri set edende 
            //fullname = name + surname yaziram
            CreateMap<Course, CourseListItemDto>().ForMember(dest=>dest.CatgName,from=>from.MapFrom(x=>x.Category.Name));
            //Map olunacaq obyektin icerisindeki CatgName propertisinin valusu  Source obyektinin icerisinde neye beraber olsun
            CreateMap<Course, CourseDetailedDto>();
                //.ForMember(dest => dest.Tags, 
                //from => from.MapFrom(x => x.CourseTags.Select(x => new TagInCourseDetailedDto { TagId = x.TagId, Name = x.Tag.Name })));

            CreateMap<TagCreateDto, Tag>().ReverseMap();//Normal mentiqde yenisini yazmali olacam Tag-dan => TagCreateDto
            CreateMap<Tag, TagDetailedDto>();
            CreateMap<Tag, TagListItemDto>();
            CreateMap<Tag, TagItemDto>();
        }
    }
}
