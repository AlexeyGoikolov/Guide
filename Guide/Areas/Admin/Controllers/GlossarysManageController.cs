using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guide.Models;
using Guide.Models.Data;
using Guide.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            List<Glossary> glossaries = _db.Glossaries.Where(g=>g.Active==true).OrderBy(g => g.Name).ToList();
            return View(glossaries);
        }
        
        public IActionResult Preview(int id)
        {
            Glossary glossary = _db.Glossaries.FirstOrDefault(g => g.Id == id);
            return View(glossary);
        }
        
        public IActionResult Create(int id)
        {
            Glossary glossary = _db.Glossaries.FirstOrDefault(g => g.Id == id);
            if (glossary != null)
            {
                return View(new GlossaryViewModel() {Id = id, Name = glossary.Name, Action = "AddInterpretation"});
            }

            return View(new GlossaryViewModel());
        }
        
        [HttpPost]
        public IActionResult Create(GlossaryViewModel model)
        {
            if (ModelState.IsValid )
            {
                Glossary glossary = _db.Glossaries.FirstOrDefault(g => g.Name == model.Name);
                if (glossary == null)
                {
                      glossary = new Glossary() {Name = model.Name};
                      _db.Glossaries.Add(glossary);
                      Interpretation interpretation = new Interpretation()
                      {
                          GlossaryId = glossary.Id,
                          Description = model.Description,
                          Abbreviation = model.Abbreviation,
                          Source = model.Source
                      };
                      _db.Interpretations.Add(interpretation);
                     _db.SaveChanges();
                     return RedirectToAction("Index");
                }
                else
                {
                    Interpretation interpretation = new Interpretation()
                    {
                        GlossaryId = glossary.Id,
                        Description = model.Description,
                        Abbreviation = model.Abbreviation,
                        Source = model.Source
                    };
                    _db.Interpretations.Add(interpretation);
                    _db.SaveChanges();
                    return View("~/Areas/Admin/Views/GlossarysManage/Preview.cshtml", _db.Glossaries.FirstOrDefault(g => g.Id == model.Id));
                }
            }
            return View(model);
        }
        
        public IActionResult Delete(int id, int intrId)
        {
            Glossary glossary = _db.Glossaries.FirstOrDefault(v => v.Id == id);
            if (glossary != null && intrId == 0)
            {
                glossary.Active = false;
                _db.Glossaries.Update(glossary);
                List<Interpretation> interpretations =
                    _db.Interpretations.Where(i => i.GlossaryId == glossary.Id).ToList();
                foreach (var interpretationVar in interpretations)
                {
                    interpretationVar.Active = false;
                }

                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            Interpretation interpretation = _db.Interpretations.FirstOrDefault(i => i.Id == intrId);
            if (glossary != null && interpretation != null)
            {
                interpretation.Active = false;
                _db.Interpretations.Update(interpretation);
                _db.SaveChanges();
                return View("~/Areas/Admin/Views/GlossarysManage/Preview.cshtml", _db.Glossaries.FirstOrDefault(g => g.Id == id));
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

        public IActionResult Edit(int id, int intrId)
        {
            Glossary glossary = _db.Glossaries.FirstOrDefault(g => g.Id == id);
            Interpretation interpretation = _db.Interpretations.FirstOrDefault(i => i.Id == intrId);
            if (glossary != null && interpretation!=null)
            {
                return View(new GlossaryViewModel()
                {
                    Id = id,
                    InterpretationId = intrId,
                    Name = glossary.Name,
                    Description = interpretation.Description,
                    Abbreviation = interpretation.Abbreviation,
                    Source = interpretation.Source
                });
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GlossaryViewModel model)
        {
            if (ModelState.IsValid )
            {
                Glossary glossary = await _db.Glossaries.FirstOrDefaultAsync(g => g.Id == model.Id);
                if (glossary != null && model.Name != glossary.Name)
                {
                    glossary.Name = model.Name;
                    _db.Glossaries.Update(glossary);
                    await _db.SaveChangesAsync();
                }

                Interpretation interpretation = await _db.Interpretations.FirstOrDefaultAsync(i => i.Id == model.InterpretationId);
                if (interpretation != null && 
                    (interpretation.Description!=model.Description || 
                     interpretation.Abbreviation!=model.Abbreviation ||
                     interpretation.Source!=model.Source))
                {
                    interpretation.Description = model.Description;
                    interpretation.Abbreviation = model.Abbreviation;
                    interpretation.Source = model.Source;
                    _db.Interpretations.Update(interpretation);
                    await _db.SaveChangesAsync();
                }
                return View("~/Areas/Admin/Views/GlossarysManage/Preview.cshtml", _db.Glossaries.FirstOrDefault(g => g.Id == model.Id));
            }
            return View(model);
        }
    }
}