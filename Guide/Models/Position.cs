using System;

namespace Guide.Models
{
    public class Position
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
    }
}