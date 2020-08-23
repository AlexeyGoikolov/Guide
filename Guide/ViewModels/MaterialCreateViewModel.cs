using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Guide.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Type = System.Type;

namespace Guide.ViewModels
{
    public class MaterialCreateViewModel
    {
        public int Id { get; set; } 
        [Required(ErrorMessage = "Необходимо заполнить поле Заголовок")]
        [DataType(DataType.Text)]
        
        public string Title { get; set; }
        [Required(ErrorMessage = "Необходимо заполнить информацию об Авторе")]
        [DataType(DataType.Text)]
        public string Author { get; set; }
        public string TextContent { get; set; }
        public IFormFile VirtualPath { get; set; }
        public string _virtualPath { get; set; }
        public string PhysicalPath { get; set; }
        public int? TypeContentId { get; set; }
        public int? TypeStateId { get; set; }
        public int? CategoryId { get; set; }
        [Required(ErrorMessage = "Необходимо заполнить поле Тип")]
        public int? TypeId { get; set; }
        
        
    }
}