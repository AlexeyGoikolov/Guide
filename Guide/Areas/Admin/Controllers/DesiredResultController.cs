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
            List< DesiredResult> desiredResults = _db.DesiredResults.Where(d=>d.Active).ToList();
            return View(desiredResults);
        }
        // GET
        public IActionResult Create(int issuesId, int stepId)
        {
            CreateDesiredResultViewModel model = new CreateDesiredResultViewModel();

           model.Results = _db.DesiredResults.Where(d=>d.Active).ToList();
           model.IssueId = issuesId;
           model.StepId = stepId;
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateDesiredResultViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.IssueId != 0) //добавляем данные в таблицу связи Задачи и ЖР , если выбрали из списка
                {
                    if (model.DesiredResultId.Count != 0)
                    {
                        foreach (var result in model.DesiredResultId)
                        {
                            DesiredResultIssue desiredResultIssue = new DesiredResultIssue();
                            desiredResultIssue.IssueId = model.IssueId;
                            desiredResultIssue.DesiredResultId = result;
                            _db.DesiredResultIssue.Add(desiredResultIssue);
                            _db.SaveChanges();
                        }
                    }
                    
                        
                        if ( model.DesResult.Name != null)//создает новый ЖР
                        {
                            DesiredResult desiredResult  =new DesiredResult();
                            desiredResult.Number = model.DesResult.Number;
                            desiredResult.Name = model.DesResult.Name;
                            _db.DesiredResults.Add(desiredResult);
                            _db.SaveChanges();
                            // теперь в общую таблицу добавляем новый ЖР в связи с Задачей
                            DesiredResultIssue desiredResultIssue = new DesiredResultIssue();
                            desiredResultIssue.IssueId = model.IssueId;
                            desiredResultIssue.DesiredResultId = desiredResult.Id;
                            _db.DesiredResultIssue.Add(desiredResultIssue);
                            _db.SaveChanges();
                        }
                    

                    return RedirectToAction("Details", "IssuesManage", new {id = model.IssueId});
                }

                if (model.StepId != 0) //добавляем данные в таблицу связи Шаг и ЖР , если выбрали из списка
                {
                    if (model.DesiredResultId.Count != 0)
                    {
                        foreach (var result in model.DesiredResultId)
                        {
                            DesiredResultStep desiredResultStep = new DesiredResultStep();
                            desiredResultStep.StepId = model.StepId;
                            desiredResultStep.DesiredResultId = result;
                            _db.DesiredResultStep.Add(desiredResultStep);
                            _db.SaveChanges();
                        }


                        //создает новый ЖР
                        if (model.DesResult.Name != null)
                        {
                            DesiredResult desiredResult = new DesiredResult();
                            desiredResult.Number = model.DesResult.Number;
                            desiredResult.Name = model.DesResult.Name;
                            _db.DesiredResults.Add(desiredResult);
                            _db.SaveChanges();
                            // теперь в общую таблицу добавляем новый ЖР в связи с Шагом
                            DesiredResultStep desiredResultStep = new DesiredResultStep();
                            desiredResultStep.StepId = model.StepId;
                            desiredResultStep.DesiredResultId = desiredResult.Id;
                            _db.DesiredResultStep.Add(desiredResultStep);
                            _db.SaveChanges();
                        }
                    }

                    return RedirectToAction("Details", "StepsManage", new {id = model.StepId});
                }

                if (model.StepId == 0 && model.StepId == 0)
                {
                    if (model.DesResult.Name != null)
                    {
                        DesiredResult desiredResult = new DesiredResult();
                        desiredResult.Number = model.DesResult.Number;
                        desiredResult.Name = model.DesResult.Name;
                        _db.DesiredResults.Add(desiredResult);
                        _db.SaveChanges();
                    }

                    return RedirectToAction("Create");
                }// просто новый ЖР
            }

            return NotFound();
        }

        public IActionResult Delete(int id)
        {
            if (id != 0)
            {
                DesiredResult desiredResult = _db.DesiredResults.FirstOrDefault(s => s.Id == id);
                if (desiredResult != null)
                {
                    return View(desiredResult);
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
            List<DesiredResult> desiredResults = _db.DesiredResultIssue.OrderBy(d => d.Id).
                Where(d => d.IssueId == issuesid)
                .Select(s => s.DesiredResult).ToList();
            return View(desiredResults);
        }

        public IActionResult DeleteFromIssue(int issueid, int drId)
        {

            DesiredResultIssue d = _db.DesiredResultIssue.FirstOrDefault(d => d.IssueId == issueid && d.DesiredResultId == drId);
            _db.DesiredResultIssue.Remove(d);
            _db.SaveChanges();
            return RedirectToAction("Details", "Issues", new {id = issueid});
        }
    }
}