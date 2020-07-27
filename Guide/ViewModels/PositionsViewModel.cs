using System.Collections.Generic;
using Guide.Models;

namespace Guide.ViewModels
{
    public class PositionsViewModel
    {
        public RegisterViewModel User { get; set; }
        public List<Position> Positions { get; set; }
    }
}