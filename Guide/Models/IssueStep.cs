namespace Guide.Models
{
    public class IssueStep
    {
        public int Id { get; set; }
        public int IssueId { get; set; }
        public virtual Issue Issue { get; set; }
        public int StepId { get; set; }
        public virtual Step Step { get; set; }
    }
}