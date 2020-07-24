using System.Linq;
using Guide.Models;
using Guide.Models.Data;
using Guide.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Guide.Controllers
{
    public class PostsController : Controller
    {
        private readonly GuideContext _db;

        public PostsController(GuideContext db)
        {
            _db = db;
        }
        // GET
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
        
    }
}