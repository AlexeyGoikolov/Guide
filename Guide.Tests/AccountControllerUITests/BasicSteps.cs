using System;
using OpenQA.Selenium;

namespace Guide.Tests.AccountControllerUITests
{
    public class BasicSteps : IDisposable
    {
        private readonly IWebDriver _driver;
        private const string MainPageUrl = "http://128.199.37.179/";
        private const string LoginPageUrl = MainPageUrl + "Account/Login";

        public BasicSteps(IWebDriver driver)
        {
            _driver = driver;
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
        
        public void GoToUrl(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }
        
        public void GoToMainPage()
        {
            GoToUrl(MainPageUrl);
        }
        
        public string ReturnLoginPage()
        {
            string a = LoginPageUrl;
            return a;
        }
        
        public void ClickLink(string linkText)
        {
            _driver.FindElement(By.LinkText(linkText)).Click();
        }
        
        public void ClickButton(string caption)
        {
            _driver.FindElement(By.XPath($"//button[contains(text(),'{caption}'")).Click();
        }
        
        public void ClickById(string id)
        {
            _driver.FindElement(By.Id(id)).Click();
        }
        
        public void FilTextField(string fieldId, string inputText)
        {
            _driver.FindElement(By.Id(fieldId)).SendKeys(inputText);
        }
        
        public bool IsElementFound(string text)
        {
            var element = _driver.FindElement(By.XPath($"//*[contains(text(), '{text}')]"));
            return element != null;
        }
    }
}