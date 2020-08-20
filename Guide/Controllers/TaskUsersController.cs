using Guide.Models;
using Guide.Models.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Guide.Controllers
{
    public class TaskUsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly GuideContext _db;

        public TaskUsersController(GuideContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult Creat(TaskUser taskUser)
        {
            if (taskUser != null)
            {
                _db.TaskUsers.Update(taskUser);
                _db.SaveChanges();
                string id = taskUser.UserId;
                return RedirectToAction("Details", "Account",  id);
            }
            return RedirectToAction("Details", "Account");
        }
    }
}