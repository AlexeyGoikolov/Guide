namespace Guide.Models
{
    public class SourceAuthor
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }
        public int SourceId { get; set; }
        public virtual Source Source { get; set; }
    }
}