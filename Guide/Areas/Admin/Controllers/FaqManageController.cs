using System.Collections.Generic;
using System.Linq;
using Guide.Models;
using Guide.Models.Data;
using Guide.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Guide.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class FaqManageController : Controller
    {
        private readonly GuideContext _db;
        private readonly UserManager<User> _userManager;

        public FaqManageController(GuideContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            List<QuestionAnswer> questionAnswer = _db.QuestionAnswers
                .Where(q => q.Answer != null).ToList();
            return View(questionAnswer);
        }

        public IActionResult IndexQuestions()
        {
            List<QuestionAnswer> questionAnswer = _db.QuestionAnswers
                .Where(q => q.Answer == null).ToList();
            return View(questionAnswer);
        }

        public IActionResult Create(QuestionAnswersStatus status, int id, int? postId)
        {
            QuestionAnswersViewModel model = new QuestionAnswersViewModel();
            if (id != 0)
                model.QuestionAnswer = _db.QuestionAnswers.FirstOrDefault(q => q.Id == id);
            else
                model.QuestionAnswer = new QuestionAnswer();
            model.Status = status;
            model.SourceId = postId;
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(QuestionAnswersViewModel model)
        {
            if (ModelState.IsValid)
            {
                QuestionAnswer questionAnswer = new QuestionAnswer();
                if (model.Status == QuestionAnswersStatus.Edit || model.Status == QuestionAnswersStatus.ToAnswer)
                {
                    questionAnswer = _db.QuestionAnswers.FirstOrDefault(q => q.Id == model.QuestionAnswer.Id);
                    questionAnswer.Question = model.QuestionAnswer.Question;
                    questionAnswer.Answer = model.QuestionAnswer.Answer;
                    questionAnswer.SourceId = model.SourceId;
                    questionAnswer.ResponderId = _userManager.GetUserId(User);

                    _db.QuestionAnswers.Update(questionAnswer);
                }
                else
                {
                    model.QuestionAnswer.State = State.Ответили;
                    questionAnswer.Question = model.QuestionAnswer.Question;
                    questionAnswer.Answer = model.QuestionAnswer.Answer;
                    questionAnswer.SourceId = model.SourceId;
                    questionAnswer.ResponderId = _userManager.GetUserId(User);
                    questionAnswer.State = model.QuestionAnswer.State;

                    _db.QuestionAnswers.Add(questionAnswer);
                }

                _db.SaveChanges();
            }

            if (model.Status == QuestionAnswersStatus.ToAnswer)
                return RedirectToAction("IndexQuestions", "FaqManage");
            return RedirectToAction("Index", "FaqManage");
        }

        public IActionResult Preview(int id)
        {
            QuestionAnswer questionAnswer = _db.QuestionAnswers.FirstOrDefault(q => q.Id == id);
            return View(questionAnswer);
        }

        public IActionResult Delete(QuestionAnswersStatus status, int id)
        {
            if (id != 0)
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
        public IActionResult ConfirmDelete(QuestionAnswersViewModel model)
        {
            if (model.QuestionAnswer.Id != 0)
            {
                QuestionAnswer questionAnswer =
                    _db.QuestionAnswers.FirstOrDefault(v => v.Id == model.QuestionAnswer.Id);
                _db.QuestionAnswers.Remove(questionAnswer);
                _db.SaveChanges();
                if (model.Status == QuestionAnswersStatus.Edit)
                    return RedirectToAction("Index", "FaqManage");
                return RedirectToAction("IndexQuestions", "FaqManage");
            }

            return NotFound();
        }
    }
}