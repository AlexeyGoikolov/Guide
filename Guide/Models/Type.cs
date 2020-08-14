using System;

namespace Guide.Models
{
    public class Type
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public bool Active { get; set; } = true;
    }
}