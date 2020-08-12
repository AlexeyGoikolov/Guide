using System.Collections.Generic;
using Guide.Models;

namespace Guide.ViewModels
{
    public class MaterialTemplateViewModel
    {
        public MaterialCreateViewModel Material { get; set; }
        public List<Template> Templates { get; set; }
    }
}