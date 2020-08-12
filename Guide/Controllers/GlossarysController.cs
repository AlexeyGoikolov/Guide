using System.Collections.Generic;
using System.Linq;
using Guide.Models;
using Guide.Models.Data;
using Microsoft.AspNetCore.Mvc;
namespace Guide.Controllers
{
    public class GlossarysController : Controller
    {
        private readonly GuideContext _db;

        public GlossarysController(GuideContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Glossary> glossaries = _db.Glossaries.OrderBy(g => g.Name).ToList();
            return View(glossaries);
        }
        public IActionResult Preview(string id)
        {
            Glossary glossary = _db.Glossaries.FirstOrDefault(g => g.Id == id);
            return View(glossary);
        }
    }
}