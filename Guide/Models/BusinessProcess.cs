using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guide.Models
{
    public class BusinessProcess
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } = "Бизнес-процесс";
        public DateTime CreatedAt { get; set; } = DateTimeOffset.Now.UtcDateTime.AddHours(6);
        [NotMapped]
        public virtual List<Issue> Issues{ get; set; }
    }
}