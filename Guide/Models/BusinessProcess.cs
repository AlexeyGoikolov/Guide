using System;

namespace Guide.Models
{
    public class BusinessProcess
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } = "Бизнес-процесс";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}