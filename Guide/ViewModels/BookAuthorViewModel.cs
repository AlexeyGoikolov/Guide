using System.Collections.Generic;
using Guide.Models;

namespace Guide.ViewModels
{
    public class BookAuthorViewModel
    {
        public SourceCreateViewModel Source { get; set; }
        public List<Author> AllAuthors { get; set; }
    }
}