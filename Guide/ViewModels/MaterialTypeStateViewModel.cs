using System.Collections.Generic;
using Guide.Models;

namespace Guide.ViewModels
{
    public class MaterialTypeStateViewModel
    {
        public MaterialCreateViewModel Material { get; set; }
        public List<TypeState> TypeContents { get; set; }
    }
}