using System.Collections.Generic;
using Guide.Models;

namespace Guide.ViewModels
{
    public class MaterialCategoryViewModel
    {
        public MaterialCreateViewModel Material { get; set; }
        public List<Category> Categories { get; set; }

    }
}