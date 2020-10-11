using System.Collections.Generic;
using Guide.Models;

namespace Guide.ViewModels
{
    public class SourceTypeContentViewModel
    {
        public Source Source { get; set; }
        public List<SourceType> SourceTypes { get; set; }
    }
}