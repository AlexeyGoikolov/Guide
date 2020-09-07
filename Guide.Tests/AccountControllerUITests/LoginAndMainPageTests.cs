﻿using System;
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
            _basicSteps.GoToLoginPage();
            _basicSteps.FilTextField("email", "wrong@wrong.email");
            _basicSteps.FilTextField("password", "WrongPassword");
            _basicSteps.ClickById("submit");
            Assert.True(_basicSteps.IsElementFound("Неправильный логин или пароль"));
            
        }
        
        [Fact]
        public void LoginEmptyModelDataReturnsErrorMessageTest()
        {
            _basicSteps.GoToLoginPage();
            _basicSteps.FilTextField("email", string.Empty);
            _basicSteps.FilTextField("password", "WrongPassword");
            _basicSteps.ClickById("submit");
            Assert.True(_basicSteps.IsElementFound("Это поле обязательно"));
        }
        
        [Fact]
        public void LoginCorrectDataReturnsSuccessAuthTest()
        {
            
            _driver.Navigate().GoToUrl("http://localhost:5000/Account/Login");
            _driver.FindElement(By.Id("email")).SendKeys("admin@admin.com");
            _driver.FindElement(By.Id("password")).SendKeys("Qwerty123@");
            _driver.FindElement(By.Id("submit")).Click();
            var userCreateButtonLink = _driver.FindElement(By.LinkText("Добавить пользователя"))
                .GetAttribute("href");
            var changePasswordButtonLink = _driver.FindElement(By.LinkText("Изменить пароль"))
                .GetAttribute("href");
            var changeUserProfileLink = _driver.FindElement(By.LinkText("Изменить"))
                .GetAttribute("href");
            Assert.Contains("admin@admin.com", _driver.PageSource);
            Assert.Equal("http://localhost:5000/Account/Register", userCreateButtonLink);
            Assert.Contains("http://localhost:5000/Account/ChangePassword/", changePasswordButtonLink);
            Assert.Contains("http://localhost:5000/Account/Edit/", changeUserProfileLink);
        }
        
        [Fact]
        public void LogOutTest()
        {
            
            _driver.Navigate().GoToUrl("http://localhost:5000/Account/Login");
            _driver.FindElement(By.Id("email")).SendKeys("admin@admin.com");
            _driver.FindElement(By.Id("password")).SendKeys("Qwerty123@");
            _driver.FindElement(By.Id("submit")).Click();
            _driver.FindElement(By.LinkText("admin@admin.com")).Click();
            _driver.FindElement(By.Id("exitButton")).Click();
            Assert.Equal("http://localhost:5000/Account/Login", _driver.Url);
            

            
        }
    }
}