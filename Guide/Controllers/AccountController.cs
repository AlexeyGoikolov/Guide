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
        

        [Authorize]
        public IActionResult Details(string id)
        {
            User user = new User();
            if (User.IsInRole("admin") && id == null)
                return RedirectToAction("Profile", "Service", new {area = "Admin"});
            
            if (id == null)
                user = _userManager.GetUserAsync(User).Result;
            else
                user = _db.Users.FirstOrDefault(u => u.Id == id);
            UserDetailsViewModel model = new UserDetailsViewModel();
            model.User = user;
            model.Task = _db.TaskUsers.FirstOrDefault(t => t.UserId == user.Id);
            model.Issues = _db.UserIssues.OrderBy(d => d.Id)
                .Where(d => d.UserId == user.Id).
                Select(s => s.Issue).ToList();
            return View(model);
        }

        
        [Authorize(Roles = "admin")]
        public IActionResult Delete(string id)
        {
            User user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user!=null)
            {
                user.Active = false;
                _db.Users.Update(user);
                _db.SaveChanges();
            }
            return RedirectToAction("Details");
        }
        
        
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string id = null)
        {
            User user = await _userManager.FindByIdAsync(id);
            PositionsViewModel model = new PositionsViewModel();
            model.Positions = _db.Positions.Where(p => p.Active).ToList();
            model.UserEdit = new EditUserViewModel();
            if (user != null)
            {
                model.UserEdit.Id = user.Id;
                model.UserEdit.Name = user.Name;
                model.UserEdit.Email = user.Email;
                model.UserEdit.PositionsId = user.PositionId;
                model.UserEdit.Surname = user.Surname;
                if (await _userManager.IsInRoleAsync(user, "admin"))
                    model.UserEdit.Role = Roles.admin;
                else
                    model.UserEdit.Role = Roles.user;
                model.UserEdit.Id = user.Id;
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(PositionsViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.UserEdit.Id);
                if (user != null)
                {
                    user.Name = model.UserEdit.Name;
                    user.Surname = model.UserEdit.Surname;
                    user.PositionId = model.UserEdit.PositionsId;
                    user.Email = model.UserEdit.Email;
                    user.UserName = model.UserEdit.Name + " " + model.UserEdit.Surname;
                    if (model.UserEdit.Password != null)
                    {
                        await ChangePasswordUsers(user, model.UserEdit.Password);
                    }
                    
                    string role = Convert.ToString(model.UserEdit.Role);
                    if (role == "admin")
                    {
                        await _userManager.AddToRoleAsync(user, role);
                        await _userManager.RemoveFromRoleAsync(user, "user");
                    }
                    if (role == "user")
                    {
                        await _userManager.AddToRoleAsync(user, role);
                        await _userManager.RemoveFromRoleAsync(user, "admin");
                    }
                    await _userManager.UpdateAsync(user);
                    await _db.SaveChangesAsync();
                    
                    return Redirect($"~/Account/Details/{user.Id}");
                }
            }
            model.Positions = _db.Positions.ToList();
            return View(model);
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
                    await ChangePasswordUsers(user, model.NewPassword);
                    return Redirect($"~/Account/Details/{user.Id}");
                }
                ModelState.AddModelError("", "Пользователь не существует");
            }
            return View(model);
        }

        private async Task ChangePasswordUsers(User user, string password)
        {
            var passwordValidator = HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
            var passwordHasher = HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;
            var result = await passwordValidator.ValidateAsync(_userManager, user, password);
            if (result.Succeeded)
            {
                if (passwordHasher != null)
                    user.PasswordHash = passwordHasher.HashPassword(user, password);
                await _userManager.UpdateAsync(user);
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("NewPassword", error.Description);
            }
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
                    string role = Convert.ToString(model.User.Role);
                    await _userManager.AddToRoleAsync(user,role);
                    return Redirect($"~/Account/Details/{user.Id}");
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
                if (user != null)
                {
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
        public IActionResult CreatePositionAjax(Position position, PositionsViewModel data)
        {
            if (position.Name != null)
            {
                _db.Positions.Add(position);
                _db.SaveChanges();
            }
            PositionsViewModel model = new PositionsViewModel()
            {
                User = new RegisterViewModel(),
                // ReSharper disable once RedundantBoolCompare
                Positions = _db.Positions.Where(p=>p.Active).ToList()
                
            };
            if (data != null)
            {
                model.UserEdit = data.UserEdit;
            }
            return PartialView("PartialViews/PositionsPortal", model);
            
        }
        //Удаление должности
        public IActionResult DeletePositionAjax(int id)
        {
            Position position = _db.Positions.FirstOrDefault(p => p.Id == id);
            if (position != null)
            {
                position.Active = false;
                _db.SaveChanges();
            }
            PositionsViewModel model = new PositionsViewModel()
            {
                User = new RegisterViewModel(),
                Positions = _db.Positions.Where(p=>p.Active).ToList()
            };
            return PartialView("PartialViews/PositionsPortal", model);
        }
    }
}