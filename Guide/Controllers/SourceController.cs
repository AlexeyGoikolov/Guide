﻿using System.Collections.Generic;
 using System.IO;
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
    public class SourceController : Controller
    {
  private readonly GuideContext _db;

  public SourceController(GuideContext db)
  {
      _db = db;
  }


  public IActionResult Index()
        {
            List<Post> posts = _db.Posts.Where(p => p.Active).ToList();
            List<Book> books = _db.Books.Where(b => b.Active).ToList();
            List<LibraryListViewModel> models = new List<LibraryListViewModel>();
            foreach (var post in posts)
            {
                models.Add(new LibraryListViewModel()
                {
                    Id = post.Id,
                    Author = post.Author,
                    Name = post.Title,
                    Category = post.Category,
                    Type = post.Type,
                    TypeContent = post.TypeContent,
                    TypeState = post.TypeState,
                    DateCreate = post.DateOfCreate,
                    Active = post.Active
                });
            }
            foreach (var book in books)
            {
                string s = book.VirtualPath;
                if (s != null)
                {
                    string[] parts = s.Split('.');
                    s = parts[parts.Length - 1];
                }

                LibraryListViewModel bookModel = new LibraryListViewModel
                {
                    Id = book.Id,

                    Name = book.Name,
                    Type = new Type() {Name = s},
                    TypeContent = new TypeContent() {Name = "Книга"},
                    TypeState = book.IsRecipe ? new TypeState() {Name = "Рецепт"} : new TypeState() {Name = ""},
                    DateCreate = book.DateCreate,
                    Active = book.Active
                };
                List<Author> bookAuthors =
                    _db.BookAuthors.Where(b => b.BookId == book.Id).Select(a => a.Author).ToList();
                bookModel.BookAuthors = bookAuthors;
                models.Add(bookModel);
            }
            
            return View(models);
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
    }
}
