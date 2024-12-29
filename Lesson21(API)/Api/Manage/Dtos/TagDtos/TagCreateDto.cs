using FluentValidation;

namespace Lesson21_API_.Api.Manage.Dtos.TagDtos
{
    public class TagCreateDto
    {
        public string Name { get; set; }

    }
    public class CourseCreateDtoValidator:AbstractValidator<TagCreateDto>
    {
        public CourseCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().MaximumLength(30);
        }
    }
}
