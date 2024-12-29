using FluentValidation;

namespace Lesson21_API_.Api.Manage.Dtos.TagDtos
{
    public class TagUpdateDto
    {
        public string Name { get; set; }
        
    }

    public class TagUpdateDtoValidator : AbstractValidator<TagUpdateDto>
    {
        public TagUpdateDtoValidator()
        {
            RuleFor(x => x.Name).NotNull();
        }
    }
}
