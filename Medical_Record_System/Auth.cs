using System;
using System.Collections.ObjectModel;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Medical_Record_System
{
    public class Auth
    {
        public void doLogin(IWebDriver driver)
        {
            // Find the username field and assign value into it
            IWebElement username = driver.FindElement(By.Name("username"));
            username.SendKeys("Admin");

            // Find the password field and assign value into it
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("Admin123");

            // Find the location section and select Inpatient Ward option
            ReadOnlyCollection<IWebElement> location = driver.FindElements(By.XPath("//ul[@id='sessionLocation']/li"));
            foreach (var opt in location)
            {
                if (opt.Text.Equals("Inpatient Ward"))
                {
                    opt.Click();
                    break;
                }
            }

            // Find the log in button and click after entered the email/password and selected location
            IWebElement pressLogin = driver.FindElement(By.Id("loginButton"));
            pressLogin.Click();

            // Create a wait method to wait the logging in process finishes successfully and move to Home page
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Func<IWebDriver, bool> waitForElement = new Func<IWebDriver, bool>((IWebDriver Web) =>
            {
                return Web.Title == "Home";
            });
            wait.Until(waitForElement);

            // Check whether the Dashboad page is displayed
            string currentWindowTitle = "Home";
            Assert.AreEqual(currentWindowTitle, driver.Title);
        }
    }
}
