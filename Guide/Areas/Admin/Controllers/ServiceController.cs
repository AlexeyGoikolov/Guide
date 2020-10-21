using System.Linq;
using System.Threading.Tasks;
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
       public async Task<IActionResult> Profile(string id = null)
        {
            User user;
            if (id == null)
                 user = await _userManager.GetUserAsync(User);
            else
                user = _db.Users.FirstOrDefault(u => u.Id == id);

            return View(user);
        }
    }
}