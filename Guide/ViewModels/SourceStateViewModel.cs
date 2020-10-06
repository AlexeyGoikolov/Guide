using System.Collections.Generic;
using Guide.Models;

namespace Guide.ViewModels
{
    public class SourceStateViewModel
    {
        public Source Source { get; set; }
        public List<SourceState> SourceStates { get; set; }
    }
}