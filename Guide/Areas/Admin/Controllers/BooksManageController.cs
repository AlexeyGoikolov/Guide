
using System.Collections.Generic;
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
            BookCreateViewModel model = new BookCreateViewModel
            {
                AllAuthors = _db.Authors.Where(c=> c.Active).ToList()
            };
            return View(model);
        }
        
        [HttpPost]
        public IActionResult Create(IFormFile coverPath, IFormFile virtualPath, BookCreateViewModel model, int[] authors)
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
                    CoverPath = Load(model.Name, coverPath),
                    VirtualPath = Load(model.Name, virtualPath),
                    PhysicalPath = model.PhysicalPath,
                    YearOfWriting = model.YearOfWriting
                };
                if (book.CoverPath == null)
                {
                    book.CoverPath = "/BooksFiles/Cover_missing.png";
                }
                _db.Books.Add(book);
                _db.SaveChanges();
                if (authors.Length > 0)
                {
                    foreach (var author in authors)
                    {
                        BookAuthor bookAuthor = new BookAuthor()
                        {
                            BookId = book.Id,
                            AuthorId = author
                        };
                        _db.BookAuthors.Add(bookAuthor);
                    }
                }

                _db.SaveChanges();
                return Json(book.Id);
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
                        book.Active = false;
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
        
        public IActionResult CreateAuthorAjax(string name)
        {
            if (name != null)
            {
                Author author = new Author
                {
                    Name = name
                };
                _db.Authors.Add(author);
                _db.SaveChanges();
            }
            BookCreateViewModel model = new BookCreateViewModel
            {
                AllAuthors = _db.Authors.Where(a=> a.Name == name).ToList(),
            };

            return Json(model);
            
        }
        public IActionResult DeleteAuthorAjax(int id)
        {
            Author author = _db.Authors.FirstOrDefault(a => a.Id == id);
            if (author != null)
            {
                author.Active = false;
                _db.SaveChanges();
            }
            BookCreateViewModel model = new BookCreateViewModel()
            {
                AllAuthors = _db.Authors.Where(c=> c.Active).ToList(),
            };
            return PartialView("PartialViews/AuthorPartial", model);
        }
    }
}