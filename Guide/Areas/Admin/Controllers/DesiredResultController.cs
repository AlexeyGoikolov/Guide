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
    public class DesiredResultController : Controller
    {
        private readonly GuideContext _db;

        public DesiredResultController(GuideContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List< DesiredResult> desiredResults = _db.DesiredResults.ToList();
            return View(desiredResults);
        }
        // GET
        public IActionResult Create()
        {
            return View(new  DesiredResult());
        }

        [HttpPost]
        public IActionResult Create( DesiredResult desiredResult)
        {
            if (ModelState.IsValid)
            {
                _db.DesiredResults.Add(desiredResult);
                _db.SaveChanges();
                return RedirectToAction("Index", "StepsManage", new {area = "Admin"});
            }

            return NotFound();
        }
    }
}