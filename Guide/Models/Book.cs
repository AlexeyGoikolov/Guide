using System;

namespace Guide.Models
{
    public class Book
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string CoverPath { get; set; }
        public string VirtualPath { get; set; }
        public string PhysicalPath { get; set; } = null;
        public bool Active { get; set; } = true;
        public string TypeId { get; set; }
        public virtual Type Type { get; set; }
        public string YearOfWriting { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public DateTime DateUpdate { get; set; } = DateTime.Now;
    }
}