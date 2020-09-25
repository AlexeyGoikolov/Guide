using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Guide.Models;
using Guide.Models.Data;
using Guide.Services;
using Guide.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Guide.Controllers
{
    public class MaterialController : Controller
    {
        
        private readonly GuideContext _db;
        private readonly UserManager<User> _userManager;
        private IHostEnvironment _environment;
        private UploadService _uploadService;

        public MaterialController(GuideContext db, UserManager<User> userManager, IHostEnvironment environment,
            UploadService uploadService)
        {
            _db = db;
            _userManager = userManager;
            _environment = environment;
            _uploadService = uploadService;
        }

        public IActionResult Index()
        {
            List<Post> posts = _db.Posts.ToList();
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            Post post = _db.Posts.FirstOrDefault(p => p.Id == id);


            return View(post);
        }

        private string Load(int id, IFormFile file)
        {
            if (file != null)
            {
                string path = Path.Combine(_environment.ContentRootPath + $"\\wwwroot\\PostsFiles\\{id}");
                string filePath = $"PostsFiles/{id}/{file.FileName}";
                if (!Directory.Exists($"wwwroot/PostsFiles/{id}"))
                {
                    Directory.CreateDirectory($"wwwroot/PostsFiles/{id}");
                }

                _uploadService.Upload(path, file.FileName, file);
                return filePath;
            }

            return null;
        }

        [HttpGet]
        public IActionResult ViewComment(int id)
        {
            List<Comment> comments = _db.Comments.Where(c => c.PostId == id)
                .OrderByDescending(g => g.DateOfCreate).ToList();

            return PartialView("PartialViews/CommentsPartial", comments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(Comment model)
        {
            if (model.PostId != 0 && model.Description != null)
            {
                Comment comment = new Comment()
                {
                    PostId = model.PostId,
                    AuthorId = _userManager.GetUserId(User),
                    Description = model.Description,
                };
                await _db.Comments.AddAsync(comment);
                await _db.SaveChangesAsync();
            }

            List<Comment> comments = await _db.Comments.Include(c => c.Author).Where(c => c.PostId == model.PostId)
                .OrderByDescending(g => g.DateOfCreate).ToListAsync();

            return PartialView("PartialViews/CommentsPartial", comments);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteComment(int id, int postId)
        {
            Comment comment = await _db.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (comment != null)
            {
                _db.Comments.Remove(comment);
                await _db.SaveChangesAsync();
            }

            List<Comment> comments = await _db.Comments.Where(c => c.PostId == postId)
                .OrderByDescending(g => g.DateOfCreate).ToListAsync();

            return PartialView("PartialViews/CommentsPartial", comments);
        }

       
         
      
       

      
    }

        
    
 
}