using System;
using System.Diagnostics;
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
                    TypeId = 1,
                    Name = model.Name,
                    Author = model.Author,
                    ISBN = model.ISBN,
                    CoverPath = Load(model.Name, model.CoverPath),
                    VirtualPath = Load(model.Name, model.VirtualPath),
                    PhysicalPath = model.PhysicalPath,
                    YearOfWriting = model.YearOfWriting
                };
                _db.Books.Add(book);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Details(int id)
        {
            Book book = _db.Books.FirstOrDefault(b => b.Id == id);
            return View(book);
        }

        private string Load(string name, IFormFile file)
        {
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
            return RedirectToAction("Index" , "BooksManage");
        }

        public  IActionResult ReadBook(string path, int id)
        {

            if (path != null)
            {
                string commandText = @_environment.ContentRootPath + $"\\wwwroot" + path;
                var proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = commandText;
                proc.StartInfo.UseShellExecute = true;
                proc.Start();
            }

            return RedirectToAction("Details" , "BooksManage", new {id=id});
        }
    }
}