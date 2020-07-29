using System.Collections.Generic;
using System.Linq;
using Guide.Models;
using Guide.Models.Data;
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
            List<QuestionAnswer> questionAnswer = _db.QuestionAnswers.ToList();
            return View();
        }
    }
}