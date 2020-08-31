namespace Guide.Models
{
    public class BusinessProcessIssue
    {
        public int Id { get; set; }
        public int BusinessProcessId { get; set; }
        public virtual BusinessProcess BusinessProcess { get; set; }
        public int IssueId { get; set; }
        public virtual Issue Issue { get; set; }
    }
}