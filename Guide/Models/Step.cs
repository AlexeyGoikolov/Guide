using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guide.Models
{
    public class Step
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StepDescription { get; set; }
        public string Type { get; set; } = "Шаг";
        public DateTime CreatedAt { get; set; } = DateTimeOffset.Now.UtcDateTime.AddHours(6);
        public virtual List<IssueStep> IssueSteps { get; set; }
        [NotMapped]
        public List<DesiredResult> DesiredResults { get; set; }

        [NotMapped] 
        public string Baсk { get; set; } 
    
        public Step()
        {
            IssueSteps = new List<IssueStep>();
        }
    }
}