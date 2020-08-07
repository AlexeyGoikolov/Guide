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
        public string PostId { get; set; }
        public  Post Post { get; set; }
    }
}