using System;
using Guide.Models;
using Microsoft.AspNetCore.Identity;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace Guide.Tests.AccountControllerUITests
{
    public class LoginAndMainPageTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly BasicSteps _basicSteps;

        public LoginAndMainPageTests()
        {
            _driver = new ChromeDriver();
            _basicSteps = new BasicSteps(_driver);
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
        
        [Fact]
        public void LoginWrongModelDataReturnsErrorMessageTest()
        {
            _basicSteps.GoToMainPage();
            _basicSteps.FilTextField("email", "wrong@wrong.email");
            _basicSteps.FilTextField("password", "WrongPassword");
            _basicSteps.ClickById("submit");
            Assert.True(_basicSteps.IsElementFound("Неправильный логин или пароль"));
        }
        
        [Fact]
        public void LoginEmptyModelDataReturnsErrorMessageTest()
        {
            _basicSteps.GoToMainPage();
            _basicSteps.FilTextField("email", string.Empty);
            _basicSteps.FilTextField("password", "WrongPassword");
            _basicSteps.ClickById("submit");
            Assert.True(_basicSteps.IsElementFound("Это поле обязательно"));
        }
        
        [Fact]
        public void LoginCorrectDataReturnsSuccessAuthTest()
        {
            _basicSteps.GoToMainPage();
            _driver.FindElement(By.Id("email")).SendKeys("admin@admin.com");
            _driver.FindElement(By.Id("password")).SendKeys("Qwerty123@");
            _driver.FindElement(By.Id("submit")).Click();
            var userEditButtonLink = _driver.FindElement(By.Id("edit-profile"))
                .GetAttribute("href");
            Assert.Contains("Личный кабинет : администратор", _driver.PageSource);
            Assert.Contains("http://128.199.37.179/Account/Edit", userEditButtonLink);
        }
        
        [Fact]
        public void LogOutTest()
        {
            _basicSteps.GoToMainPage();
            _driver.FindElement(By.Id("email")).SendKeys("admin@admin.com");
            _driver.FindElement(By.Id("password")).SendKeys("Qwerty123@");
            _driver.FindElement(By.Id("submit")).Click();
            _driver.FindElement(By.Id("navbar-user-name")).Click();
            _driver.FindElement(By.Id("exitButton")).Click();
            string loginPage = _basicSteps.ReturnLoginPage();
            Assert.Equal(loginPage, _driver.Url);
        }
    }
}