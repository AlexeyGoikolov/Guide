using System.Collections.Generic;
using Guide.Models;
using Microsoft.AspNetCore.Mvc;

namespace Guide.ViewModels
{
    public class BookCreateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ISBN { get; set; }
        public bool IsRecipe { get; set; } = false;
        public string PhysicalPath { get; set; }
        [Remote("CheckYear","Validation", "", ErrorMessage = "Некорректное значение")]
        public string YearOfWriting { get; set; }
        public string Edition { get; set; }
        public string AdditionalInformation { get; set; }
        public List<Author> AllAuthors { get; set; }
        public List<BusinessProcess> BusinessProcessesList { get; set; }
        public string BusinessProcesses { get; set; }
        public int BookId { get; set; }
        public string Keys { get; set; }
    }
    
    
   
    
}