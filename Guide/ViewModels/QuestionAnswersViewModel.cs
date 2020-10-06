using Guide.Models;

namespace Guide.ViewModels
{
    public enum QuestionAnswersStatus
    {
        Edit,
        Create,
        ToAnswer,
        ToQuestion
    }
    public class QuestionAnswersViewModel
    {
        public QuestionAnswer QuestionAnswer { get; set; } = new QuestionAnswer();
        public QuestionAnswersStatus Status { get; set; } = QuestionAnswersStatus.Create;
        public int? SourceId { get; set; }
        public virtual Source Source { get; set; }
    }
}