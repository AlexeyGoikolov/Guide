using System.Collections.Generic;
using Guide.Models;

namespace Guide.ViewModels
{
    public class PostTypeViewModel
    {
        public PostCreateViewModel Post { get; set; }
        public List<Type> Types { get; set; }
    }
}