using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
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
        private Auth auth = new Auth();

        [SetUp]
        public void startBrowser()
        {
            driver = new SafariDriver();
        }

        [Test()]
        public void Home()
        {
            // Go to the medical record page
            driver.Url = ("https://demo.openmrs.org/openmrs/login.htm");
            auth.doLogin(driver);
            verifyHomeItems();
            findPatientRecord();
        }

        void verifyHomeItems()
        {
            Thread.Sleep(5000);
            // Verify the list on Home to match to the required list
            var homeList = new string[] { "Find Patient Record", "Active Visits", "Register a patient", "Capture Vitals", "Appointment Scheduling", "Reports", "Data Management", "Configure Metadata", "System Administration" };
            ReadOnlyCollection<IWebElement> homeItems = driver.FindElements(By.XPath("//div[@class='col-12 col-sm-12 col-md-12 col-lg-12 homeList']//child::a[@type='button']/text()"));
            List<string> homeItemsText = homeItems.Select(o => o.Text.Trim().Replace("\n", "").Replace("\r", "")).Where(o => !String.IsNullOrEmpty(o)).ToList();
            Assert.True(homeList.SequenceEqual(homeItemsText));

            // Find the location dropdown and verify the options inside it
            var expLocaList = new string[] { "Inpatient Ward", "Isolation Ward", "Laboratory", "Outpatient Clinic", "Pharmacy", "Registration Desk"};
            ReadOnlyCollection<IWebElement> locaList = driver.FindElements(By.XPath("//div[@id='session-location']//child::ul/li"));
            List<string> actualLocaList = locaList.Select(o => o.Text.Trim()).Where(o => !String.IsNullOrEmpty(o)).ToList();
            Assert.True(expLocaList.SequenceEqual(actualLocaList));
        }

        void findPatientRecord()
        {
            // Find patient record and click to access
            IWebElement findPatient = driver.FindElement(By.XPath("//a[@id='coreapps-activeVisitsHomepageLink-coreapps-activeVisitsHomepageLink-extension' and @type='button']"));
            findPatient.Click();
            Thread.Sleep(5000);

            // Find the searchbox and identify the default value is empty
            IWebElement findSearchBox = driver.FindElement(By.XPath("//form[@id='patient-search-form']//child::input"));
            String searchDefaultValue = findSearchBox.GetAttribute("value");
            Assert.True(String.IsNullOrEmpty(searchDefaultValue));

            // Verify the colums titles in find patient record
            var listColumns = new string[] { "Identifier", "Name", "Gender", "Age", "Birthdate"};
            ReadOnlyCollection<IWebElement> bookingTable = driver.FindElements(By.XPath("//table[@class='table table-sm dataTable']//child::tr//th"));
            List<string> actualListColumns = bookingTable.Select(o => o.Text.Trim()).Where(o => !String.IsNullOrEmpty(o)).ToList();
            Assert.True(listColumns.SequenceEqual(actualListColumns));
        }

        // Create a exit process to close the browser whenever all the testing processes is finished
        [TearDown]
        public void closeBrowser()
        {
            driver.Close();
        }
    }
}
