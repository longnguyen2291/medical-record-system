using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Medical_Record_System
{
    [TestFixture()]
    public class Test
    {
        // Create a setup process to access into medical record system
        IWebDriver driver;
        [SetUp]
        public void startBrowser()
        {
            driver = new SafariDriver();
        }

        [Test()]
        public void Main()
        {
            // Go to the medical record page
            driver.Url = ("https://demo.openmrs.org/openmrs/login.htm");
            doLogin();
            verifyHomeItems();
        }

        void doLogin()
        {
            // Find the username field and assign value into it
            IWebElement username = driver.FindElement(By.Name("username"));
            username.SendKeys("Admin");

            // Find the password field and assign value into it
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("Admin123");

            // Find the location section and select Inpatient Ward option
            ReadOnlyCollection<IWebElement> location = driver.FindElements(By.XPath("//ul[@id='sessionLocation']/li"));
            foreach(var opt in location)
            {
                if(opt.Text.Equals("Inpatient Ward"))
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

        void verifyHomeItems()
        {
            Thread.Sleep(5000);
            var homeList = new string[] { "Find Patient Record", "Active Visits", "Register a patient", "Capture Vitals", "Appointment Scheduling", "Reports", "Data Management", "Configure Metadata", "System Administration" };
            ReadOnlyCollection<IWebElement> homeItems = driver.FindElements(By.XPath("//div[@class='col-12 col-sm-12 col-md-12 col-lg-12 homeList']//child::a[@type='button']/text()"));
            List<string> homeItemsText = homeItems.Select(o => o.Text.Replace("\n", "").Replace("\r", "")).ToList();
            Assert.True(homeList.SequenceEqual(homeItemsText));
        }

        // Create a exit process to close the browser whenever all the testing processes is finished
        [TearDown]
        public void closeBrowser()
        {
            driver.Close();
        }
    }
}
