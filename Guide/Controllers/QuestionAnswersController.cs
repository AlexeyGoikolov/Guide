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
        
        public async Task<IActionResult> Create(string question)
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
        
        public async Task<IActionResult> AddQuestionAjax()
        {
            QuestionAnswer model = new QuestionAnswer();
            return PartialView("PartialViews/AddQuestion", model);
        }
    }
}