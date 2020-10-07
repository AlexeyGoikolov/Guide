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
    [Authorize(Roles = "admin")]
    public class SourceManageController : Controller
    {
        
        private readonly GuideContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IHostEnvironment _environment;
        private readonly UploadService _uploadService;

        public SourceManageController(GuideContext db, UserManager<User> userManager, IHostEnvironment environment, UploadService uploadService)
        {
            _db = db;
            _userManager = userManager;
            _environment = environment;
            _uploadService = uploadService;
        }

        public IActionResult Index(string activ)
        {
            List<Source> sources;
            
            if (activ == null)
                sources = _db.Sources.Where(s => s.Active).ToList();
            else
                sources = _db.Sources.Where(b => b.Active == false).ToList();
            
            foreach (var source in sources)
            {
                SourceIdAndEnglishSourceId sourceIdAndEnglishSourceId = _db.SourceIdAndEnglishSourceIds.FirstOrDefault(b => b.SourceId == source.Id);
                int translationID = 0;
                if (sourceIdAndEnglishSourceId == null)
                    sourceIdAndEnglishSourceId = _db.SourceIdAndEnglishSourceIds.FirstOrDefault(b => b.EnglishSourceId == source.Id);
                if (sourceIdAndEnglishSourceId != null)
                    translationID = sourceIdAndEnglishSourceId.EnglishSourceId;
                source.TranslationID = translationID;
                source.Authors = _db.SourceAuthors.Where(b => b.SourceId == source.Id).Select(a => a.Author).ToList();
                source.BusinessProcesses = _db.SourceBusinessProcesses.Where(b => b.SourceId == source.Id)
                    .Select(b => b.BusinessProcess).ToList();
            }
            return View(sources);
        }
        public IActionResult Create(int sourceId)
        {
            SourceCreateViewModel model = new SourceCreateViewModel
            {
                AllAuthors = _db.Authors.Where(c=> c.Active).ToList(),
                BusinessProcessesList = _db.BusinessProcesses.ToList()
                
            };
            model.SourceId = sourceId;
            ViewBag.SourceFormatAvailable =
                "Документы: pdf, doc, docx, txt, xls, xlsx\r\n" +
                "Видео: mp4, avi, mpeg, quicktime\r\n" +
                "Аудио: mp3, amr, ogg\r\nИзображения: jpg, png, bmp\r\n" +
                "Все другие форматы загружать в архивах zip, 7z, rar";
            ViewBag.CoverFormatAvailable = "Разрешено загрузить jpeg(jpg), png, bmp";
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SourceCreateViewModel model, string authors, IFormFile coverFile, IFormFile sourceFile)
        {
            if (model.Name != null && sourceFile != null)
            {
                Source source = new Source
                {
                    SourceTypeId = model.SourceTypeId,
                    SourceStateId = model.SourceStateId,
                    CategoryId = model.CategoryId,
                    Name = model.Name,
                    SourceDescription = model.SourceDescription,
                    AdditionalInformation = model.AdditionalInformation,
                    IsRecipe = model.IsRecipe,
                    ISBN = model.ISBN,
                    Edition = model.Edition,
                    PhysicalPath = model.PhysicalPath,
                    YearOfWriting = model.YearOfWriting,
                    Keys = model.Keys
                };
                if (coverFile != null)
                {
                    if (FileTypeChecker.IsValidImage(coverFile))
                        source.CoverPath = Load(model.Name, coverFile);
                    else
                        return Json("falseCoverType");
                }
                else
                    source.CoverPath = "Files/Cover_missing.png";

                if (FileTypeChecker.IsValidDocument(sourceFile)
                    || FileTypeChecker.IsValidImage(sourceFile)
                    || FileTypeChecker.IsValidVideo(sourceFile)
                    || FileTypeChecker.IsValidAudio(sourceFile))
                {
                    source.VirtualPath = Load(model.Name, sourceFile);
                }
                else
                    return Json("falseBookType");
                
                _db.Sources.Add(source);
                _db.SaveChanges();
                if (authors != null)
                    SaveSourceAuthors(authors,source);
                if (model.BusinessProcesses != null)
                    SaveBusinessProcessesSource(model, source);
                if (model.SourceId != 0)
                    SaveBookIdAndEnglishBookId(model,source);
                return Json(true);
            }
            return Json("falseData");
        }

        public void SaveSourceAuthors(string authors, Source source )
        {
            string[] authorsId = authors.Split(',');
            foreach (var authorName in authorsId)
            {
                var author = _db.Authors.FirstOrDefault(a => a.Name == authorName);
                if (author != null)
                {
                    SourceAuthor sourceAuthor = new SourceAuthor()
                    {
                        SourceId = source.Id,
                        AuthorId = author.Id
                    };
                    _db.SourceAuthors.Add(sourceAuthor);
                    _db.SaveChanges();    
                }
            }
        }

        public void SaveBookIdAndEnglishBookId(SourceCreateViewModel model, Source source)
        {
            SourceIdAndEnglishSourceId sourceIdAndEnglishSourceId = new SourceIdAndEnglishSourceId()
            {
                SourceId = model.SourceId,
                EnglishSourceId = source.Id
            };
            _db.SourceIdAndEnglishSourceIds.Add(sourceIdAndEnglishSourceId);
            _db.SaveChanges();

        }

        public void SaveBusinessProcessesSource(SourceCreateViewModel model, Source source)
        {
            string[] businessProcesses = model.BusinessProcesses.Split(',');
            foreach (var businessProcess in businessProcesses)
            {
                if (businessProcess != "")
                {
                    var process = _db.BusinessProcesses.FirstOrDefault(b => b.Name == businessProcess);
                    if (process != null)
                    {
                        SourceBusinessProcess sourceBusinessProcess = new SourceBusinessProcess()
                        {
                            SourceId = source.Id,
                            BusinessProcessId = process.Id
                        };
                        _db.SourceBusinessProcesses.Add(sourceBusinessProcess);
                        _db.SaveChanges(); 
                    }
                }
            }
        }

        public IActionResult Details(int id)
        {
            int translationID = 0;
            ViewBag.BookTransferLanguage = 0;
            Source source = _db.Sources.FirstOrDefault(b => b.Id == id);
            source.BusinessProcesses = _db.SourceBusinessProcesses.Where(s => s.SourceId == source.Id)
                .Select(s => s.BusinessProcess).ToList();
            SourceIdAndEnglishSourceId sourceIdAndEnglishSourceId = new SourceIdAndEnglishSourceId();
            sourceIdAndEnglishSourceId = _db.SourceIdAndEnglishSourceIds.FirstOrDefault(b => b.SourceId == id);

            if (sourceIdAndEnglishSourceId != null)
            {
                translationID = sourceIdAndEnglishSourceId.EnglishSourceId;
                ViewBag.BookTransferLanguage = "en";
            }
            
            if (sourceIdAndEnglishSourceId == null)
                sourceIdAndEnglishSourceId = _db.SourceIdAndEnglishSourceIds.FirstOrDefault(b => b.EnglishSourceId == id);
            
            if(translationID == 0 && sourceIdAndEnglishSourceId != null)
            {
                translationID = sourceIdAndEnglishSourceId.SourceId;
                ViewBag.BookTransferLanguage = "ru";
            }
            
            ViewBag.BookTransferId = translationID;
            
            return View(source);
        }

        private string Load(string name, IFormFile file)
        {
            string path = Path.Combine(_environment.ContentRootPath + $"/wwwroot/Files/{name}");
            string filePath = $"Files/{name}/{file.FileName}";
            if (!Directory.Exists($"wwwroot/Files/{name}"))
            {
                Directory.CreateDirectory($"wwwroot/Files/{name}");
            }
            _uploadService.Upload(path, file.FileName, file);
            return filePath;
        }
        
        public IActionResult Delete(int id)
        {
            if (id != 0)
            {
                Source source = _db.Sources.FirstOrDefault(s => s.Id == id);
                if (source != null)
                {
                    return View(source);
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
                Source source = _db.Sources.FirstOrDefault(v => v.Id == id);
                if (source != null)
                {
                    if (source.Active)
                        source.Active = false;
                    else
                        source.Active = true;
                    _db.Sources.Update(source); 
                    _db.SaveChanges();
                }
            }
            return RedirectToAction("Index" , "SourceManage");
        }

        public  IActionResult ReadBook(int id)
        {
            Source source = _db.Sources.FirstOrDefault(b => b.Id == id);
            ViewBag.Path = Request.Scheme + "://" + Request.Host.Value + "/" + source.VirtualPath;
            return View(source) ;
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
            SourceCreateViewModel model = new SourceCreateViewModel
            {
                AllAuthors = _db.Authors.Where(a=> a.Name == name).ToList()
            };
            return PartialView("PartialViews/AuthorPartial", model);
        }
        public IActionResult DeleteAuthorAjax(int id)
        {
            Author author = _db.Authors.FirstOrDefault(a => a.Id == id);
            if (author != null)
            {
                author.Active = false;
                _db.SaveChanges();
            }
            SourceCreateViewModel model = new SourceCreateViewModel()
            {
                AllAuthors = _db.Authors.Where(c=> c.Active).ToList(),
            };
            return PartialView("PartialViews/AuthorPartial", model);
        }

        public IActionResult CreateTypeContentAjax(SourceType sourceType)
        {
            if (sourceType.Name != null)
            {
                _db.SourceTypes.Add(sourceType);
                _db.SaveChanges();
            }

            SourceTypeContentViewModel model = new SourceTypeContentViewModel
            {
                Source = new Source(),
                SourceTypes = _db.SourceTypes.Where(c=> c.Active).ToList(),
            };

            return PartialView("PartialViews/TypeContentPartial", model);
        }
        
        public IActionResult DeleteTypeContentAjax(int id)
        {
            SourceType sourceType = _db.SourceTypes.FirstOrDefault(c => c.Id == id);
            if (sourceType != null)
            {
                sourceType.Active = false;
                _db.SaveChanges();
            }

            SourceTypeContentViewModel model = new SourceTypeContentViewModel()
            {
                Source = new Source(),
                SourceTypes = _db.SourceTypes.Where(c=> c.Active).ToList(),
            };

            return PartialView("PartialViews/TypeContentPartial", model);
        }
        
        public IActionResult CreateCategoryAjax(Category category)
        {
            if (category.Name != null)
            {
                _db.Categories.Add(category);
                _db.SaveChanges();
            }

            SourceCategoryViewModel model = new SourceCategoryViewModel()
            {
                Source = new Source(),
                Categories = _db.Categories.Where(c=> c.Active).ToList(),
            };

            return PartialView("PartialViews/CategoriesPartial", model);
        }
        
        public IActionResult DeleteCategoryAjax(int id)
        {
            Category category = _db.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                category.Active = false;
               
                _db.SaveChanges();
            }

            SourceCategoryViewModel model = new SourceCategoryViewModel()
            {
                Source = new Source(),
                Categories = _db.Categories.Where(c=>c.Active).ToList(),
            };

            return PartialView("PartialViews/CategoriesPartial", model);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateComment(int sourceId, string description)
        {
            if (sourceId != 0 && description != null)
            {
                Comment comment = new Comment();
                comment.SourceId = sourceId;
                comment.Description = description;
                comment.AuthorId = _userManager.GetUserId(User);
                await _db.Comments.AddAsync(comment);
                await _db.SaveChangesAsync();
            }

            List<Comment> comments = await _db.Comments.Include(c => c.Author).Where(c => c.SourceId == sourceId)
                    .OrderByDescending(g => g.DateOfCreate).ToListAsync();

            return PartialView("PartialViews/CommentsPartial", comments);
        }
        
        [HttpGet]
        public IActionResult ViewComment(int id)
        {
            List<Comment> comments = _db.Comments.Where(c => c.SourceId == id)
                    .OrderByDescending(g => g.DateOfCreate).ToList();
            return PartialView("PartialViews/CommentsPartial", comments);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteComment(int id, int sourceId)
        {
            Comment comment = await _db.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (comment != null)
            {
                _db.Comments.Remove(comment);
                await _db.SaveChangesAsync();
            }

            List<Comment> comments = await _db.Comments.Where(c => c.SourceId == sourceId)
                                .OrderByDescending(g => g.DateOfCreate).ToListAsync();
            
            return PartialView("PartialViews/CommentsPartial", comments);
        }
        
        public IActionResult CreateSourceStateAjax(SourceState sourceState)
        {
            if (sourceState.Name != null)
            {
                _db.SourceStates.Add(sourceState);
                _db.SaveChanges();
            }
            SourceStateViewModel model = new SourceStateViewModel
            {
                Source = new Source(),
                SourceStates = _db.SourceStates.Where(c=> c.Active).ToList(),
            };
            return PartialView("PartialViews/SourceStatesPartial", model);
        }
        
        public IActionResult DeleteSourceStateAjax(int id)
        {
            SourceState sourceState = _db.SourceStates.FirstOrDefault(c => c.Id == id);
            if (sourceState != null)
            {
                sourceState.Active = false;
                _db.SaveChanges();
            }

            SourceStateViewModel model = new SourceStateViewModel()
            {
                Source = new Source(),
                SourceStates = _db.SourceStates.Where(c=> c.Active).ToList(),
            };

            return PartialView("PartialViews/SourceStatesPartial", model);
        }
    }
}