using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guide.Models;
using Guide.Models.Data;
using Guide.Services;
using Guide.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Guide.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private UserRepository _db;
        
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, UserRepository db)
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
                user = _db.GetUser(id);
            UserDetailsViewModel model = new UserDetailsViewModel();
            model.User = user;
            model.Task = _db.GetUserTask(user.Id);
            model.Issues = _db.GetUserIssues(user.Id);
            model.PositionsIssues = _db.PositionsIssues(user.PositionId);
            foreach (var issue in model.PositionsIssues)
            {
                issue.BP = _db.BusinessProcessIssues(issue.Id);
            }
            foreach (var issue in model.Issues)
            {
                issue.BP = _db.BusinessProcessIssues(issue.Id);
            }
            model.Issues.OrderBy(i => i.BP);
            model.PositionsIssues.OrderBy(i => i.BP);
            return View(model);
        }

        
        [Authorize(Roles = "admin")]
        public IActionResult Delete(string id)
        {
            User user = _db.GetUser(id);
            if (user!=null)
            {
                user.Active = false;
                _db.Update(user);
                _db.Save();
            }
            return RedirectToAction("Details");
        }
        
        
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string id = null)
        {
            User user = await _userManager.FindByIdAsync(id);
            RegisterViewModel model = new RegisterViewModel();
            model.Positions = _db.GetActivePositions();
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
        public async Task<IActionResult> Edit(RegisterViewModel model)
        {
            if (model.UserEdit!=null)
            {
                User user = await _userManager.FindByIdAsync(model.UserEdit.Id);
                if (user != null)
                {
                    user.Name = model.UserEdit.Name;
                    user.Surname = model.UserEdit.Surname;
                    user.PositionId = (int) model.UserEdit.PositionsId;
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
                        await _userManager.UpdateAsync(user);
                        _db.Save();

                        return Redirect($"~/Admin/Service/Profile/{user.Id}");
                    }
                    if (role == "user")
                    {
                        await _userManager.AddToRoleAsync(user, role);
                        await _userManager.RemoveFromRoleAsync(user, "admin");
                        await _userManager.UpdateAsync(user);
                        _db.Save();

                        return Redirect($"~/Account/Details/{user.Id}");
                    }
                   
                }
            }
            model.Positions = _db.GetAllPositions();
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
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = model.Email,
                    UserName = model.Email,
                    Name = model.Name,
                    Surname = model.Surname,
                    PositionId = model.PositionsId
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    string role = Convert.ToString(model.Role);
                    await _userManager.AddToRoleAsync(user,role);
                    return Redirect($"~/Admin/UsersManage/ListUsers");
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
        public IActionResult CreatePositionAjax(Position position, RegisterViewModel data)
        {
            if (position.Name != null)
            {
                _db.AddPosition(position);
                _db.Save();
            }
            RegisterViewModel model = new RegisterViewModel()
            {
               Positions = _db.GetActivePositions()
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
            Position position = _db.GetPosition(id);
            if (position != null)
            {
                position.Active = false;
                _db.Save();
            }
            RegisterViewModel model = new RegisterViewModel()
            {
               Positions = _db.GetActivePositions()
            };
            return PartialView("PartialViews/PositionsPortal", model);
        }
    }
}