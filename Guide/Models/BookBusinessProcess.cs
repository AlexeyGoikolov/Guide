namespace Guide.Models
{
    public class BookBusinessProcess
    {
        public int Id { get; set; }
        public int BusinessProcessId { get; set; }
        public virtual BusinessProcess BusinessProcess { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
    }
}