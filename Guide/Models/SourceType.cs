using System;

namespace Guide.Models
{
    public class SourceType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; } = true;
    }
}