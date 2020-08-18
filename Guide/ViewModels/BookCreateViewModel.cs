﻿using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Guide.ViewModels
{
    public class BookCreateViewModel
    {
        public int Id { get; set; } 
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Author { get; set; }
        public string ISBN { get; set; }
        public bool IsRecipe { get; set; } = false;
        public IFormFile CoverPath { get; set; }
        [Required(ErrorMessage = "Файл не прекреплен")]
        public IFormFile VirtualPath { get; set; }
        public string PhysicalPath { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Remote("CheckYear","Validation", "", ErrorMessage = "Некорректное значение")]
        public string YearOfWriting { get; set; }
        
    }
    
   
    
}