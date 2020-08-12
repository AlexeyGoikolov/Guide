using System.Linq;
using Guide.Models;
using Guide.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Guide.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class ServiceController : Controller
    {
        
        private readonly UserManager<User> _userManager;
        private readonly GuideContext _db;

        public ServiceController(UserManager<User> userManager, GuideContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        // GET
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Profile()
        {
            User user = _db.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
            return View(user);
        }
    }
}