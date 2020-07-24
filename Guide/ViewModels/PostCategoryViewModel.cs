using System.Collections.Generic;
using Guide.Models;

namespace Guide.ViewModels
{
    public class PostCategoryViewModel
    {
        public Post Post { get; set; }
        public List<Category> Categories { get; set; }
    }
}