using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SimPitchSelenium.Utils;

public static class WaitHelper
{
    public static IWebElement WaitForElementVisible(IWebDriver driver, By locator, int timeout = 10)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
        return wait.Until(drv => drv.FindElement(locator));
    }

    internal static void Sleep()
    {
        Thread.Sleep(1500);
    }
}
