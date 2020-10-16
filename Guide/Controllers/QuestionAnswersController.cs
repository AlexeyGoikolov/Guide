using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guide.Models;
using Guide.Models.Data;
using Guide.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Guide.Controllers
{
    [Authorize]
    public class QuestionAnswersController : Controller
    {
        private readonly GuideContext _db;
        private readonly UserManager<User> _userManager;

        public QuestionAnswersController(GuideContext db, UserManager<User> userManager)
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
        
        public IActionResult Create(QuestionAnswersStatus status, int id, int? postId)
        {
            QuestionAnswersViewModel model = new QuestionAnswersViewModel();
            if (id != 0)
                model.QuestionAnswer = _db.QuestionAnswers.FirstOrDefault(q => q.Id == id);
            else
                model.QuestionAnswer = new QuestionAnswer();
            model.Status = status;
            model.StepId = postId;
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(QuestionAnswersViewModel model)
        {
            if (ModelState.IsValid)
            {
                QuestionAnswer questionAnswer = new QuestionAnswer();
                questionAnswer.Question = model.QuestionAnswer.Question;
                questionAnswer.Answer = model.QuestionAnswer.Answer;
                questionAnswer.StepId = model.StepId;
                questionAnswer.AskingId = _userManager.GetUserId(User);
                questionAnswer.State = model.QuestionAnswer.State;

                _db.QuestionAnswers.Add(questionAnswer);

                _db.SaveChanges();
            }
            
            return RedirectToAction("Index", "QuestionAnswers");
        }

        public async Task<IActionResult> CreateAjax(string question)
        {
            if (question != null)
            {
                QuestionAnswer model = new QuestionAnswer
                {
                    Question = question,
                    AskingId = _userManager.GetUserId(User),
                };
                await _db.QuestionAnswers.AddAsync(model);
                _db.SaveChanges();
                return Json(true);
            }

            return Json(false);
        }

        public IActionResult Preview(int id)
        {
            QuestionAnswer questionAnswer = _db.QuestionAnswers.FirstOrDefault(q => q.Id == id);

            return View(questionAnswer);
        }

        public IActionResult AddQuestionAjax()
        {
            QuestionAnswer model = new QuestionAnswer();
            return PartialView("PartialViews/AddQuestion", model);
        }
    }
}