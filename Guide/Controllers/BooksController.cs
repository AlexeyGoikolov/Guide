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

  public BooksController(GuideContext db)
  {
      _db = db;
  }


  public IActionResult Index(string activ)
        {
            if (activ == null)
            {
                return View(_db.Books.Where(b => b.Active).ToList());
            }
            return View(_db.Books.Where(b => b.Active == false).ToList());
        }
        
       
        public IActionResult Details(int id)
        {
            Book book = _db.Books.FirstOrDefault(b => b.Id == id);
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
