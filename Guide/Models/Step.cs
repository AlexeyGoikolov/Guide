using System;
using System.Collections.Generic;

namespace Guide.Models
{
    public class Step
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StepDescription { get; set; }
        public string Author { get; set; }
        public string Type { get; set; } = "Шаг";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual List<IssueStep> IssueSteps { get; set; }

        public Step()
        {
            IssueSteps = new List<IssueStep>();
        }
    }
}