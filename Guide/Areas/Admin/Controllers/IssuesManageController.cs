using System;
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
    public class IssuesManageController : Controller
    {
        private readonly GuideContext _db;

        public IssuesManageController(GuideContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Issue> issues = _db.Issues.ToList();
            return View(issues);
        }

        public IActionResult Create()
        {
            return View(new Issue());
        }
        
        [HttpPost]
        public IActionResult Create(Issue issue, int choice)
        {
            if (ModelState.IsValid)
            {
                _db.Issues.Add(issue);
                _db.SaveChanges();
                 Issue issue1 = _db.Issues.FirstOrDefault(i => i.Name == issue.Name 
                                                               && i.IssueDescription == issue.IssueDescription);
                     //переходит на добавление шага
                if (choice == 2)
                    return RedirectToAction("AddSteps", "IssuesManage", new {id = issue1.Id});
                        // переходит на добавление ЖР
                if (choice==3)
                       return RedirectToAction("Create", "DesiredResult", new {issuesId = issue.Id});
            }

            return View(issue);
        }

        public IActionResult AddSteps(int id)
        {
            if (id != 0)
            {
                
                IssueStepsViewModel model = new IssueStepsViewModel
                {
                    Issue = _db.Issues.FirstOrDefault(i => i.Id == id),
                    AllSteps = _db.Steps.OrderByDescending(s => s.CreatedAt).ToList(),
                    DesignatedSteps = _db.IssueStep.OrderBy(i => i.Id).Where(i => i.IssueId == id).Select(s => s.Step).ToList()
                };
                return View(model);
            }

            return NotFound();
        }
        
        public IActionResult Details(int id)
        {
            IssueStepsViewModel model = new IssueStepsViewModel
            {
                Issue = _db.Issues.FirstOrDefault(i => i.Id == id),
                DesignatedSteps = _db.IssueStep.OrderBy(i => i.Id).Where(i => i.IssueId == id).
                    Select(s => s.Step).ToList(),
                AllSteps = new List<Step>(),
                DesiredResults = _db.DesiredResultIssue.OrderBy(d=>d.Id).Where(d=>d.IssueId==id).
                    Select(s=>s.DesiredResult).ToList()
            };
            return View(model);
        }

        public IActionResult AjaxStepSearch(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                List<Step> model = _db.Steps.OrderByDescending(s => s.CreatedAt).ToList();
                return PartialView("PartialViews/FilterStepsPartial", model);
            }
            
            string filterWord = word.ToUpper();
            var steps = _db.Steps.Where(s => 
                s.Name.ToUpper().Contains(filterWord)).OrderByDescending(s => s.CreatedAt).ToList();
            if (steps.Count > 0)
            {
                return PartialView("PartialViews/FilterStepsPartial", steps);
            }
            return Json(false);
        }


        public IActionResult Edit(int id)
        {
            if (id != 0)
            {
                Issue issue = _db.Issues.FirstOrDefault(s => s.Id == id);
                if (issue != null)
                    return View(issue);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Issue issue)
        {
            if (ModelState.IsValid)
            {
                _db.Issues.Update(issue);
                _db.SaveChanges();
                return RedirectToAction("AddSteps", "IssuesManage", new {id = issue.Id});;
            }

            return View(issue);
        }
        
        
        public IActionResult AddIssueStepConnection(int issueId, int[] stepsId)
        {
            if (issueId != 0)
            {
                var deleteIssueStep =
                    from issue in _db.IssueStep
                    where issue.IssueId == issueId
                    select issue;
                
                foreach (var issue in deleteIssueStep)
                {
                    _db.IssueStep.Remove(issue);
                }

                foreach (var step in stepsId)
                {
                    IssueStep model = new IssueStep
                    {
                        IssueId = issueId,
                        StepId = Convert.ToInt32(step)
                    };
                    _db.IssueStep.Add(model);
                }
                _db.SaveChanges();
            }

            return RedirectToAction("Details", "IssuesManage", new {area = "Admin", id = issueId});
        }
    }
}