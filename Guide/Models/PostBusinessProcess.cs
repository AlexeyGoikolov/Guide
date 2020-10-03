namespace Guide.Models
{
    public class PostBusinessProcess
    {
        public int Id { get; set; }
        public int BusinessProcessId { get; set; }
        public virtual BusinessProcess BusinessProcess { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}