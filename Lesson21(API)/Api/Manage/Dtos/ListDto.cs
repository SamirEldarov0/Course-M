using System.Collections.Generic;

namespace Lesson21_API_.Api.Manage.Dtos
{
    public class ListDto<T>
    {
        public List<T> Data { get; set; }
        public int TotalPage { get; set; }
    }
}
