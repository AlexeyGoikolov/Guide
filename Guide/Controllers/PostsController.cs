using System.Linq;
using Guide.Models;
using Guide.Models.Data;
using Guide.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Guide.Controllers
{
    public class PostsController : Controller
    {
        private readonly GuideContext _db;

        public PostsController(GuideContext db)
        {
            _db = db;
        }
        // GET
        public IActionResult Create()
        {
            return View(new Post());
        }

        
    }
}