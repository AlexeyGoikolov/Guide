using System;

namespace Guide.Models
{
    public class Book
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string CoverPath { get; set; }
        public string VirtualPath { get; set; }
        public string PhysicalPath { get; set; }
        public string Type { get; set; } = "2";
        public DateTime DateOfWriting { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public DateTime DateUpdate { get; set; } = DateTime.Now;
    }
}