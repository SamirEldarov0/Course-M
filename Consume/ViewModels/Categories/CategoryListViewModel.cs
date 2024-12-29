using System.Collections.Generic;

namespace Consume.ViewModels.Categories
{
    public class CategoryListViewModel
    {
        public int TotalPage { get; set; }
        public List<CategoryListItemViewModel> Data { get; set; }
    }

    public class CategoryListItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CourseCount { get; set; }
    }
}
