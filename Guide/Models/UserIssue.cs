namespace Guide.Models
{
    public class UserIssue
    {
        public int Id { get; set; }
        public int IssueId { get; set; }
        public virtual Issue Issue { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}