using System;

namespace Guide.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string TextContent { get; set; }
        public string VirtualPath { get; set; }
        public int? TypeContentId { get; set; }
        public int? TypeStateId  { get; set; }
        public string UserId { get; set; }

        //Физический Путь
        public string PhysicalPath { get; set; }
        public DateTime DateOfCreate { get; set; } = DateTime.Now;
        public DateTime DateOfUpdate { get; set; } = DateTime.Now;
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int? TypeId { get; set; }
        public bool Active { get; set; } = true;
        public virtual Type Type { get; set; }
        public virtual TypeState TypeState { get; set; }
        public virtual TypeContent TypeContent { get; set; }
        public virtual User User { get; set; }
         public string Keys { get; set; }
    }
}