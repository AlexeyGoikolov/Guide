using System;
using Microsoft.AspNetCore.Http;

namespace Guide.Models
{
    public class Post
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        //Виртуальный Путь
        public string VirtualPath { get; set; }
        //Физический Путь
        public string PhysicalPath { get; set; }
        public DateTime DateOfCreate { get; set; } = DateTime.Now;
        public DateTime DateOfUpdate { get; set; } = DateTime.Now;
        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public string TypeId { get; set; }
        public virtual Type Type { get; set; }
        
        
        
    }
}