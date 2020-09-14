using System.Collections.Generic;
using Guide.Models;

namespace Guide.ViewModels
{
    public class AddForPositionViewModel
    {
        public int PositionId { get; set; }
        public List<Issue> Issues { get; set; }
        public List<int> IssuesId { get; set; }
        public List<Position> Positions { get; set; }
    }
}