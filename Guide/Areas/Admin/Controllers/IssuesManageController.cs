using System;
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
            foreach (var issue in issues)
            {
                issue.BP = _db.BusinessProcessIssues.OrderBy(b => b.Id)
                    .Where(b => b.IssueId == issue.Id).Select(b => b.BusinessProcess).ToList();
            }

            return View(issues);
        }

        public IActionResult IndexPosition(int id)
        {
            PositionIssueViewModel model = new PositionIssueViewModel()
            {
                Issues = _db.PositionIssues.OrderBy(d => d.Id)
                    .Where(d => d.PositionId == id).Select(s => s.Issue).ToList(),
                PositionId = id,
                Position = _db.Positions.FirstOrDefault(p => p.Id == id)
            };
            return View(model);
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
                if (choice == 1)
                    return RedirectToAction("AddSteps", "IssuesManage", new {id = issue1.Id, type = "create"});
                if (choice == 2)
                    return RedirectToAction("Create", "DesiredResult", new {issuesId = issue.Id});
            }

            return View(issue);
        }

        public IActionResult AddSteps(int id, string type)
        {
            if (id != 0)
            {
                IssueStepsViewModel model = new IssueStepsViewModel
                {
                    Issue = _db.Issues.FirstOrDefault(i => i.Id == id),
                    AllSteps = _db.Steps.OrderByDescending(s => s.CreatedAt).ToList(),
                    DesignatedSteps = _db.IssueStep.OrderBy(i => i.Id).Where(i => i.IssueId == id).Select(s => s.Step)
                        .ToList()
                };
                if (type == "create")
                    model.Action = "create";
                return View(model);
            }

            return NotFound();
        }

        public IActionResult Details(int id, string back)
        {
            IssueStepsViewModel model = new IssueStepsViewModel
            {
                Back = back,
                Issue = _db.Issues.FirstOrDefault(i => i.Id == id),
                DesignatedSteps = _db.IssueStep.OrderBy(i => i.Id).Where(i => i.IssueId == id).Select(s => s.Step)
                    .ToList(),
                AllSteps = new List<Step>(),
                DesiredResults = _db.DesiredResultIssue.OrderBy(d => d.Id).Where(d => d.IssueId == id)
                    .Select(s => s.DesiredResult).Where(s => s.Active).ToList()
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
                IssueStepsViewModel model = new IssueStepsViewModel
                {
                    Issue = _db.Issues.FirstOrDefault(i => i.Id == id),
                    DesignatedSteps = _db.IssueStep.OrderBy(i => i.Id)
                        .Where(i => i.IssueId == id).Select(s => s.Step)
                        .ToList(),
                    AllSteps = new List<Step>(),
                    DesiredResults = _db.DesiredResultIssue.OrderBy(d => d.Id)
                        .Where(d => d.IssueId == id)
                        .Select(s => s.DesiredResult).Where(s => s.Active).ToList()
                };
                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(IssueStepsViewModel model, int choice)
        {
            if (ModelState.IsValid)
            {
                Issue issue = _db.Issues.FirstOrDefault(i => i.Id == model.Issue.Id);
                issue.Name = model.Issue.Name;
                issue.IssueDescription = model.Issue.IssueDescription;
                _db.Issues.Update(issue);
                _db.SaveChanges();
                if (choice == 2)
                    return RedirectToAction("AddSteps", "IssuesManage", new {id = issue.Id});
                if (choice == 3)
                    return RedirectToAction("Create", "DesiredResult", new {issuesId = issue.Id});
            }

            return RedirectToAction("Details", "IssuesManage", new {id = model.Issue.Id});
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

        public IActionResult Delete(int id)
        {
            if (id != 0)
            {
                Issue issue = _db.Issues.FirstOrDefault(i => i.Id == id);
                if (issue != null)
                    return View(issue);
            }

            return NotFound();
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)
        {
            if (id != 0)
            {
                Issue issue = _db.Issues.FirstOrDefault(i => i.Id == id);
                if (issue != null)
                {
                    _db.Issues.Remove(issue);
                    _db.SaveChanges();
                }
            }

            return RedirectToAction("Index", "IssuesManage");
        }

        public IActionResult AddForUser(string id)
        {
            AddForUserViewModel model = new AddForUserViewModel();
            model.UserId = id;
            model.Issues = _db.Issues.ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult AddForUser(AddForUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var issueId in model.IssuesId)
                {
                    UserIssue userIssue = new UserIssue();
                    userIssue.UserId = model.UserId;
                    userIssue.IssueId = issueId;
                    _db.UserIssues.Add(userIssue);
                    _db.SaveChanges();
                }
            }

            return RedirectToAction("ListUsers", "UsersManage");
        }

        public IActionResult AddForPosition()
        {
            AddForPositionViewModel model = new AddForPositionViewModel();
            model.Issues = _db.Issues.ToList();
            model.Positions = _db.Positions.Where(p => p.Active).ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult AddForPosition(AddForPositionViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.PositionId != 0 && model.IssuesId == null)
                    return RedirectToAction("IndexPosition", "IssuesManage", new {id = model.PositionId});
                if (model.IssuesId == null)
                    return RedirectToAction("ListUsers", "UsersManage");
                foreach (var issueId in model.IssuesId)
                {
                    PositionIssue positionIssue = new PositionIssue();
                    positionIssue.IssueId = issueId;
                    positionIssue.PositionId = model.PositionId;
                    _db.PositionIssues.Add(positionIssue);
                    _db.SaveChanges();
                }
            }

            return RedirectToAction("IndexPosition", "IssuesManage", new {id = model.PositionId});
        }

        public IActionResult CreatePositionAjax(Position position, AddForPositionViewModel data)
        {
            if (position.Name != null)
            {
                _db.Positions.Add(position);
                _db.SaveChanges();
            }

            AddForPositionViewModel model = new AddForPositionViewModel()
            {
                Positions = _db.Positions.Where(p => p.Active).ToList()
            };

            return PartialView("PartialViews/PosiitionPartial", model);
        }

        public IActionResult DeletePositionAjax(int id)
        {
            Position position = _db.Positions.FirstOrDefault(p => p.Id == id);
            if (position != null)
            {
                position.Active = false;
                _db.SaveChanges();
            }

            AddForPositionViewModel model = new AddForPositionViewModel()
            {
                Positions = _db.Positions.Where(p => p.Active).ToList()
            };
            return PartialView("PartialViews/PosiitionPartial", model);
        }

        public IActionResult DeleteStepLink(int issueId, int stepId)
        {
            if (issueId != 0 && stepId != 0)
            {
                IssueStep model = _db.IssueStep.Where(i => i.IssueId == issueId)
                    .FirstOrDefault(i => i.StepId == stepId);
                if (model != null)
                {
                    _db.IssueStep.Remove(model);
                    _db.SaveChanges();
                    return Json("true");
                }
            }

            return Json("false");
        }

        public IActionResult DeleteDesireResult(int issueId, int desResultId)
        {
            if (issueId != 0 && desResultId != 0)
            {
                DesiredResultIssue model = _db.DesiredResultIssue.Where(i => i.IssueId == issueId)
                    .FirstOrDefault(i => i.DesiredResultId == desResultId);
                if (model != null)
                {
                    _db.DesiredResultIssue.Remove(model);
                    _db.SaveChanges();
                    return Json("true");
                }
            }

            return Json("false");
        }

        public IActionResult DeleteIssuePosition(int issueId, int positionId)
        {
            if (issueId != 0 && positionId != 0)
            {
                PositionIssue model = _db.PositionIssues.Where(i => i.IssueId == issueId)
                    .FirstOrDefault(i => i.PositionId == positionId);
                if (model != null)
                {
                    _db.PositionIssues.Remove(model);
                    _db.SaveChanges();
                    return Json("true");
                }
            }

            return Json("false");
        }

        public IActionResult DeleteIssueUser(int issueId, string userId)
        {
            if (issueId != 0 && userId != null)
            {
                UserIssue model = _db.UserIssues.Where(i => i.IssueId == issueId)
                    .FirstOrDefault(i => i.UserId == userId);
                if (model != null)
                {
                    _db.UserIssues.Remove(model);
                    _db.SaveChanges();
                    return Json("true");
                }
            }

            return Json("false");
        }
    }
}