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
    public class DesiredResultController : Controller
    {
        private readonly GuideContext _db;

        public DesiredResultController(GuideContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<DesiredResult> desiredResults = _db.DesiredResults.Where(d => d.Active).ToList();
            return View(desiredResults);
        }

        // GET
        public IActionResult Create(int issuesId, int stepId)
        {
            CreateDesiredResultViewModel model = new CreateDesiredResultViewModel()
            {
                Results = _db.DesiredResults.Where(d => d.Active).ToList(),
                IssueId = issuesId,
                StepId = stepId
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateDesiredResultViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.IssueId != 0)
                {
                    if (model.DesiredResultId.Count != 0)
                    {
                        foreach (var result in model.DesiredResultId)
                        {
                            NewDesiredResultIssue(model.IssueId, result);
                        }
                    }

                    if (model.DesResult.Name != null)
                    {
                        DesiredResult desiredResult = NewDesiredResult(model.DesResult.Number, model.DesResult.Name);
                        NewDesiredResultIssue(model.IssueId, desiredResult.Id);
                    }

                    return RedirectToAction("Details", "IssuesManage", new {id = model.IssueId});
                }

                if (model.StepId != 0)
                {
                    if (model.DesiredResultId.Count != 0)
                    {
                        foreach (var result in model.DesiredResultId)
                        {
                            NewDesiredResultStep(model.IssueId, result);
                        }
                    }

                    if (model.DesResult.Name != null)
                    {
                        DesiredResult desiredResult = NewDesiredResult(model.DesResult.Number, model.DesResult.Name);
                        NewDesiredResultStep(model.StepId, desiredResult.Id);
                    }

                    return RedirectToAction("Details", "StepsManage", new {id = model.StepId});
                }

                if (model.StepId == 0 && model.StepId == 0)
                {
                    if (model.DesResult.Name != null)
                        NewDesiredResult(model.DesResult.Number, model.DesResult.Name);
                    return RedirectToAction("Create");
                }
            }

            return NotFound();
        }

        public DesiredResult NewDesiredResult(int number, string name)
        {
            DesiredResult desiredResult = new DesiredResult()
            {
                Number = number,
                Name = name
            };
            _db.DesiredResults.Add(desiredResult);
            _db.SaveChanges();
            return (desiredResult);
        }

        public void NewDesiredResultStep(int stepId, int desiredResultId)
        {
            DesiredResultStep desiredResultStep = new DesiredResultStep()
            {
                StepId = stepId,
                DesiredResultId = desiredResultId
            };
            _db.DesiredResultStep.Add(desiredResultStep);
            _db.SaveChanges();
        }

        public void NewDesiredResultIssue(int issueId, int desiredResultId)
        {
            DesiredResultIssue desiredResultIssue = new DesiredResultIssue()
            {
                IssueId = issueId,
                DesiredResultId = desiredResultId
            };
            _db.DesiredResultIssue.Add(desiredResultIssue);
            _db.SaveChanges();
        }

        public IActionResult Delete(int id)
        {
            if (id != 0)
            {
                DesiredResult desiredResult = _db.DesiredResults.FirstOrDefault(s => s.Id == id);
                if (desiredResult != null)
                    return View(desiredResult);
            }

            return NotFound();
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)
        {
            if (id != 0)
            {
                DesiredResult desiredResult = _db.DesiredResults.FirstOrDefault(s => s.Id == id);
                if (desiredResult != null)
                {
                    desiredResult.Active = false;
                    _db.SaveChanges();
                }
            }

            return RedirectToAction("Index", "DesiredResult");
        }

        public IActionResult Edit(int issuesid)
        {
            List<DesiredResult> desiredResults = _db.DesiredResultIssue.OrderBy(d => d.Id)
                .Where(d => d.IssueId == issuesid)
                .Select(s => s.DesiredResult).ToList();
            return View(desiredResults);
        }

        public IActionResult DeleteFromIssue(int issueid, int desiredResultId)
        {
            DesiredResultIssue desiredResult =
                _db.DesiredResultIssue.FirstOrDefault(d =>
                    d.IssueId == issueid && d.DesiredResultId == desiredResultId);
            _db.DesiredResultIssue.Remove(desiredResult);
            _db.SaveChanges();
            return RedirectToAction("Details", "Issues", new {id = issueid});
        }
    }
}