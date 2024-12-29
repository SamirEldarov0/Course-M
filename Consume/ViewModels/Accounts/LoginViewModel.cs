using System.ComponentModel.DataAnnotations;

namespace Consume.ViewModels.Accounts
{
    public class LoginViewModel
    {
        [StringLength(maximumLength:25)]
        public string UserName { get; set; }
        [StringLength(maximumLength: 25)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
