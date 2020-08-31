namespace Guide.Models
{
    public class DesiredResultIssue
    {
        public int Id { get; set; }
        public int IssueId { get; set; }
        public virtual Issue Issue { get; set; }
        public int  DesiredResultId { get; set; }
        public virtual  DesiredResult  DesiredResult { get; set; }
    }
}