using Microsoft.AspNetCore.Identity;

namespace Lesson21_API_.Data.Entities
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
