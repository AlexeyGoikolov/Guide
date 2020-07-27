using System.Collections.Generic;
using System.Linq;
using Guide.Models;
using Guide.Models.Data;
using Guide.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Guide.Controllers
{
    public class PostsController : Controller
    {
        private readonly GuideContext _db;

        public PostsController(GuideContext db)
        {
            _db = db;
        }



        public IActionResult Index()
        {
            List<Post> posts = _db.Posts
                .Include(c => c.Category)
                .Include(t => t.Type).ToList();
            return View(posts);
        }

        public IActionResult Preview(string id)
        {
            Post post = _db.Posts
                .Include(c => c.Category)
                .Include(t => t.Type).FirstOrDefault(p => p.Id == id);
            return View(post);
        }
        public IActionResult Create()
        {
            return View(new Post());
        }
        public IActionResult CreateCategoryAjax(Category category)
        {
            if (category.Name != null)
            {
                _db.Categories.Add(category);
                _db.SaveChanges();
            }
            PostCategoryViewModel model = new PostCategoryViewModel()
            {
                Post = new Post(),
                Categories = _db.Categories.ToList()
            };
            
            return PartialView("PartialViews/CategoriesPartial", model);
        }
        
        public IActionResult CreateTypeAjax(Type type)
        {
            if (type.Name != null)
            {
                _db.Types.Add(type);
                _db.SaveChanges();
            }
            PostTypeViewModel model = new PostTypeViewModel()
            {
                Post = new Post(),
                Types = _db.Types.ToList()
            };
            
            return PartialView("PartialViews/TypesPartial", model);
        }
        
        public IActionResult DeleteCategoryAjax(string id)
        {
            Category category = _db.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _db.Categories.Remove(category);
                _db.SaveChanges();
            }
            PostCategoryViewModel model = new PostCategoryViewModel()
            {
                Post = new Post(),
                Categories = _db.Categories.ToList()
            };
            
            return PartialView("PartialViews/CategoriesPartial", model);
        }
        public IActionResult DeleteTypeAjax(string id)
        {
            Type type = _db.Types.FirstOrDefault(t => t.Id == id);
            if (type != null)
            {
                _db.Types.Remove(type);
                _db.SaveChanges();
            }
            PostTypeViewModel model = new PostTypeViewModel()
            {
                Post = new Post(),
                Types = _db.Types.ToList()
            };
            
            return PartialView("PartialViews/TypesPartial", model);
        }
    }
}