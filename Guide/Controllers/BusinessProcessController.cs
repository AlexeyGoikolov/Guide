using System.Collections.Generic;
using System.Linq;
using Guide.Models;
using Guide.Models.Data;
using Guide.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Guide.Controllers
{
    public class BusinessProcessController : Controller
    {
        // GET
        private readonly GuideContext _db;

        public BusinessProcessController(GuideContext db)
        {
            _db = db;
        }

        public IActionResult Details(int id)
        {
            BusinessProcessIssuesViewModel model = new BusinessProcessIssuesViewModel()
            {
                BusinessProcess = _db.BusinessProcesses.FirstOrDefault(i => i.Id == id),
                DesignatedIssues = _db.BusinessProcessIssues.OrderBy(b => b.Id).Where(b => b.BusinessProcessId == id).
                    Select(i => i.Issue).ToList(),
                AllIssue = new List<Issue>(),
            };
            return View(model);
        }
    }
}