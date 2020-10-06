using System.Collections.Generic;
using System.Linq;
using Guide.Models;
using Guide.Models.Data;
using Guide.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Guide.Controllers
{
    public class SearchController : Controller
    {
        private readonly GuideContext _db;

        public SearchController(GuideContext db)
        {
            _db = db;
        }

        // GET
        public IActionResult SearchIndex(string search)
        {
            if (search != null)
            {
                string a = "%" + search.ToLower() + "%";
                List<Interpretation> interpretations =
                    new List<Interpretation>(_db.Interpretations.Where(s =>
                        EF.Functions.Like(s.Description.ToLower(), a)));
                List<Glossary> glossaries = new List<Glossary>(_db.Glossaries.Where(g => EF.Functions.Like
                    (g.Name.ToLower(), a)));
                foreach (var interpretation in interpretations)
                {
                    Glossary glossary = _db.Glossaries.FirstOrDefault(g => g.Id == interpretation.GlossaryId);
                    if (glossary != glossaries.FirstOrDefault(gl => gl.Id == glossary.Id))
                        glossaries.Add(glossary);
                }

                SearchViewModel model = new SearchViewModel()
                {
                    Search = search,
                    Issues = new List<Issue>(_db.Issues.Where(i => EF.Functions.Like
                                                                       (i.Name.ToLower(), a) ||
                                                                   EF.Functions.Like(i.IssueDescription.ToLower(), a))),
                    Steps = new List<Step>(_db.Steps.Where(s => EF.Functions.Like
                                                                    (s.Name.ToLower(), a) ||
                                                                EF.Functions.Like(s.StepDescription.ToLower(), a))),

                    Glossaries = glossaries,
                    QuestionAnswers = new List<QuestionAnswer>(_db.QuestionAnswers.Where(s => EF.Functions.Like
                                                                                              (s.Question.ToLower(),
                                                                                                  a) ||
                                                                                              EF.Functions.Like(s.Answer.ToLower(),
                                                                                                  a))),
                    Sources = new List<Source>(_db.Sources.Where(b => EF.Functions.Like
                                                                (b.Name.ToLower(), a) ||
                                                                EF.Functions.Like(b.Keys.ToLower(), a)))
                };
                model.TotalFound = model.Issues.Count + model.Steps.Count + model.Glossaries.Count + 
                                   model.Sources.Count + model.QuestionAnswers.Count;
                return View(model);
            }

            return View();
        }
    }
}