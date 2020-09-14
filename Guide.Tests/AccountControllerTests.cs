using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Guide.Controllers;
using Guide.Models;
using Guide.Models.Data;
using Guide.Services;
using Guide.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace Guide.Tests
{
    public class AccountControllerTests
    {
        public static User GetTestUser()
        {
            return new User{Id = "1", Email = "test@test.test", Name = "John", Surname = "Dou", UserName = "test@test.test"};
        }

        public ClaimsPrincipal GetClaims(string userRole)
        {
            IEnumerable<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, GetTestUser().UserName),
                new Claim(ClaimTypes.NameIdentifier, GetTestUser().Id),
                new Claim(ClaimTypes.Role, userRole),
                new Claim("name", GetTestUser().Name)
            }.AsEnumerable();
            var identity = new ClaimsIdentity(claims, "Identity.Application");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            return claimsPrincipal;
        }

        public ControllerContext GetControllerContext(string userRole)
        {
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = GetClaims(userRole)
                }
            };
            return context;
        }
        
        
        [Fact]
        public void DetailsReturnsViewResultWithUserDetailsViewModelTest()
        {
            var mockDb = new Mock<UserRepository>();
            var mockUserManager = new Mock<FakeUserManager>();
            var mockSignInManager = new Mock<FakeSignInManager>();
            mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(GetTestUser());

            mockDb.Setup(db => db.GetUserTask(It.IsAny<string>())).Returns(new TaskUser{Id = 1, Task = "aaa", UserId = "1"});
            mockDb.Setup(db => db.GetUserIssues(It.IsAny<string>())).Returns(new List<Issue> {new Issue{Id = 1, Name = "test"}});
            
            var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object, mockDb.Object);
            controller.ControllerContext = GetControllerContext("user");
            var result = controller.Details(null);
            
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<UserDetailsViewModel>(viewResult?.Model);
            Assert.Equal(typeof(UserDetailsViewModel), model?.GetType());
        }
        
        [Fact]
        public void DetailsIsInRoleAdminReturnRedirectToProfileTest()
        {
            var mockDb = new Mock<UserRepository>();
            var mockUserManager = new Mock<FakeUserManager>();
            var mockSignInManager = new Mock<FakeSignInManager>();
            
            var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object, mockDb.Object);
            controller.ControllerContext = GetControllerContext("admin");
            var result = controller.Details(null);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Service", redirectToActionResult.ControllerName);
            Assert.Equal("Profile", redirectToActionResult.ActionName);
        }

        [Fact]
        public void DeleteUserAndReturnsRedirectTest()
        {
            var mockDb = new Mock<UserRepository>();
            var mockUserManager = new Mock<FakeUserManager>();
            var mockSignInManager = new Mock<FakeSignInManager>();
            User user = GetTestUser();
            mockDb.Setup(db => db.GetUser(user.Id)).Returns(user);
            mockDb.Setup(db => db.Update(user)).Verifiable();
            mockDb.Setup(db => db.Save()).Verifiable();

            var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object, mockDb.Object);
            controller.ControllerContext = GetControllerContext("admin");
            
            var result = controller.Delete(user.Id);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Details", redirectToActionResult.ActionName);
            mockDb.Verify(r => r.Update(user));
            mockDb.Verify(r => r.Save());
        }

        [Fact]
        public async Task EditUserReturnsViewResultWithPositionsViewModelTest()
        {
            var mockDb = new Mock<UserRepository>();
            var mockUserManager = new Mock<FakeUserManager>();
            var mockSignInManager = new Mock<FakeSignInManager>();
            User user = GetTestUser();
            mockUserManager.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);

            mockDb.Setup(db => db.GetActivePositions()).Returns(new List<Position>{new Position{Id = 1, Name = "Test",Active = true}});

            
            var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object, mockDb.Object);
            controller.ControllerContext = GetControllerContext("user");
            var result = await controller.Edit(user.Id);
            
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsAssignableFrom<RegisterViewModel>(viewResult.Model);
            Assert.Equal(user.Name, viewModel.UserEdit.Name);
            Assert.Equal(typeof(RegisterViewModel), viewModel.GetType());
        }

        [Fact]
        public async Task EditUserInDbAndRedirectTest()
        {
            var mockDb = new Mock<UserRepository>();
            var mockUserManager = new Mock<FakeUserManager>();
            var mockSignInManager = new Mock<FakeSignInManager>();
            User user = GetTestUser();
            RegisterViewModel model = new RegisterViewModel();
            EditUserViewModel editViewModel = new EditUserViewModel
            {
               Id = user.Id, Email = user.Email, Name = user.Name, Surname = user.Surname, PositionsId = 1
            };
            model.Positions = new List<Position>{new Position()};
            model.UserEdit = editViewModel;

            mockUserManager.Setup(m => m.FindByIdAsync(model.UserEdit.Id)).ReturnsAsync(user);

            mockUserManager.Setup(m => m.AddToRoleAsync(user, It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            mockUserManager.Setup(m => m.RemoveFromRoleAsync(user, It.IsAny<string>())).Verifiable();
            mockUserManager.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object, mockDb.Object)
            {
                ControllerContext = GetControllerContext("admin")
            };

            var result = await controller.Edit(model);
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.NotNull(redirectResult.Url);
            Assert.Equal($"~/Admin/Service/Profile/{user.Id}", redirectResult.Url);
            mockUserManager.Verify(r => r.RemoveFromRoleAsync(user, It.IsAny<string>()));
        }

        [Fact]
        public async Task EditUserModelStateIsNotValidReturnViewModelTest()
        {
            var mockDb = new Mock<UserRepository>();
            var mockUserManager = new Mock<FakeUserManager>();
            var mockSignInManager = new Mock<FakeSignInManager>();
            User user = GetTestUser();
            RegisterViewModel model = new RegisterViewModel();
            model.Positions = new List<Position>{new Position()};
           // model.User = new RegisterViewModel{Name = GetTestUser().Name};
            
            mockDb.Setup(db => db.GetAllPositions()).Returns(new List<Position>());

            var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object, mockDb.Object)
            {
                ControllerContext = GetControllerContext("admin")
            };
            controller.ModelState.AddModelError("key", "error message");
            
            var result = await controller.Edit(model);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(typeof(RegisterViewModel), viewResult.Model.GetType());
        }
    }
}