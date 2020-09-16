using System.Collections.Generic;
using Guide.Models;

namespace Guide.ViewModels
{
    public class PositionIssueViewModel
    {
        public List<Issue> Issues { get; set; }
        public int PositionId{ get; set; }
        public virtual Position Position{ get; set; }
    }
}