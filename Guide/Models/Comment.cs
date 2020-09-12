using System;

namespace Guide.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public virtual User Author { get; set; }
        public int? PostId { get; set; }
        public virtual Post Post { get; set; }
        public int? BookId { get; set; }
        public virtual Book Book { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreate { get; set; } = DateTime.Now;
    }
}