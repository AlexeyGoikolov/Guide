namespace Guide.Models
{
    public class SourceIdAndEnglishSourceId
    {
        public int Id { get; set; }
        public int EnglishSourceId { get; set; }
        public virtual Source EnglishSource { get; set; }
        public int SourceId { get; set; }
        public virtual Source Source { get; set; }
    }
}