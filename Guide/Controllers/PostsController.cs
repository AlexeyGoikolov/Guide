using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guide.Models;
using Guide.Models.Data;
using Guide.Services;
using Guide.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Guide.Controllers
{
    public class PostsController : Controller
    {
        private readonly GuideContext _db;
        private readonly UserManager<User> _userManager;


        public PostsController(GuideContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;

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

            ViewBag.ListComment = _db.Comments
                .Include(c => c.Post)
                .Include(u => u.Author).ToList();
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
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateComment(Comment model)
        {
            if (model.PostId != null && model.Description != null)
            {
                Comment comment = new Comment()
                {
                    PostId = model.PostId,
                    AuthorId = _userManager.GetUserId(User),
                    Description = model.Description
                };
                var result = _db.Comments.AddAsync(comment);
                if (result.IsCompleted)
                {
                    await _db.SaveChangesAsync();
                }
                comment.Author = _db.Users.FirstOrDefault(u => u.Id == comment.AuthorId);
                return Json(comment);
            }
            return NotFound();
           
        }
    }
}