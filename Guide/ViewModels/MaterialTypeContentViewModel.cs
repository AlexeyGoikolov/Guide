using System.Collections.Generic;
using System.ComponentModel;
using Guide.Models;

namespace Guide.ViewModels
{
    public class MaterialTypeContentViewModel
    {
        public MaterialCreateViewModel Material { get; set; }
        public List<TypeContent> TypeContents { get; set; }
    }
}