using System.Collections.Generic;
using Guide.Models;

namespace Guide.ViewModels
{
    public class UserDetailsViewModel
    {
        public User User { get; set; }
        public TaskUser Task { get; set; }
        public List<Issue> Issues{ get; set; }
        public List<Issue> PositionsIssues{ get; set; }
    }
}