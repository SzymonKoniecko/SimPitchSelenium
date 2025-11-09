using System;
using OpenQA.Selenium;
using SimPitchSelenium.Drivers;
using SimPitchSelenium.Reports;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Tests;

public abstract class BaseTest
{
    protected IWebDriver Driver;
    protected string BaseUrl;

    [SetUp]
    public void SetUp()
    {
        BaseUrl = ConfigReader.GetBaseUrl();
        var browser = ConfigReader.GetBrowser();
        var headless = ConfigReader.GetHeadless();

        Driver = WebDriverFactory.CreateDriver(browser, headless);
    }

    [TearDown]
    public void TearDown()
    {
        var outcome = TestContext.CurrentContext.Result.Outcome.Status;
        var testName = TestContext.CurrentContext.Test.Name;

        if (outcome == NUnit.Framework.Interfaces.TestStatus.Failed && Driver != null)
        {
            ErrorReporter.CaptureFailure(Driver, testName);
            Thread.Sleep(1000);
        }

        if (Driver != null)
        {
            try { Driver.Quit(); }
            finally { Driver.Dispose(); }
        }
    }
}