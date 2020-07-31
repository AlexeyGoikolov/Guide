using System.Collections.Generic;
using System.Linq;
using Guide.Models;
using Guide.Models.Data;
using Guide.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Guide.Controllers
{
    public class QuestionAnswersController : Controller
    {
        private readonly GuideContext _db;
  public QuestionAnswersController(GuideContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
           
            List<QuestionAnswer> questionAnswer = new List<QuestionAnswer>();
            questionAnswer = _db.QuestionAnswers
                    .Where(q => q.Answer != null).ToList();
          return View(questionAnswer);
        }

        public IActionResult IndexQuestions()
        {
            List<QuestionAnswer> questionAnswer = new List<QuestionAnswer>();
            questionAnswer = _db.QuestionAnswers
                .Where(q => q.Answer == null).ToList();
            return View(questionAnswer);
        }

        public IActionResult Preview(string id)
        {
            QuestionAnswer questionAnswer = _db.QuestionAnswers.FirstOrDefault(q => q.Id == id);
            return View(questionAnswer);
        }
        public IActionResult Create(QuestionAnswersStatus status, string id)
        {
            QuestionAnswersViewModel model = new QuestionAnswersViewModel();
            if (id != null)
                model.QuestionAnswer = _db.QuestionAnswers.FirstOrDefault(q => q.Id == id);
            model.Status = status;
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(QuestionAnswersViewModel model)
        {
            if (ModelState.IsValid)
            {
                QuestionAnswer questionAnswer = new QuestionAnswer();
                if (model.Status != QuestionAnswersStatus.Creat)
                {
                    questionAnswer = _db.QuestionAnswers.FirstOrDefault(q => q.Id == model.QuestionAnswer.Id);
                    questionAnswer.Question = model.QuestionAnswer.Question;
                    questionAnswer.Answer = model.QuestionAnswer.Answer;
                    _db.QuestionAnswers.Update(questionAnswer);
                    
                }
                else
                {
                    questionAnswer.Id = model.QuestionAnswer.Id;
                    questionAnswer.Question = model.QuestionAnswer.Question;
                    questionAnswer.Answer = model.QuestionAnswer.Answer;
                    _db.QuestionAnswers.Add(questionAnswer);
                }
                _db.SaveChanges();
            }

            if (model.Status == QuestionAnswersStatus.ToAnswer)
                return RedirectToAction("IndexQuestions", "QuestionAnswers");
            return RedirectToAction("Index", "QuestionAnswers");
        }
        public IActionResult Delete(QuestionAnswersStatus status, string id)
        {
            if (id != null)
            {
                QuestionAnswersViewModel model = new QuestionAnswersViewModel();
                model.QuestionAnswer = _db.QuestionAnswers.FirstOrDefault(v => v.Id == id);
                model.Status = status;
                
                if (model.QuestionAnswer != null)
                {
                    return View(model);
                }
            }
            return NotFound();
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult ConfirmDeleta(QuestionAnswersViewModel model)
        {
            if (model.QuestionAnswer.Id != null)
            {
                QuestionAnswer questionAnswer = _db.QuestionAnswers.FirstOrDefault(v => v.Id == model.QuestionAnswer.Id);
                _db.QuestionAnswers.Remove(questionAnswer);
                    _db.SaveChanges();
                    if (model.Status == QuestionAnswersStatus.Edit)
                        return RedirectToAction("Index" , "QuestionAnswers" );
                    return RedirectToAction("IndexQuestions" , "QuestionAnswers" );
            }
            return NotFound();
        }
    }
}