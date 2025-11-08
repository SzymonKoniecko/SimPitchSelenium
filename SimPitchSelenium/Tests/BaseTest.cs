using System;
using OpenQA.Selenium;
using SimPitchSelenium.Drivers;
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
        if (Driver != null)
        {
            try { Driver.Quit(); }
            finally { Driver.Dispose(); }
        }
    }
}