using System;
using Guide.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Type = System.Type;

namespace Guide.ViewModels
{
    public class PostCreateViewModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string Author { get; set; }
        public string TextContent { get; set; }
        public IFormFile VirtualPath { get; set; }
        public string PhysicalPath { get; set; }
        public string CategoryId { get; set; }
        public string TypeId { get; set; }
    }
}