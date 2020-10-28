using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Guide.Models;
using Guide.Models.Data;
using Guide.Services;
using Guide.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _environment;
        private readonly UploadService _uploadService;

        public SourceManageController(GuideContext db, UserManager<User> userManager, IWebHostEnvironment environment, UploadService uploadService)
        {
            _db = db;
            _userManager = userManager;
            _environment = environment;
            _uploadService = uploadService;
        }
        [Authorize(Roles = "admin")]
        public IActionResult Index(string activ)
        {
            List<Source> sources;

            if (activ == null)
                sources = _db.Sources.Where(s => s.Active).ToList();
            else
                sources = _db.Sources.Where(b => b.Active == false).ToList();
            
            foreach (var source in sources)
            {
                SourceIdAndEnglishSourceId sourceIdAndEnglishSourceId =
                    _db.SourceIdAndEnglishSourceIds.FirstOrDefault(b => b.SourceId == source.Id);
                int translationId = 0;
                if (sourceIdAndEnglishSourceId == null)
                    sourceIdAndEnglishSourceId =
                        _db.SourceIdAndEnglishSourceIds.FirstOrDefault(b => b.EnglishSourceId == source.Id);
                if (sourceIdAndEnglishSourceId != null)
                    translationId = sourceIdAndEnglishSourceId.EnglishSourceId;
                source.TranslationID = translationId;
                source.Authors = _db.SourceAuthors.Where(b => b.SourceId == source.Id)
                    .Select(a => a.Author).ToList();
                source.BusinessProcesses = _db.SourceBusinessProcesses
                    .Where(b => b.SourceId == source.Id)
                    .Select(b => b.BusinessProcess).ToList();
            }

            return View(sources);
        }
        [Authorize(Roles = "admin")]
        public IActionResult Create(int sourceId)
        {
            SourceCreateViewModel model = new SourceCreateViewModel
            {
                AllAuthors = _db.Authors.Where(c => c.Active).ToList(),
                BusinessProcessesList = _db.BusinessProcesses.ToList(),
                SourceId = sourceId
            };
            ViewBag.SourceFormatAvailable =
                "Документы: pdf, doc, docx, txt, xls, xlsx\r\n" +
                "Видео: mp4, avi, mpeg, quicktime\r\n" +
                "Аудио: mp3, amr, ogg\r\nИзображения: jpg, gif, png, bmp\r\n" +
                "Все другие форматы загружать в архивах zip, 7z, rar";
            ViewBag.CoverFormatAvailable = "Разрешено загрузить jpeg(jpg), gif, png, bmp";
            return View(model);
        }
        
        [Authorize(Roles = "admin")]        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SourceCreateViewModel model, string authors, IFormFile coverFile,
            IFormFile sourceFile)
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
                    SaveSourceAuthors(authors, source);
                if (model.BusinessProcesses != null)
                    SaveBusinessProcessesSource(model, source);
                if (model.SourceId != 0)
                    SaveBookIdAndEnglishBookId(model, source);
                return Json(true);
            }

            return Json("falseData");
        }
        
        [Authorize(Roles = "admin")]
        public void SaveSourceAuthors(string authors, Source source)
        {
            string[] authorsId = authors.Split(',');
            foreach (var authorName in authorsId)
            {
                var author = _db.Authors.FirstOrDefault(a => a.Name == authorName);
                if (author != null)
                {
                    SourceAuthor check =
                        _db.SourceAuthors.FirstOrDefault(s => s.SourceId == source.Id && s.AuthorId == author.Id);
                    if (check == null)
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
        }
        
        [Authorize(Roles = "admin")]
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
        
        [Authorize(Roles = "admin")]
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
                        SourceBusinessProcess check =
                            _db.SourceBusinessProcesses.FirstOrDefault(s =>
                                s.SourceId == source.Id && s.BusinessProcessId == process.Id);
                        if (check == null)
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
        }
        
        [Authorize(Roles = "admin")]
        public IActionResult Details(int id)
        {
            int translationId = 0;
            ViewBag.BookTransferLanguage = 0;
            Source source = _db.Sources.FirstOrDefault(b => b.Id == id);
            source.Authors = _db.SourceAuthors.Where(c => c.SourceId == source.Id).Select(s=>s.Author).ToList();
            source.BusinessProcesses = _db.SourceBusinessProcesses.Where(s => s.SourceId == source.Id)
                .Select(s => s.BusinessProcess).ToList();
            var sourceIdAndEnglishSourceId = _db.SourceIdAndEnglishSourceIds
                .FirstOrDefault(b => b.SourceId == id);

            if (sourceIdAndEnglishSourceId != null)
            {
                translationId = sourceIdAndEnglishSourceId.EnglishSourceId;
                ViewBag.BookTransferLanguage = "en";
            }

            if (sourceIdAndEnglishSourceId == null)
                sourceIdAndEnglishSourceId =
                    _db.SourceIdAndEnglishSourceIds.FirstOrDefault(b => b.EnglishSourceId == id);

            if (translationId == 0 && sourceIdAndEnglishSourceId != null)
            {
                translationId = sourceIdAndEnglishSourceId.SourceId;
                ViewBag.BookTransferLanguage = "ru";
            }

            ViewBag.BookTransferId = translationId;

            return View(source);
        }
        
        [Authorize(Roles = "admin")]
        private string Load(string name, IFormFile file)
        {
            if (file.FileName.Contains(".js") || file.FileName.Contains(".JS"))
                return null;
            string path = Path.Combine(_environment.WebRootPath + "/Files/");
            string additionalName = file.FileName;
            if (name.Contains(" "))
                name = name.Replace(" ", "_");
            if (file.FileName.Contains(" "))
                additionalName = file.FileName.Replace(" ", "_");
            string additional = DateTime.Now.ToBinary().ToString().Trim('-');
            string fileName = $"{name}_{additional.Substring(0, additional.Length / 2)}_{additionalName}";
            string filePath = $"Files/{fileName}";
            if (!Directory.Exists("wwwroot/Files"))
            {
                Directory.CreateDirectory("wwwroot/Files");
            }
            _uploadService.Upload(path,fileName, file);
            return filePath;
        }
        
        [Authorize(Roles = "admin")]
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
        
        [Authorize(Roles = "admin")]
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

            return RedirectToAction("Index", "SourceManage");
        }
        
        [Authorize(Roles = "admin")]
        public IActionResult ReadBook(int id)
        {
            Source source = _db.Sources.FirstOrDefault(b => b.Id == id);
            ViewBag.Path = Request.Scheme + "://" + Request.Host.Value + "/" + source.VirtualPath;
            return View(source);
        }
        
        [Authorize(Roles = "admin")]
        public IActionResult CreateAuthorAjax(string name)
        {
            List<Author> model = new List<Author>();
            if (name != null)
            {
                Author author = new Author
                {
                    Name = name
                };
                _db.Authors.Add(author);
                _db.SaveChanges();
                model.Add(author);
                ViewBag.Check = "Create";
                
            }
            else
            {
                ViewBag.Check = "Load";
                model = _db.Authors.Where(a => a.Active).ToList();
            }
            return PartialView("PartialViews/AuthorPartial", model);
        }
        
        [Authorize(Roles = "admin")]
        public IActionResult DeleteAuthorAjax(int id)
        {
            Author author = _db.Authors.FirstOrDefault(a => a.Id == id);
            if (author != null)
            {
                author.Active = false;
                _db.SaveChanges();
            }

            List<Author> model = _db.Authors.Where(c => c.Active).ToList();
            ViewBag.Check = "Delete";
            return PartialView("PartialViews/AuthorPartial", model);
        }
        
        [Authorize(Roles = "admin")]
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
                SourceTypes = _db.SourceTypes.Where(c => c.Active).ToList(),
            };

            return PartialView("PartialViews/TypeContentPartial", model);
        }
        
        [Authorize(Roles = "admin")]
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
                SourceTypes = _db.SourceTypes.Where(c => c.Active).ToList(),
            };

            return PartialView("PartialViews/TypeContentPartial", model);
        }
        
        [Authorize(Roles = "admin")]
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
                Categories = _db.Categories.Where(c => c.Active).ToList()
            };

            return PartialView("PartialViews/CategoriesPartial", model);
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

            SourceCategoryViewModel model = new SourceCategoryViewModel()
            {
                Source = new Source(),
                Categories = _db.Categories.Where(c => c.Active).ToList()
            };

            return PartialView("PartialViews/CategoriesPartial", model);
        }
        
        [Authorize]
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

        [Authorize]
        [HttpGet]
        public IActionResult ViewComment(int id)
        {
            List<Comment> comments = _db.Comments.Where(c => c.SourceId == id)
                .OrderByDescending(g => g.DateOfCreate).ToList();
            return PartialView("PartialViews/CommentsPartial", comments);
        }
        
        [Authorize]
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

        [Authorize(Roles = "admin")]
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
                SourceStates = _db.SourceStates.Where(c => c.Active).ToList(),
            };
            return PartialView("PartialViews/SourceStatesPartial", model);
        }

        [Authorize(Roles = "admin")]
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
                SourceStates = _db.SourceStates.Where(c => c.Active).ToList(),
            };

            return PartialView("PartialViews/SourceStatesPartial", model);
        }
        
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Source source = _db.Sources.FirstOrDefault(b => b.Id == id);
            SourceCreateViewModel model = new SourceCreateViewModel
            {
                Name = source.Name,
                ISBN = source.ISBN,
                Edition = source.Edition,
                YearOfWriting = source.YearOfWriting,
                SourceDescription = source.SourceDescription,
                Keys = source.Keys,
                PhysicalPath = source.PhysicalPath,
                AdditionalInformation = source.AdditionalInformation,
                AllAuthorsOfThisSourse =
                    _db.SourceAuthors.Where(c => c.SourceId == source.Id).Select(s => s.Author).ToList(),
                AllAuthors = _db.Authors.Where(a => a.Active).ToList(),
                BusinessProcessesList = _db.BusinessProcesses.ToList(),
                BusinessProcessesListOfThisSourse = _db.SourceBusinessProcesses.Where(s => s.SourceId == source.Id)
                    .Select(s => s.BusinessProcess).ToList(),
                SourceId = id
            };
            return View(model);
        }
        
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Edit(SourceCreateViewModel model, string authors)
        {
            Source source = _db.Sources.FirstOrDefault(b => b.Id == model.SourceId);
            if (model != null)
            {
                source.Name = model.Name;
                source.ISBN = model.ISBN;
                source.Edition = model.Edition;
                source.YearOfWriting = model.YearOfWriting;
                source.SourceDescription = model.SourceDescription;
                source.Keys = model.Keys;
                source.PhysicalPath = model.PhysicalPath;
                source.AdditionalInformation = model.AdditionalInformation;
                source.DateUpdate = DateTime.Now;
                source.SourceTypeId = model.SourceTypeId;
                source.SourceStateId = model.SourceStateId;
                source.CategoryId = model.CategoryId;
                _db.Sources.Update(source);
                _db.SaveChanges();
                if (authors != null)
                    SaveSourceAuthors(authors, source);
                if (model.BusinessProcesses != null)
                    SaveBusinessProcessesSource(model, source);
                return Json(true);
            }
            return Json("falseData");
        }
    }
}