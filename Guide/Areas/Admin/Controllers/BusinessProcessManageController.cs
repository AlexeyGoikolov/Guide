using System.Collections.Generic;
using System.Linq;
using Guide.Models;
using Guide.Models.Data;
using Guide.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Guide.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class BusinessProcessManageController : Controller
    {
        private readonly GuideContext _db;

        public BusinessProcessManageController(GuideContext db)
        {
            _db = db;
        }

        
        public IActionResult Index()
        {
            List<BusinessProcess> businessProcesses = _db.BusinessProcesses.ToList();
            return View(businessProcesses);
        }

        public IActionResult Create()
        {
            return View(new BusinessProcess());
        }
        
        [HttpPost]
        public IActionResult Create(BusinessProcess model)
        {
            if (ModelState.IsValid)
            {
                _db.BusinessProcesses.Add(model);
                _db.SaveChanges();
                BusinessProcess business = _db.BusinessProcesses.FirstOrDefault(b => b.Name == model.Name 
                                                                                     && b.Description == model.Description);
                return RedirectToAction("AddIssues", "BusinessProcessManage", new {id = business.Id});
            }

            return View(model);
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

        public IActionResult Edit(int id)
        {
            if (id != 0)
            {
                BusinessProcess process = _db.BusinessProcesses.FirstOrDefault(b => b.Id == id);
                if (process != null)
                    return View(process);
            }
            return NotFound();
        }

        public IActionResult AddIssues(int id)
        {
            if (id != 0)
            {
                BusinessProcessIssuesViewModel model = new BusinessProcessIssuesViewModel
                {
                    BusinessProcess = _db.BusinessProcesses.FirstOrDefault(b => b.Id == id),
                    AllIssue = _db.Issues.OrderByDescending(i => i.CreatedAt).ToList(),
                    DesignatedIssues = _db.BusinessProcessIssues.OrderBy(b => b.Id)
                        .Where(b => b.BusinessProcessId == id).Select(b => b.Issue)
                        .ToList()
                };
                return View(model);
            }

            return NotFound();
        }

        public IActionResult AddBusinessProcessIssuesConnection(int businessId, int[] issuesId)
        {
            if (businessId != 0)
            {
                var deleteBusinessProcessIssues =
                    from business in _db.BusinessProcessIssues
                    where business.BusinessProcessId == businessId
                    select business;

                foreach (var business in deleteBusinessProcessIssues)
                {
                    _db.BusinessProcessIssues.Remove(business);
                }

                foreach (var issue in issuesId)
                {
                    BusinessProcessIssue model = new BusinessProcessIssue
                    {
                        BusinessProcessId = businessId,
                        IssueId = issue
                    };
                    _db.BusinessProcessIssues.Add(model);
                }

                _db.SaveChanges();
            }
            return RedirectToAction("Details", "BusinessProcessManage", new {area = "Admin", id = businessId});
        }
    }
}