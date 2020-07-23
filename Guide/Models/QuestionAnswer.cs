using System.Collections.Generic;

namespace Guide.Models
{
    public class QuestionAnswer
    {
        public string Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public List<string> Links { get; set; }
        public List<Post> Posts { get; set; }
    }
}