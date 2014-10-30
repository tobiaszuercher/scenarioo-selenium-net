using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.Extensions;
using Scenarioo.Model.Docu.Entities;

namespace Scenarioo.SeleniumNetIntegration
{
    public class ScenariooWebDriverIntegration
    {
        public IWebDriver Driver { get; private set; }
        public ScenariooMsTestIntegration Scenarioo { get; private set; }

        private int _screenshotCounter;

        public ScenariooWebDriverIntegration(IWebDriver driver, ScenariooMsTestIntegration scenarioo)
        {
            Scenarioo = scenarioo;

            var eventFiringWebDriver = new EventFiringWebDriver(driver);
            eventFiringWebDriver.ElementClicking += (sender, args) => GenerateStep(args.Driver);
            eventFiringWebDriver.ElementClicked += (sender, args) => GenerateStep(args.Driver);
            eventFiringWebDriver.Navigating += (sender, args) => GenerateStep(args.Driver);
            eventFiringWebDriver.Navigated += (sender, args) => GenerateStep(args.Driver);

            Driver = eventFiringWebDriver;
        }

        protected virtual void GenerateStep(IWebDriver driver)
        {
            var step = new Step();
            step.Page = new Page(new Uri(driver.Url).AbsolutePath);
            step.StepHtml = new StepHtml(driver.PageSource);
            step.StepDescription = new StepDescription();
            step.StepDescription.Index = _screenshotCounter;
            step.StepDescription.Title = driver.Title;
            step.StepDescription.ScreenshotFileName = string.Format("{0:000}.png", _screenshotCounter);

            Scenarioo.SaveStep(step, Driver.TakeScreenshot().AsByteArray);

            _screenshotCounter += 1;
        }
    }
}