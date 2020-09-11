using System.Collections.Generic;
using Guide.Models;

namespace Guide.ViewModels
{
    public class IssueStepsViewModel
    {
        public Issue Issue { get; set; }
        public List<Step> DesignatedSteps { get; set; }
        public List<Step> AllSteps { get; set; }
        public List<DesiredResult> DesiredResults { get; set; }
        public string Action { get; set; }
        public string Back { get; set; }
    }
}