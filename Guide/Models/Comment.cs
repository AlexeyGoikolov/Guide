using System;

namespace Guide.Models
{
    public class Comment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string AuthorId { get; set; }
        public virtual User Author { get; set; }
        public string PostId { get; set; }
        public virtual Post Post { get; set; }
        public string SelText { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreate { get; set; }
    }
}