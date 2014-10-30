using System;
using System.IO;
using NUnit.Core;
using NUnit.Core.Extensibility;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Scenarioo.Model.Docu.Entities;

namespace Scenarioo.SeleniumNetIntegration
{
    [TestFixture]
    public class NUnitSample
    {
        public IWebDriver Driver { get; set; }

        [Test]
        public void Search_For_Jack_Bauer()
        {
            Driver = new ChromeDriver();

            // Go to the home page
            Driver.Navigate().GoToUrl("http://www.google.ch");

            // Get the page elements
            var userNameField = Driver.FindElement(By.Name("q"));

            // Type user name and password
            userNameField.SendKeys("tobi");
            userNameField.Submit();

            Driver.Dispose();
        }

        [Test]
        public void Another_Tet()
        {
            
        }
    }

    [TestFixture]
    public class NUnitSample2
    {
        [Test]
        public void Gugus()
        {
            
        }
    }

    [NUnitAddin]
    public class ScenariooDecorator : IAddin
    {
        public bool Install(IExtensionHost host)
        {
            var bla = host.GetExtensionPoint("EventListeners");
            bla.Install(new ScenariooListener());
            

            return true;
        }
    }

    public class ScenariooListener : EventListener
    {
        private string BranchName { get; set; }
        private string BuildName { get; set; }
        private string ScenarioName { get; set; }
        private string UseCaseName { get; set; }

        public ScenariooListener(string branchName = "master", string buildName = "build")
        {
            BranchName = branchName;
            BuildName = buildName;
        }

        public void RunStarted(string name, int testCount)
        {
            Console.WriteLine("RunStarted {0} {1}", name, testCount);
        }

        public void RunFinished(TestResult result)
        {
            Console.WriteLine("RunFinished {0} {1}", result.Name, result.IsSuccess);
        }

        public void RunFinished(Exception exception)
        {
            Console.WriteLine("Run finished with exc");
        }

        public void TestStarted(TestName testName)
        {
            Console.WriteLine("TestStarted: {0}", testName.Name);
        }

        public void TestFinished(TestResult result)
        {
            Console.WriteLine("TestFinished {0} {1}", result.Name, result.IsSuccess);
        }

        public void SuiteStarted(TestName testName)
        {
            Console.WriteLine("SuiteStarted {0}", testName.FullName);
        }

        public void SuiteFinished(TestResult result)
        {
            Console.WriteLine("SuiteFinished {0}", result.ResultState);
        }

        public void UnhandledException(Exception exception)
        {
            Console.WriteLine("Unhandled exc");
        }

        public void TestOutput(TestOutput testOutput)
        {
            ////Console.WriteLine("TestOutput");
        }
    }
}