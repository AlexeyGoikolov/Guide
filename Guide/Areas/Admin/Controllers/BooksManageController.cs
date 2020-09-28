
using System;
using System.IO;
using System.Linq;
using Guide.Models;
using Guide.Models.Data;
using Guide.Services;
using Guide.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

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

      public IActionResult Create(int bookId)
        {
            BookCreateViewModel model = new BookCreateViewModel
            {
                AllAuthors = _db.Authors.Where(c=> c.Active).ToList(),
                BusinessProcessesList = _db.BusinessProcesses.ToList()
                
            };
            if (bookId != 0)
            {
                model.BookId = bookId;
            }
            return View(model);
        }
        
        [HttpPost]
        public IActionResult Create(BookCreateViewModel model, string authors, IFormFile coverFile, IFormFile bookFile)
        {
            if (model.Name != null && bookFile != null)
            {
                Book book = new Book()
                {
                    TypeId = 1,
                    Name = model.Name,
                    IsRecipe = model.IsRecipe,
                    ISBN = model.ISBN,
                    Edition = model.Edition,
                    CoverPath = Load(model.Name, coverFile),
                    VirtualPath = Load(model.Name, bookFile),
                    PhysicalPath = model.PhysicalPath,
                    YearOfWriting = model.YearOfWriting,
                    Keys = model.Keys
                };
                if (book.CoverPath == null)
                {
                    book.CoverPath = "/BooksFiles/Cover_missing.png";
                }
                _db.Books.Add(book);
                _db.SaveChanges();
                if (authors != null)
                {
                    SaveBookAuthors(authors,book);
                }
                if (model.BusinessProcesses != null)
                {
                    SaveBusinessProcessesBook(model, book);
                }
                if (model.BookId != 0)
                {
                    SaveBookIdAndEnglishBookId(model,book);
                }
                return Json(true);
            }
            return Json(false);
        }

        public void SaveBookAuthors(string authors, Book book )
        {
            string[] authorsId = authors.Split(',');
            foreach (var authorName in authorsId)
            {
                var author = _db.Authors.FirstOrDefault(a => a.Name == authorName);
                if (author != null)
                {
                    BookAuthor bookAuthor = new BookAuthor()
                    {
                        BookId = book.Id,
                        AuthorId = author.Id
                    };
                    _db.BookAuthors.Add(bookAuthor);
                    _db.SaveChanges();    
                }
            }
        }

        public void SaveBookIdAndEnglishBookId(BookCreateViewModel model, Book book)
        {
            BookIdAndEnglishBookId bookIdAndEnglishBookId = new BookIdAndEnglishBookId()
            {
                BookId = model.BookId,
                EnglishBookId = book.Id
            };
            _db.BookIdAndEnglishBookIds.Add(bookIdAndEnglishBookId);
            _db.SaveChanges();

        }

        public void SaveBusinessProcessesBook(BookCreateViewModel model, Book book)
        {
            string[] businessProcesses = model.BusinessProcesses.Split(',');
            foreach (var businessProcess in businessProcesses)
            {
                if (businessProcess != "")
                {
                    var process = _db.BusinessProcesses.FirstOrDefault(b => b.Name == businessProcess);
                    if (process != null)
                    {
                        BookBusinessProcess bookBusinessProcess = new BookBusinessProcess()
                        {
                            BookId = book.Id,
                            BusinessProcessId = process.Id
                        };
                        _db.BookBusinessProcesses.Add(bookBusinessProcess);
                        _db.SaveChanges(); 
                    }
                }
            }
        }

        public IActionResult Details(int id)
        {
            int translationID = 0;
            ViewBag.BookTransferLanguage = 0;
            Book book = _db.Books.FirstOrDefault(b => b.Id == id);
            BookIdAndEnglishBookId bookIdAndEnglishBookId = new BookIdAndEnglishBookId();
            bookIdAndEnglishBookId = _db.BookIdAndEnglishBookIds.FirstOrDefault(b => b.BookId == id);

            if (bookIdAndEnglishBookId != null)
            {
                translationID = bookIdAndEnglishBookId.EnglishBookId;
                ViewBag.BookTransferLanguage = "en";
            }
            
            if (bookIdAndEnglishBookId == null)
                bookIdAndEnglishBookId = _db.BookIdAndEnglishBookIds.FirstOrDefault(b => b.EnglishBookId == id);
            
            if(translationID == 0 && bookIdAndEnglishBookId != null)
            {
                translationID = bookIdAndEnglishBookId.BookId;
                ViewBag.BookTransferLanguage = "ru";
            }
            
            ViewBag.BookTransferId = translationID;
            
            return View(book);
        }

        private string Load(string name, IFormFile file)
        {
            if (file != null)
            {
                string path = Path.Combine(_environment.ContentRootPath + $"/wwwroot/BooksFiles/{name}");
                string filePath = $"BooksFiles/{name}/{file.FileName}";
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

        public  IActionResult ReadBook(int id)
        {
            Book book = _db.Books.FirstOrDefault(b => b.Id == id);
            ViewBag.Path = Request.Scheme + "://" + Request.Host.Value + "/" + book.VirtualPath;
            return View(book) ;
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