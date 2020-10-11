using System.Collections.Generic;
using Guide.Models;

namespace Guide.ViewModels
{
    public class SourceCategoryViewModel
    {
        public Source Source { get; set; }
        public List<Category> Categories { get; set; }

    }
}