namespace Guide.Models
{
    public class DesiredResultStep
    {
        public int Id { get; set; }
        public int StepId { get; set; }
        public virtual Step  Step{ get; set; }
        public int  DesiredResultId { get; set; }
        public virtual  DesiredResult  DesiredResult { get; set; }
        public bool Active { get; set; } = true;
    }
}