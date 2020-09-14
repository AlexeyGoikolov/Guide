namespace Guide.Models
{
    public class PositionIssue
    {
        public int Id { get; set; }
        public int IssueId { get; set; }
        public virtual Issue Issue { get; set; }
        public int PositionId { get; set; }
        public virtual Position Position { get; set; }
    }
}