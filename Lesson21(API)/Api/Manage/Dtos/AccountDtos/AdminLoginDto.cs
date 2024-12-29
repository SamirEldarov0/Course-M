using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Lesson21_API_.Api.Manage.Dtos.AccountDtos
{
    public class AdminLoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }



    public class AdminLoginDtoValidator : AbstractValidator<AdminLoginDto>  
    {
        public AdminLoginDtoValidator()
        {
            RuleFor(x => x.UserName).NotNull().MaximumLength(30);
            RuleFor(x => x.Password).NotNull().MaximumLength(20);
        }
    }
}
