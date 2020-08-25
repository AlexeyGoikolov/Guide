using System;
using System.Collections.Generic;

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

        public Issue()
        {
            IssueSteps = new List<IssueStep>();
        }
    }
}