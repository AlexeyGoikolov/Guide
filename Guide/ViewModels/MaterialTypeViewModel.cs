using System.Collections.Generic;
using Guide.Models;

namespace Guide.ViewModels
{
    public class MaterialTypeViewModel
    {
        public MaterialCreateViewModel Material { get; set; }
        public List<Type> Types { get; set; }
    }
}