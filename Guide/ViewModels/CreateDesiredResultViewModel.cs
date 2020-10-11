using System.Collections.Generic;
using Guide.Models;


namespace Guide.ViewModels
{
    public class CreateDesiredResultViewModel
    {
        public DesiredResult DesResult { get; set; }= new DesiredResult();
        public List<DesiredResult> Results { get; set; } 
        public int IssueId{ get; set; }
        public int StepId{ get; set; }
        public List<int> DesiredResultId{ get; set; }=new List<int>();
        public string Baсk { get; set; } = "Список ЖР";
    }
}