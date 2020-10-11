using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guide.Models;
using Guide.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Guide.Controllers
{
    [Authorize]
    public class StepsController : Controller
    {
        private readonly GuideContext _db;

        public StepsController(GuideContext db)
        {
            _db = db;
        }

        // GET
        public IActionResult Details(int id, string back)
        {
            Step step = _db.Steps.FirstOrDefault(s => s.Id == id);
            step.DesiredResults = _db.DesiredResultStep.OrderBy(d => d.Id)
                .Where(d => d.StepId == id)
                .Select(s => s.DesiredResult).Where(s => s.Active).ToList();
            if (back != null)
                step.Baсk = back;
            else
            {
              step.Baсk = "Личный кабинет";
            }
            return View(step);
        }

        [HttpGet]
        public IActionResult ViewStep(int id)
        {
            List<Step> steps = _db.IssueStep.OrderBy(i => i.Id).Where(i => i.IssueId == id).Select(s => s.Step)
                .ToList();
            return PartialView("PartialViews/StepsForIssuePartialView", steps);
        }
    }
}