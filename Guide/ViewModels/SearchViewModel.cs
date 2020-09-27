using System.Collections.Generic;
using Guide.Models;

namespace Guide.ViewModels
{
    public class SearchViewModel
    {
        public string Search { get; set; }
        public int TotalFound{ get; set; }
        public List<Book>  Books{ get; set; }
        public List<Post>  Posts{ get; set; }
        public List<Issue>  Issues{ get; set; }
        public List<Step> Steps{ get; set; }
        public List<Glossary> Glossaries{ get; set; }
        public List<QuestionAnswer> QuestionAnswers{ get; set; }
    }
}