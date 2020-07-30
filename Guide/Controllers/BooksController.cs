﻿using System.IO;
using System.Linq;
using Guide.Models;
using Guide.Models.Data;
using Guide.Services;
using Guide.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Guide.Controllers
{
    public class BooksController : Controller
    {
        private readonly GuideContext _db;
        private readonly UserManager<User> _userManager;
        private IHostEnvironment _environment;
        private UploadService _uploadService;

        public BooksController(GuideContext db, UserManager<User> userManager, IHostEnvironment environment, UploadService uploadService)
        {
            _db = db;
            _userManager = userManager;
            _environment = environment;
            _uploadService = uploadService;
        }

        public IActionResult Index()
        {
            return View(_db.Books.ToList());
        }
        
        public IActionResult Create()
        {
            return View(new BookCreateViewModel());
        }
        
        [HttpPost]
        public IActionResult Create(BookCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Book book = new Book()
                {
                    Id = model.Id,
                    Author = model.Author,
                    ISBN = model.ISBN,
                    CoverPath = Load(model.Id, model.CoverPath),
                    VirtualPath = Load(model.Id, model.VirtualPath),
                    PhysicalPath = model.PhysicalPath,
                    DateOfWriting = model.DateOfWriting
                };
                _db.Books.Add(book);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        
        private string Load(string id, IFormFile file)
        {
            if (file != null)
            {
                string path = Path.Combine(_environment.ContentRootPath + $"\\wwwroot\\BooksFiles\\{id}");
                string filePath = $"BooksFiles/{id}/{file.FileName}";
                if (!Directory.Exists($"wwwroot/BooksFiles/{id}"))
                {
                    Directory.CreateDirectory($"wwwroot/BooksFiles/{id}");
                }
                _uploadService.Upload(path, file.FileName, file);
                return filePath;
            }

            return null;
        }
    }
}