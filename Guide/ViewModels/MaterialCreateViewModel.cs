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
        public string Id { get; set; } = Guid.NewGuid().ToString();
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
         [Required(ErrorMessage = "Необходимо заполнить поле Категория")]
                [DataType(DataType.Text)]
        public string CategoryId { get; set; }
        [Required(ErrorMessage = "Необходимо заполнить поле Тип")]
        [DataType(DataType.Text)]
        public string TypeId { get; set; }
        
        
    }
}