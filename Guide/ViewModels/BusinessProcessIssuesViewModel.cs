using System.Collections.Generic;
using Guide.Models;

namespace Guide.ViewModels
{
    public class BusinessProcessIssuesViewModel
    {
        public BusinessProcess BusinessProcess { get; set; }
        public List<Issue> DesignatedIssues { get; set; }
        public List<Issue> AllIssue { get; set; }
    }
}