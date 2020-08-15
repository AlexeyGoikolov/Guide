using System;

namespace Guide.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string TextContent { get; set; }
        //Виртуальный Путь
        public string VirtualPath { get; set; }
        //Физический Путь
        public string PhysicalPath { get; set; }
        public DateTime DateOfCreate { get; set; } = DateTime.Now;
        public DateTime DateOfUpdate { get; set; } = DateTime.Now;
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int TypeId { get; set; }
        public bool Active { get; set; } = true;
        public virtual Type Type { get; set; }
        
    }
}