﻿
using System.IO;
using System.Linq;
using Guide.Models;
using Guide.Models.Data;
using Guide.Services;
using Guide.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Guide.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class BooksManageController : Controller
    {
        
        private readonly GuideContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IHostEnvironment _environment;
        private readonly UploadService _uploadService;

        public BooksManageController(GuideContext db, UserManager<User> userManager, IHostEnvironment environment, UploadService uploadService)
        {
            _db = db;
            _userManager = userManager;
            _environment = environment;
            _uploadService = uploadService;
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
                    TypeId = 1,
                    Name = model.Name,
                    IsRecipe = model.IsRecipe,
                    ISBN = model.ISBN,
                    Edition = model.Edition,
                    CoverPath = Load(model.Name, model.CoverPath),
                    VirtualPath = Load(model.Name, model.VirtualPath),
                    PhysicalPath = model.PhysicalPath,
                    YearOfWriting = model.YearOfWriting
                };
                if (model.Author != null)
                {
                    
                    BookAuthor bookAuthor = new BookAuthor()
                    {
                        BookId = book.Id,
                        AuthorId = model.Author
                        
                    };
                    _db.BookAuthors.Add(bookAuthor);
                }
                if (book.CoverPath == null)
                {
                    book.CoverPath = "/BooksFiles/Cover_missing.png";
                }
               
                _db.Books.Add(book);
                _db.SaveChanges();
                return RedirectToAction("Index" , "SourceManage");
            }
            return View(model);
        }

        public IActionResult Details(int id)
        {
            Book book = _db.Books.FirstOrDefault(b => b.Id == id);
            return View(book);
        }

        private string Load(string name, IFormFile file)
        {http:
            if (file != null)
            {
                string path = Path.Combine(_environment.ContentRootPath + $"\\wwwroot\\BooksFiles\\{name}");
                string filePath = $"/BooksFiles/{name}/{file.FileName}";
                if (!Directory.Exists($"wwwroot/BooksFiles/{name}"))
                {
                    Directory.CreateDirectory($"wwwroot/BooksFiles/{name}");
                }
                _uploadService.Upload(path, file.FileName, file);
                return filePath;
            }
            return null;
        }
        
        public IActionResult Delete(int id)
        {
            if (id != 0)
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
        public IActionResult ConfirmDelete(int id)
        {
            if (id != 0)
            {
                Book book = _db.Books.FirstOrDefault(v => v.Id == id);
                if (book != null)
                {
                    if (book.Active)
                        book.Active = false;
                    else
                        book.Active = true;
                    _db.Books.Update(book); 
                    _db.SaveChanges();
                }
            }
            return RedirectToAction("Index" , "SourceManage");
        }

        public  IActionResult ReadBook(string path, int id)
        {
            
            if (path != null)
            {
                string ext=path.Substring(path.LastIndexOf('.'));
                if (ext == ".pdf")
                {
                    Book book = _db.Books.FirstOrDefault(b => b.Id == id);
                    return View(book) ;
                }
                
            }

            return NotFound() ;
        }
        
        public IActionResult CreateAuthorAjax(Author author)
        {
            if (author.Name != null)
            {
                _db.Authors.Add(author);
                _db.SaveChanges();
            }
            BookAuthorViewModel model = new BookAuthorViewModel()
            {
                Book = new BookCreateViewModel(),
                Authors = _db.Authors.Where(c=> c.Active).ToList(),
            };

            return PartialView("PartialViews/AuthorPartial", model);
            
        }
        public IActionResult DeleteAuthorAjax(int id)
        {
            Author author = _db.Authors.FirstOrDefault(a => a.Id == id);
            if (author != null)
            {
                author.Active = false;
                _db.SaveChanges();
            }
            BookAuthorViewModel model = new BookAuthorViewModel()
            {
                Book = new BookCreateViewModel(),
                Authors = _db.Authors.Where(c=> c.Active).ToList(),
            };
            return PartialView("PartialViews/AuthorPartial", model);
        }
    }
}