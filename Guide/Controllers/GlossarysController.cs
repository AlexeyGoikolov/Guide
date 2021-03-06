﻿using System.Collections.Generic;
using System.Linq;
using Guide.Models;
using Guide.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Guide.Controllers
{
    [Authorize]
    public class GlossarysController : Controller
    {
        private readonly GuideContext _db;

        public GlossarysController(GuideContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Glossary> glossaries = _db.Glossaries.Where(g => g.Language == Models.Language.ru).OrderBy(g => g.Name)
                .ToList();
            return View(glossaries);
        }

        public IActionResult Preview(int id)
        {
            Glossary glossary = _db.Glossaries.FirstOrDefault(g => g.Id == id);
            if (glossary != null)
            {
                glossary.GlossarysInterpretations =
                    _db.Interpretations.Where(i => i.GlossaryId == glossary.GlossarysId).ToList();
                return View(glossary);
            }

            return NotFound();
        }
    }
}