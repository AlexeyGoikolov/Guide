using System;

namespace Guide.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public virtual User Author { get; set; }
        public int? SourceId { get; set; }
        public virtual Source Source { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreate { get; set; } = DateTime.Now;
    }
}