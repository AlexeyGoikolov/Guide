using System;
using System.Linq;
using System.Threading.Tasks;
using Guide.Models;
using Guide.Models.Data;
using Guide.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Guide.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly GuideContext _db;
        


        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            GuideContext db
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            return View(_db.Users.ToList());
        }

        [Authorize]
        public IActionResult Details()
        {
            User user = _userManager.GetUserAsync(User).Result;
            if (User.IsInRole("admin"))
                return RedirectToAction("Profile", "Service", new {area = "Admin"});
            return View(user);
        }

        [Authorize]
        public IActionResult Delete(string id)
        {
            User user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user!=null)
            {
                user.Active = false;
                _db.Users.Update(user);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        
        
        [Authorize]
        public IActionResult Edit(string id = null)
        {
            User user = new User();
            if (User.IsInRole("admin"))
            {
                user = id == null? _userManager.GetUserAsync(User).Result : _userManager.FindByIdAsync(id).Result;
            }
            else
            {
                user = _userManager.FindByIdAsync(id).Result;
            }
            return View(user);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(User userModel)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(userModel.Id);
                if (user != null)
                {
                    user.Email = userModel.Email;
                    
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Details");
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }

            return View();

        }

        [Authorize]
        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();
            ChangePasswordViewModel model = new ChangePasswordViewModel()
            {
                Id = user.Id,
                Email = user.Email
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var passwordValidator = HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                    var passwordHasher = HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;
                    var result = await passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                    if (result.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user, model.NewPassword);
                        await _userManager.UpdateAsync(user);
                        return RedirectToAction("Details");
                    }
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("NewPassword", error.Description);
                }
                ModelState.AddModelError("", "Пользователь не существует");
            }

            return View(model);
        }
        
        [Authorize(Roles = "admin")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(PositionsViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = model.User.Email,
                    UserName = model.User.Email,
                    Name = model.User.Name,
                    Surname = model.User.Surname,
                    PositionId = model.User.PositionsId
                };
                var result = await _userManager.CreateAsync(user, model.User.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Details", "Account");
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError(String.Empty, error.Description);
            }
            return View(model);
        }

        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel {ReturnUrl = returnUrl});
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByEmailAsync(model.Email);
                var result = await _signInManager.PasswordSignInAsync(
                    user,
                    model.Password,
                    model.RememberMe,
                    false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl)&&
                        Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    
                    return RedirectToAction("Details", "Account");
                }
                ModelState.AddModelError("", "Неправильный логин или пароль");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        //добавления должность
        public IActionResult CreatePositionAjax(Position position)
        {
            if (position.Name != null)
            {
                _db.Positions.Add(position);
                _db.SaveChanges();
            }
            PositionsViewModel model = new PositionsViewModel()
            {
                User = new RegisterViewModel(),
                Positions = _db.Positions.ToList()
            };
            
            return PartialView("PartialViews/PositionsPortal", model);
        }
        //Удоления должность
        public IActionResult DeletePositionAjax(string id)
        {
            Position position = _db.Positions.FirstOrDefault(p => p.Id == id);
            if (position != null)
            {
                _db.Positions.Remove(position);
                _db.SaveChanges();
            }
            PositionsViewModel model = new PositionsViewModel()
            {
                User = new RegisterViewModel(),
                Positions = _db.Positions.ToList()
            };
            return PartialView("PartialViews/PositionsPortal", model);
        }
    }
}