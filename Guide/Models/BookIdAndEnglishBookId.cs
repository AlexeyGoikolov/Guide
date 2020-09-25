namespace Guide.Models
{
    public class BookIdAndEnglishBookId
    {
        public int Id { get; set; }
        public int EnglishBookId { get; set; }
        public virtual Book EnglishBook { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
    }
}