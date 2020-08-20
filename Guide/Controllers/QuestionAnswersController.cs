using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guide.Models;
using Guide.Models.Data;
using Guide.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Guide.Controllers
{
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
            model.PostId = postId;
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
                    questionAnswer.PostId= model.PostId;
                    questionAnswer.ResponderId= _userManager.GetUserId(User);
                    
                    _db.QuestionAnswers.Update(questionAnswer);
                }
                else
                {
                    model.QuestionAnswer.State = State.ответили;
                    questionAnswer.Question = model.QuestionAnswer.Question;
                    questionAnswer.Answer = model.QuestionAnswer.Answer;
                    questionAnswer.PostId= model.PostId;
                    questionAnswer.AskingId=  _userManager.GetUserId(User);
                    questionAnswer.State= model.QuestionAnswer.State;
                    
                    _db.QuestionAnswers.Add(questionAnswer);
                }
                _db.SaveChanges();
            }

            if (model.Status == QuestionAnswersStatus.ToAnswer)
                return RedirectToAction("IndexQuestions", "QuestionAnswers");
            return RedirectToAction("Index", "QuestionAnswers");
        }

        public IActionResult Preview(int id)
        {
            QuestionAnswer questionAnswer = _db.QuestionAnswers.FirstOrDefault(q => q.Id == id);
           
            return View(questionAnswer);
        }
        
       
        
        
    }
}