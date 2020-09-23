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
        public IActionResult Create(BusinessProcess model, int choice)
        {
            if (ModelState.IsValid)
            {
                _db.BusinessProcesses.Add(model);
                _db.SaveChanges();

                BusinessProcess business = _db.BusinessProcesses.FirstOrDefault(b => b.Name == model.Name 
                                                                                     && b.Description == model.Description);
                if (choice == 1)
                    return RedirectToAction("Details", "BusinessProcessManage", new {id = business.Id});
                return RedirectToAction("AddIssues", "BusinessProcessManage", new {id = business.Id, type = "create"});
            }

            return View(model);
        }

        public IActionResult Details(int id, string back)
        {
            BusinessProcessIssuesViewModel model = new BusinessProcessIssuesViewModel()
            {
                Back = back,
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
        
        [HttpPost]
        public IActionResult Edit(BusinessProcess model, int choice)
        {
            if (ModelState.IsValid)
            {
                _db.BusinessProcesses.Update(model);
                _db.SaveChanges();
                if (choice == 1)
                    return RedirectToAction("Details", "BusinessProcessManage", new {id=model.Id});
                return RedirectToAction("AddIssues", "BusinessProcessManage", new {id = model.Id});;
            }

            return View(model);
        }

        public IActionResult AddIssues(int id, string type)
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
                if (type == "create")
                    model.Action = "create";
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
        
        public IActionResult AjaxIssueSearch(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                List<Issue> model = _db.Issues.OrderByDescending(i => i.CreatedAt).ToList();
                return PartialView("PartialViews/FilterIssuesPartial", model);
            }
            
            string filterWord = word.ToUpper();
            var issues = _db.Issues.Where(i => 
                i.Name.ToUpper().Contains(filterWord)).OrderByDescending(i => i.CreatedAt).ToList();
            if (issues.Count > 0)
            {
                return PartialView("PartialViews/FilterIssuesPartial", issues);
            }
            return Json(false);
        }

        public IActionResult Delete(int id)
        {
            if (id != 0)
            {
                BusinessProcess businessProcess = _db.BusinessProcesses.FirstOrDefault(b => b.Id == id);
                if (businessProcess != null)
                {
                    return View(businessProcess);
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
                BusinessProcess businessProcess = _db.BusinessProcesses.FirstOrDefault(b => b.Id == id);
                if (businessProcess != null)
                {
                    _db.BusinessProcesses.Remove(businessProcess);
                    _db.SaveChanges();
                }
            }

            return RedirectToAction("Index", "BusinessProcessManage");
        }
    }
}