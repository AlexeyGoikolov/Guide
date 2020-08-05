using System.Collections.Generic;
using Guide.Models;

namespace Guide.ViewModels
{
    public class PostTemplateViewModel
    {
        public PostCreateViewModel Post { get; set; }
        public List<Template> Templates { get; set; }
    }
}