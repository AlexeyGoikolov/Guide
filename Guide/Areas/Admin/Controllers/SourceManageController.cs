using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Guide.Models;
using Guide.Models.Data;
using Guide.Services;
using Guide.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
namespace Guide.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class SourceManageController : Controller
    {
        private readonly GuideContext _db;
        private readonly UserManager<User> _userManager;
        private IHostEnvironment _environment;
        private UploadService _uploadService;

        public SourceManageController(GuideContext db, UserManager<User> userManager, IHostEnvironment environment,
            UploadService uploadService)
        {
            _db = db;
            _userManager = userManager;
            _environment = environment;
            _uploadService = uploadService;
        }
        [Authorize(Roles = "admin")]
        public IActionResult Index(string activ)
        {
            List<Post> posts;
            List<Book> books;
            
            if (activ == null)
            {
                posts = _db.Posts.Where(p => p.Active).ToList();
                books = _db.Books.Where(b => b.Active).ToList();
            }
            else
            {
                posts = _db.Posts.Where(p => p.Active == false).ToList();
                books = _db.Books.Where(b => b.Active == false).ToList();
            }

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
                    Active = post.Active,
                    FilePath = post.VirtualPath
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
                BookIdAndEnglishBookId bookIdAndEnglishBookId = _db.BookIdAndEnglishBookIds.FirstOrDefault(b => b.BookId == book.Id);
                int translationID = 0;
                if (bookIdAndEnglishBookId == null)
                    bookIdAndEnglishBookId = _db.BookIdAndEnglishBookIds.FirstOrDefault(b => b.EnglishBookId == book.Id);
                if (bookIdAndEnglishBookId != null)
                    translationID = bookIdAndEnglishBookId.EnglishBookId;
                
                LibraryListViewModel bookModel = new LibraryListViewModel
                {
                    Id = book.Id,
                   
                    Name = book.Name,
                    Type = new Type() {Name = s},
                    TypeContent = new TypeContent() {Name = "Книга"},
                    TypeState = book.IsRecipe ? new TypeState() {Name = "Рецепт"} : new TypeState() {Name = ""},
                    DateCreate = book.DateCreate,
                    Active = book.Active,
                    FilePath = book.VirtualPath,
                    TranslationID = translationID,
                };
                List<Author> bookAuthors =
                    _db.BookAuthors.Where(b => b.BookId == book.Id).Select(a => a.Author).ToList();
                bookModel.BookAuthors = bookAuthors;
                models.Add(bookModel);

            }
            return View(models);
        }
        [Authorize(Roles = "admin")]
        public IActionResult Details(int id)
        {
            Post post = _db.Posts.FirstOrDefault(p => p.Id == id);
            ViewBag.PostPath = Request.Scheme + "://" + Request.Host.Value + "/" + post.VirtualPath;
            return View(post);
        }
        [Authorize(Roles = "admin")]
        public IActionResult ValTemplate(int templatesId)
        {
            if (templatesId != 0)
            {
                Template template = _db.Templates.FirstOrDefault(t => t.Id == templatesId);
                return Json(template);
            }

            return Json(null);
        }
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View(new MaterialCreateViewModel());
        }
        
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Create(MaterialCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                Post post = new Post()
                {
                    Id = model.Id,
                    Title = model.Title,
                    Author = model.Author,
                    TextContent = model.TextContent,
                    CategoryId = model.CategoryId,
                    TypeContentId = model.TypeContentId,
                    TypeStateId = model.TypeStateId,
                    TypeId = model.TypeId,
                    PhysicalPath = model.PhysicalPath,
                    VirtualPath = Load(model.Id, model.VirtualPath),
                    UserId = _userManager.GetUserId(User)
                };
                _db.Posts.Add(post);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }
        
        [Authorize(Roles = "admin")]
        public IActionResult CreateTemplateAjax(Template template)
        {
            if (template.Name != null)
            {
                _db.Templates.Add(template);
                _db.SaveChanges();
            }

            MaterialTemplateViewModel model = new MaterialTemplateViewModel()
            {
                Material = new MaterialCreateViewModel(),
                Templates = _db.Templates.ToList()
            };

            return PartialView("PartialViews/TemplatesPartial", model);
        }

        [Authorize(Roles = "admin")]
        public IActionResult CreateCategoryAjax(Category category)
        {
            if (category.Name != null)
            {
                _db.Categories.Add(category);
                _db.SaveChanges();
            }

            MaterialCategoryViewModel model = new MaterialCategoryViewModel()
            {
                Material = new MaterialCreateViewModel(),
                Categories = _db.Categories.Where(c=> c.Active).ToList(),
            };

            return PartialView("PartialViews/CategoriesPartial", model);
        }
        
        [Authorize(Roles = "admin")]
        public IActionResult CreateTypeStateAjax(TypeState typeState)
        {
            if (typeState.Name != null)
            {
                _db.TypeStates.Add(typeState);
                _db.SaveChanges();
            }
            MaterialTypeStateViewModel model = new MaterialTypeStateViewModel()
            {
                Material = new MaterialCreateViewModel(),
                TypeStates = _db.TypeStates.Where(c=> c.Active).ToList(),
            };
            return PartialView("PartialViews/TypeStatesPartial", model);
        }
        
        [Authorize(Roles = "admin")]
        public IActionResult DeleteTypeStateAjax(int id)
        {
            TypeState typeContent = _db.TypeStates.FirstOrDefault(c => c.Id == id);
            if (typeContent != null)
            {
                typeContent.Active = false;
                _db.SaveChanges();
            }

            MaterialTypeStateViewModel model = new MaterialTypeStateViewModel()
            {
                Material = new MaterialCreateViewModel(),
                TypeStates = _db.TypeStates.Where(c=> c.Active).ToList(),
            };

            return PartialView("PartialViews/TypeStatesPartial", model);
        }

        [Authorize(Roles = "admin")]
        public IActionResult CreateTypeContentAjax(TypeContent typeContent)
        {
            if (typeContent.Name != null)
            {
                _db.TypeContents.Add(typeContent);
                _db.SaveChanges();
            }

            MaterialTypeContentViewModel model = new MaterialTypeContentViewModel()
            {
                Material = new MaterialCreateViewModel(),
                TypeContents = _db.TypeContents.Where(c=> c.Active).ToList(),
            };

            return PartialView("PartialViews/TypeContentPartial", model);
        }
        
        [Authorize(Roles = "admin")]
        public IActionResult DeleteTypeContentAjax(int id)
        {
            TypeContent typeContent = _db.TypeContents.FirstOrDefault(c => c.Id == id);
            if (typeContent != null)
            {
                typeContent.Active = false;
                _db.SaveChanges();
            }

            MaterialTypeContentViewModel model = new MaterialTypeContentViewModel()
            {
                Material = new MaterialCreateViewModel(),
                TypeContents = _db.TypeContents.Where(c=> c.Active).ToList(),
            };

            return PartialView("PartialViews/TypeContentPartial", model);
        }

        [Authorize(Roles = "admin")]
        public IActionResult CreateTypeAjax(Type type)
        {
            if (type.Name != null)
            {
                _db.Types.Add(type);
                _db.SaveChanges();
            }

            MaterialTypeViewModel model = new MaterialTypeViewModel()
            {
                Material = new MaterialCreateViewModel(),
                Types = _db.Types.Where(t=>t.Active).ToList()
            };

            return PartialView("PartialViews/TypesPartial", model);
        }

        [Authorize(Roles = "admin")]
        public IActionResult DeleteCategoryAjax(int id)
        {
            Category category = _db.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                category.Active = false;
               
                _db.SaveChanges();
            }

            MaterialCategoryViewModel model = new MaterialCategoryViewModel()
            {
                Material = new MaterialCreateViewModel(),
                Categories = _db.Categories.Where(c=>c.Active).ToList(),
            };

            return PartialView("PartialViews/CategoriesPartial", model);
        }
        
        [Authorize(Roles = "admin")]
        public IActionResult DeleteTemplateAjax(int id)
        {
            Template template = _db.Templates.FirstOrDefault(c => c.Id == id);
            if (template != null)
            {
                _db.Templates.Remove(template);
                _db.SaveChanges();
            }

            MaterialTemplateViewModel model = new MaterialTemplateViewModel()
            {
                Material = new MaterialCreateViewModel(),
                Templates = _db.Templates.ToList(),
            };

            return PartialView("PartialViews/TemplatesPartial", model);
        }

        [Authorize(Roles = "admin")]
        public IActionResult DeleteTypeAjax(int id)
        {
            Type type = _db.Types.FirstOrDefault(t => t.Id == id);
            if (type != null)
            {
                type.Active = false;
                _db.SaveChanges();
            }

            MaterialTypeViewModel model = new MaterialTypeViewModel()
            {
                Material = new MaterialCreateViewModel(),
                Types = _db.Types.Where(t=> t.Active ).ToList()
            };

            return PartialView("PartialViews/TypesPartial", model);
        }

        [Authorize(Roles = "admin")]
        private string Load(int id, IFormFile file)
        {
            if (file != null)
            {
                string path = Path.Combine(_environment.ContentRootPath + $"/wwwroot/PostsFiles/{id}");
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
        public IActionResult ViewComment(int id, string type)
        {
            List<Comment> comments;
            if (type == "book")
            {
                comments = _db.Comments.Where(c => c.BookId == id)
                                .OrderByDescending(g => g.DateOfCreate).ToList();
            }
            else
            {
                comments = _db.Comments.Where(c => c.PostId == id)
                    .OrderByDescending(g => g.DateOfCreate).ToList();  
            }
            
            return PartialView("PartialViews/CommentsPartial", comments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(int sourceId, string description, string type)
        {
            if (sourceId != 0 && description != null)
            {
                Comment comment = new Comment();
                if (type == "book")
                    comment.BookId = sourceId;
                else
                    comment.PostId = sourceId;
                comment.Description = description;
                comment.AuthorId = _userManager.GetUserId(User);
                await _db.Comments.AddAsync(comment);
                await _db.SaveChangesAsync();
            }

            List<Comment> comments;
            if (type == "book")
            {
                comments = await _db.Comments.Include(c => c.Author).Where(c => c.BookId == sourceId)
                    .OrderByDescending(g => g.DateOfCreate).ToListAsync();
            }
            else
            {
                comments = await _db.Comments.Include(c => c.Author).Where(c => c.PostId == sourceId)
                    .OrderByDescending(g => g.DateOfCreate).ToListAsync();
            }
            
            return PartialView("PartialViews/CommentsPartial", comments);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteComment(int id, int sourceId, string type)
        {
            Comment comment = await _db.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (comment != null)
            {
                _db.Comments.Remove(comment);
                await _db.SaveChangesAsync();
            }

            List<Comment> comments;
            if (type == "book")
            { 
                comments = await _db.Comments.Where(c => c.BookId == sourceId)
                                .OrderByDescending(g => g.DateOfCreate).ToListAsync();
            }
            else
            {
                comments = await _db.Comments.Where(c => c.PostId == sourceId)
                    .OrderByDescending(g => g.DateOfCreate).ToListAsync();
            }
            
            return PartialView("PartialViews/CommentsPartial", comments);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Edit(int id)
        {
            if (id != 0)
            {
                Post post = _db.Posts.FirstOrDefault(p => p.Id == id);
                MaterialCreateViewModel model = new MaterialCreateViewModel()
                {
                    Id = post.Id,
                    Title = post.Title,
                    Author = post.Author,
                    TextContent = post.TextContent,
                    _virtualPath = post.VirtualPath,
                    CategoryId = post.CategoryId,
                    PhysicalPath = post.PhysicalPath,
                    TypeId = post.TypeId,
                };

                return View(model);
            }

            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Edit(MaterialCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Post post = _db.Posts.FirstOrDefault(p => p.Id == model.Id);
                if (post != null)
                {
                    post.Id = model.Id;
                    post.Title = model.Title;
                    post.Author = model.Author;
                    post.TextContent = model.TextContent;
                    post.CategoryId = model.CategoryId;
                    post.TypeId = model.TypeId;
                    post.PhysicalPath = model.PhysicalPath;
                    post.VirtualPath = Load(model.Id, model.VirtualPath);
                    _db.Posts.Update(post);
                    _db.SaveChanges();
                }
                return RedirectToAction("Index", "SourceManage");
            }

            return View(model);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            if (id != 0)
            {
                Post post = _db.Posts.FirstOrDefault(v => v.Id == id);
                if (post != null)
                {
                    return View(post);
                }
            }

            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)
        {
            if (id != 0)
            {
                Post post = _db.Posts.FirstOrDefault(v => v.Id == id);
                if (post != null)
                {
                    if (post.Active == false)
                        post.Active = true;
                    else
                        post.Active = false;
                    
                    _db.Posts.Update(post);
                    _db.SaveChanges();
                }
            }

            return RedirectToAction("Index", "SourceManage");
        }
    }
}