using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guide.Models;
using Guide.Models.Data;
using Guide.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Guide.Controllers
{
    public class UsersController : Controller
    {
        private readonly GuideContext _db;
        private readonly UserManager<User> _userManager;

        public UsersController(GuideContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public  IActionResult ListUsers()
        {
            List<User> listUsers = Filter("true",null);
            ListUsersViewModel users = new ListUsersViewModel()
            {
                Users = listUsers,
                Positions = _db.Positions.ToList()
            };
            return View(users);
        }
        [HttpPost]
        public  IActionResult ListUsers(ListUsersViewModel usersViewModel)
        {
            List<User> listUsers = Filter(usersViewModel.Action,usersViewModel.idPosition);
            ListUsersViewModel users = new ListUsersViewModel()
            {
                Users = listUsers,
                Positions = _db.Positions.ToList()
            };
            return View(users);
        }
        [NonAction]
        public List<User> Filter(string? activ, string? idPositions)
        {
            IQueryable<User> users = _db.Users.Include(p => p.Position).Where(u => u.Email != "admin@admin.com");
            if (activ != null)
            {
                if (activ == "true")
                    users = users.Where(u => u.Active);
                if (activ == "false")
                    users = users.Where(u => u.Active == false);
            }
            if (idPositions != null)
                users = users.Where(u => u.PositionId == idPositions);
            return users.ToList();
        }
        [HttpPost]
        public async  Task<IActionResult> BlockingAjax(string idUser)
        {
            User user = _db.Users.FirstOrDefault(u => u.Id == idUser);
            if (user != null)
            {
                if (user.Active)
                {
                    user.Active = false;
                    await _userManager.UpdateAsync(user);
                    return Json(false);
                }
                user.Active = true;
                await _userManager.UpdateAsync(user);
                return Json(true);
            }
            return NotFound();
        }

        public async Task<IActionResult> GetPosition(string userId)
        {
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            List<Position> positions = await _db.Positions.ToListAsync();
            ListUsersViewModel model = new ListUsersViewModel
            {
                Users = new List<User>{user},
                Positions = positions
            };
            return PartialView("PartialViews/ChangePositionPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePosition(string userId, string positionId)
        {
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            Position position = await _db.Positions.FirstOrDefaultAsync(p => p.Id == positionId);
            if (user != null)
            {
                user.PositionId = positionId;
                _db.Update(user);
                await _db.SaveChangesAsync();
            }
            return Json(position.Name);
        }
    }
}