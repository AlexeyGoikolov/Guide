using System.Linq;
using Guide.Models;
using Guide.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Guide.Controllers
{
    [Authorize]
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
                TaskUser task = _db.TaskUsers.FirstOrDefault(t => t.UserId == taskUser.UserId);
                if (task == null)
                {
                    TaskUser taskUser1 = new TaskUser()
                    {
                        Task = taskUser.Task,
                        UserId = taskUser.UserId
                    };
                    _db.TaskUsers.Add(taskUser1);
                }
                else
                {
                    task.Task = taskUser.Task;
                    _db.TaskUsers.Update(task);
                }

                _db.SaveChanges();
                string id = taskUser.UserId;
                return Redirect($"~/Account/Details/{taskUser.UserId}");
            }

            return RedirectToAction("Details", "Account");
        }
    }
}