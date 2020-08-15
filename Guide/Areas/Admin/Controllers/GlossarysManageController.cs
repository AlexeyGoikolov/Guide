using System.Collections.Generic;
using System.Linq;
using Guide.Models;
using Guide.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Guide.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class GlossarysManageController : Controller
    {
        private readonly GuideContext _db;

        public GlossarysManageController(GuideContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Glossary> glossaries = _db.Glossaries.OrderBy(g => g.Name).ToList();
            return View(glossaries);
        }
        
        public IActionResult Preview(int id)
        {
            Glossary glossary = _db.Glossaries.FirstOrDefault(g => g.Id == id);
            return View(glossary);
        }
        
        public IActionResult Create()
        {
            return View(new Glossary());
        }
        
        [HttpPost]
        public IActionResult Create(Glossary model)
        {
            if (ModelState.IsValid )
            {
                _db.Glossaries.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        
        public IActionResult Delete(int id)
        {
            if (id != 0)
            {
                Glossary glossary = _db.Glossaries.FirstOrDefault(v => v.Id == id);
                if (glossary != null)
                {
                    return View(glossary);
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
                Glossary glossary = _db.Glossaries.FirstOrDefault(v => v.Id == id);
                if (glossary != null)
                {
                    _db.Glossaries.Remove(glossary);
                    _db.SaveChanges();
                }
            }
            return RedirectToAction("Index" , "GlossarysManage");
        }

        public IActionResult Edit(int id)
        {
            if (id != 0)
            {
                Glossary glossary = _db.Glossaries.FirstOrDefault(g => g.Id == id);
                return View(glossary);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Glossary model)
        {
            if (ModelState.IsValid )
            {
                _db.Glossaries.Update(model);
                _db.SaveChanges();
               return RedirectToAction("Index", "GlossarysManage");
            }
            return View(model);
        }
    }
}