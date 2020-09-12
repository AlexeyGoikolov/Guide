using System;

namespace Guide.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string CoverPath { get; set; }
        public string Edition { get; set; }
        public string VirtualPath { get; set; }
        public string PhysicalPath { get; set; } = null;
        public bool Active { get; set; } = true;
        public bool IsRecipe { get; set; } = false;
        public int? TypeId { get; set; }
        public virtual Type Type { get; set; }
        public string YearOfWriting { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public DateTime DateUpdate { get; set; } = DateTime.Now;
    }
}