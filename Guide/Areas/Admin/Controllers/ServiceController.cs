using Microsoft.AspNetCore.Mvc;

namespace Guide.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
        
        
    }
}