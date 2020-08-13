﻿using System;
 using System.IO;
using System.Linq;
 using System.Net.Mime;
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

        public IActionResult Index(string activ)
        {
            if (activ == null)
            {
                return View(_db.Books.Where(b => b.Active).ToList());
            }
            return View(_db.Books.Where(b => b.Active == false).ToList());
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
                    Name = model.Name,
                    Author = model.Author,
                    ISBN = model.ISBN,
                    CoverPath = Load(model.Id, model.CoverPath),
                    VirtualPath = Load(model.Id, model.VirtualPath),
                    PhysicalPath = model.PhysicalPath,
                    YearOfWriting = model.YearOfWriting
                };
                _db.Books.Add(book);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Details(string id)
        {
            Book book = _db.Books.FirstOrDefault(b => b.Id == id);
            return View(book);
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
        
        public IActionResult Delete(string id)
        {
            if (id != null)
            {
                Book book = _db.Books.FirstOrDefault(v => v.Id == id);
                if (book != null)
                {
                    return View(book);
                }
            }
            return NotFound();
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult ConfirmDeleta(string id)
        {
            if (id != null)
            {
                Book book = _db.Books.FirstOrDefault(v => v.Id == id);
                if (book != null)
                {
                        book.Active = false;
                    _db.Books.Update(book);
                    _db.SaveChanges();
                }
            }
            return RedirectToAction("Index" , "Books");
        }
    }
}