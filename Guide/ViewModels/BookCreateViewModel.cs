using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Guide.Models;
using Microsoft.AspNetCore.Http;
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
        public int CategoryId { get; set; }
        public string Edition { get; set; }
        public List<Author> AllAuthors { get; set; }
    }
    
   
    
}