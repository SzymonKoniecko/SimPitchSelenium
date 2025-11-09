using System;
using OpenQA.Selenium;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Pages;

public class AllSimulationsPage : BasePage
{
    protected By By_Title;
    public AllSimulationsPage(IWebDriver webDriver) : base(driver: webDriver)
    {
        By_Title = GetBySeleniumId("title-all-simulations");
    }

    internal void AssertIfDisplayed()
    {
        AssertHelper.IsTrue(IsElementDisplayed(By_Title), "Page is not loaded", "AllSimulationsPage");
    }
}
