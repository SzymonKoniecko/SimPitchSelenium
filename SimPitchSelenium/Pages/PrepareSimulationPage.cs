using System;
using OpenQA.Selenium;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Pages;

public class PrepareSimulationPage : BasePage
{
    protected By By_Title;
    public PrepareSimulationPage(IWebDriver webDriver) : base(driver: webDriver)
    {
        By_Title = GetBySeleniumId("title-prepare-simulation");
    }

    internal void AssertIfDisplayed()
    {
        var el = IsElementDisplayed(By_Title);
        AssertHelper.IsTrue(IsElementDisplayed(By_Title), "Page is not loaded", "PrepareSimulationPage");
    }
}
