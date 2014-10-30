using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Scenarioo.SeleniumNetIntegration
{
    [TestClass]
    public class MsTestSample
    {
        private static ScenariooMsTestIntegration _scenarioo;
        public TestContext TestContext { get; set; }
        private IWebDriver Driver { get; set; }

        [TestInitialize]
        public void SetUp()
        {
            _scenarioo = new ScenariooMsTestIntegration(TestContext);
            _scenarioo.ScenarioStart();

            var driver = new ChromeDriver(GetChromeOptions());

            if (ConfigurationManager.AppSettings["scenarioo.enabled"] == "true")
            {
                var scenarioWebDriverAdapter = new ScenariooWebDriverIntegration(driver, _scenarioo);
                Driver = scenarioWebDriverAdapter.Driver;
            }
            else
            {
                Driver = driver;
            }
        }
        
        [TestCleanup]
        public void TearDown()
        {
            _scenarioo.ScenarioEnd();
            Driver.Quit();
        }

        [TestMethod]
        public void Search_For_Tobi()
        {
            // Go to the home page
            Driver.Navigate().GoToUrl("http://www.google.ch");

            // Get the page elements
            var userNameField = Driver.FindElement(By.Name("q"));

            // Type user name and password
            userNameField.SendKeys("tobi");
            userNameField.Submit();
        }

        private ChromeOptions GetChromeOptions()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("test-type");

            return chromeOptions;
        }
    }
}