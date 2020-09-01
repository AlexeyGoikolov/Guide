using System.Collections.Generic;
using Guide.Models;

namespace Guide.ViewModels
{
    public class AddForUserViewModel
    {
        public string UserId { get; set; }
        public List<Issue> Issues{ get; set; }
        public List<int> IssuesId{ get; set; }
    }
}