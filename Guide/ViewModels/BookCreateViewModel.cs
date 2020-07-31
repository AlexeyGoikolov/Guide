﻿using System;
using Microsoft.AspNetCore.Http;

namespace Guide.ViewModels
{
    public class BookCreateViewModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public IFormFile CoverPath { get; set; }
        public IFormFile VirtualPath { get; set; }
        public string PhysicalPath { get; set; }
        public string YearOfWriting { get; set; }
    }
}