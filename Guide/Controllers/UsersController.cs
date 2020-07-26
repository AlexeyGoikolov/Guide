using System.Collections.Generic;
using System.Linq;
using Guide.Models;
using Guide.Models.Data;
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
           
            List<User> users = _db.Users.Include(p=>p.Position).Where(u => u.Email != "admin@admin.com").ToList();
            
            return View(users);
        }
    }
}