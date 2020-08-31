using System.Collections.Generic;
using Guide.Models;


namespace Guide.ViewModels
{
    public class CreateDesiredResultViewModel
    {
        // новый ЖР , если решат добавлять
        public DesiredResult DesResult { get; set; }= new DesiredResult();
        public List<DesiredResult> Results { get; set; } = null;
        public int IssueId{ get; set; }
        //список ЖР, которые выбрали
        public List<int> DesiredResultId{ get; set; }
    }
}