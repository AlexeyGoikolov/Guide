using System.Collections.Generic;
using Guide.Models;
using Microsoft.AspNetCore.Mvc;

namespace Guide.ViewModels
{
    public class SourceCreateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ISBN { get; set; }
        public int? CategoryId { get; set; }
        public int? SourceTypeId { get; set; }
        public int? SourceStateId { get; set; }
        public bool IsRecipe { get; set; } = false;
        public string SourceDescription { get; set; }
        public string PhysicalPath { get; set; }
        [Remote("CheckYear","Validation", "", ErrorMessage = "Некорректное значение")]
        public string YearOfWriting { get; set; }
        public string Edition { get; set; }
        public string AdditionalInformation { get; set; }
        public List<Author> AllAuthors { get; set; }
        public List<BusinessProcess> BusinessProcessesList { get; set; }
        public string BusinessProcesses { get; set; }
        public int SourceId { get; set; }
        public virtual Source LinkSource { get; set; }
        public string Keys { get; set; }
    }
    
    
   
    
}