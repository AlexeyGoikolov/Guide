using System;

namespace Guide.Models
{
    public class Template
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Title { get; set; }
        public string ContentTemplate { get; set; }
        
    }
}