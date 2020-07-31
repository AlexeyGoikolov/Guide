using Guide.Models;

namespace Guide.ViewModels
{
    public enum QuestionAnswersStatus
    {
        Edit,
        Creat,
        ToAnswer,
        ToQuestion
    }
    public class QuestionAnswersViewModel
    {
        public QuestionAnswer QuestionAnswer { get; set; } = new QuestionAnswer();
        public QuestionAnswersStatus Status { get; set; } = QuestionAnswersStatus.Creat;
    }
}