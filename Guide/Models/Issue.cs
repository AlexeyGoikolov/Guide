using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guide.Models
{
    public class Issue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IssueDescription { get; set; }
        public string Type { get; set; } = "Задача";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual List<IssueStep> IssueSteps { get; set; }
        public virtual List<DesiredResultIssue>  DesiredResultIssues { get; set; }
        [NotMapped]
        public List<BusinessProcess> BP{ get; set; }
        
    }
}