using System.Collections.Generic;
using System.Linq;
using Guide.Models;
using Guide.Models.Data;
using Guide.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Guide.Controllers
{
    public class UsersController : Controller
    {
        private readonly GuideContext _db;

        public UsersController(GuideContext db)
        {
            _db = db;
        }

        public  IActionResult ListUsers()
        {
            List<User> listUsers = Filter(null,null);
            ListUsersViwModel users = new ListUsersViwModel()
            {
                Users = listUsers,
                Positions = _db.Positions.ToList()
            };
            return View(users);
        }
        [HttpPost]
        public  IActionResult ListUsers(ListUsersViwModel usersViwModel)
        {
            List<User> listUsers = Filter(usersViwModel.Action,usersViwModel.idPosition);
            ListUsersViwModel users = new ListUsersViwModel()
            {
                Users = listUsers,
                Positions = _db.Positions.ToList()
            };
            return View(users);
        }
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
    }
}