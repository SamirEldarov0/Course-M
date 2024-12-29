using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Lesson21_API_.Api.Manage.Dtos.CategoryDtos
{
    public class CategoryCreateDto
    {
        //[StringLength(maximumLength:50)]
        //[Required]
        public string Name { get; set; }
    }

    public class CategoryCreateDtoValidator:AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Name is required blet").MaximumLength(50).WithMessage("The max length is 50");
        }
    }
}
