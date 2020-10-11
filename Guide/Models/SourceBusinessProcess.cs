namespace Guide.Models
{
    public class SourceBusinessProcess
    {
        public int Id { get; set; }
        public int BusinessProcessId { get; set; }
        public virtual BusinessProcess BusinessProcess { get; set; }
        public int SourceId { get; set; }
        public virtual Source Source { get; set; }
    }
}