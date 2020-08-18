using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            List<QuestionAnswer> questionAnswer = _db.QuestionAnswers
                .Where(q => q.Answer != null).ToList();
            return View(questionAnswer);
        }
        
        public IActionResult Preview(int id)
        {
            QuestionAnswer questionAnswer = _db.QuestionAnswers.FirstOrDefault(q => q.Id == id);
            return View(questionAnswer);
        }

        [HttpPost]
        public IActionResult Create(QuestionAnswer model)
        {
            if (ModelState.IsValid)
            {
                _db.QuestionAnswers.Add(model);
                _db.SaveChanges();
            }

            return RedirectToAction("Details", "Account");
        }

        public async Task<IActionResult> AddQuestionAjax()
        {
            QuestionAnswer model = new QuestionAnswer();
            return PartialView("PartialViews/AddQuestion", model);
        }
        
    }
}