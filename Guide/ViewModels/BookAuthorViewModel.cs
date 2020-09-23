using System.Collections.Generic;
using Guide.Models;

namespace Guide.ViewModels
{
    public class BookAuthorViewModel
    {
        public BookCreateViewModel Book { get; set; }
        public List<Author> Authors { get; set; }
    }
}