﻿﻿using System.Collections.Generic;
  using System.Linq;
  using Guide.Models;
using Guide.Models.Data;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;


namespace Guide.Controllers
{
    [Authorize]
    public class SourceController : Controller
    {
        private readonly GuideContext _db;

        public SourceController(GuideContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            List<Source> sources = _db.Sources.Where(b => b.Active).ToList();
            ;

            foreach (var source in sources)
            {
                SourceIdAndEnglishSourceId sourceIdAndEnglishSourceId =
                    _db.SourceIdAndEnglishSourceIds.FirstOrDefault(b => b.SourceId == source.Id);
                int translationID = 0;
                if (sourceIdAndEnglishSourceId == null)
                    sourceIdAndEnglishSourceId =
                        _db.SourceIdAndEnglishSourceIds.FirstOrDefault(b => b.EnglishSourceId == source.Id);
                if (sourceIdAndEnglishSourceId != null)
                    translationID = sourceIdAndEnglishSourceId.EnglishSourceId;
                source.TranslationID = translationID;
                source.Authors = _db.SourceAuthors.Where(b => b.SourceId == source.Id).Select(a => a.Author).ToList();
                source.BusinessProcesses = _db.SourceBusinessProcesses.Where(b => b.SourceId == source.Id)
                    .Select(b => b.BusinessProcess).ToList();
            }

            return View(sources);
        }


        public IActionResult Details(int id)
        {
            int translationID = 0;
            ViewBag.BookTransferLanguage = 0;
            Source source = _db.Sources.FirstOrDefault(b => b.Id == id);
            source.Authors = _db.SourceAuthors.Where(c => c.SourceId == source.Id).Select(s => s.Author).ToList();
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
                sourceIdAndEnglishSourceId =
                    _db.SourceIdAndEnglishSourceIds.FirstOrDefault(b => b.EnglishSourceId == id);

            if (translationID == 0 && sourceIdAndEnglishSourceId != null)
            {
                translationID = sourceIdAndEnglishSourceId.SourceId;
                ViewBag.BookTransferLanguage = "ru";
            }

            ViewBag.BookTransferId = translationID;
            return View(source);
        }

        public IActionResult ReadBook(int id)
        {
            Source source = _db.Sources.FirstOrDefault(b => b.Id == id);
            ViewBag.BookPath = Request.Scheme + "://" + Request.Host.Value + "/" + source.VirtualPath;
            return View(source);
        }
    }
}
