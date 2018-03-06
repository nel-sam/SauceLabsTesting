using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace SauceLabsTesting
{
    // The parameters passed into the TestFixture attribute are used
    // by SauceLabs and define the platforms on which the tests will run.
    // To add more, see the Platform Configurator at https://saucelabs.com/platforms/
    // or direct link: https://wiki.saucelabs.com/display/DOCS/Platform+Configurator?&_ga=1.218594973.563852480.1455037110#/
    [TestFixture("chrome", "45", "Windows 7", "", "")]
    [TestFixture("Firefox", "44", "Windows 8.1", "", "")]
    [TestFixture("Internet Explorer", "11.0", "Windows 10", "", "")]
    public class SampleTestClass : UITestBase
    {
        // This constructor must be contain parameters that match the ones in the TestFixture
        // attribute above.
        public SampleTestClass(String browser, String version, String os, String deviceName, String deviceOrientation) : base(browser, version, os, deviceName, deviceOrientation)
        {
        }
        
        [Test]
        public void SampleTest()
        {
            List<string> searchStrs = new List<string>() { "Arizona", "Dell", "Tempe" };

            foreach (var searchStr in searchStrs)
            {
                driver.Navigate();
                driver.Url = Constants.Url;

                // Find the text input element by its name
                var searchTextBox = driver.FindElement(By.Name("q"));

                // Enter something to search for
                searchTextBox.SendKeys(searchStr);

                // Now submit the form. WebDriver will find the form for us from the element
                searchTextBox.Submit();

                // Check the title of the page
                string title = driver.GetTitleWaitForContains(searchStr);
                Assert.IsTrue(title.Contains(searchStr), "Search string not found in page title.");
            }
        }

        [SetUp]
        public void TestInitialize()
        {
            SetupBrowser();
        }

        [TearDown]
        public void TestCleanup()
        {
            try
            {
                if (!Constants.DebugMode)
                {
                    bool passed = TestContext.CurrentContext.Result.Outcome == ResultState.Success;
                    // Logs the result to Sauce Labs
                    ((IJavaScriptExecutor)driver).ExecuteScript("sauce:job-result=" + (passed ? "passed" : "failed"));
                }
            }
            finally
            {
                // Terminates the remote webdriver session
                driver.Quit();
            }
        }
    }
}