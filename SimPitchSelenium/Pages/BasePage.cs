using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Pages;

public abstract class BasePage
{
    protected readonly IWebDriver Driver;
    protected readonly WebDriverWait Wait;
    protected string BaseUrl;

    private NavBarPage _navBar;
    public NavBarPage NavBar => _navBar ??= new NavBarPage(Driver);

    protected By By_AppDiv;

    protected BasePage(IWebDriver driver, int defaultTimeoutSeconds = 10)
    {
        Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(defaultTimeoutSeconds));
        BaseUrl = ConfigReader.GetBaseUrl();

        By_AppDiv = GetByClass("app");
    }

    protected IWebElement WaitUntilVisible(By locator)
    {
        return Wait.Until(ExpectedConditions.ElementIsVisible(locator));
    }

    protected void Click(By locator)
    {
        var element = WaitUntilVisible(locator);
        element.Click();
    }

    protected void Type(By locator, string text, bool clear = true)
    {
        var element = WaitUntilVisible(locator);
        if (clear) element.Clear();
        element.SendKeys(text);
    }

    protected string GetElementText(By locator)
    {
        return WaitUntilVisible(locator).Text;
    }

    protected bool IsElementDisplayed(By locator)
    {
        try
        {
            return WaitUntilVisible(locator).Displayed;
        }
        catch
        {
            return false;
        }
    }

    protected void ScrollToElement(By locator)
    {
        var element = WaitUntilVisible(locator);
        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
    }

    public string GetTitle() => Driver.Title;

    protected By GetBySeleniumId(string seleniumId)
    {
        if (string.IsNullOrWhiteSpace(seleniumId))
            throw new ArgumentException("seleniumId cannot be null or empty", nameof(seleniumId));

        return By.CssSelector($"[selenium-id='{seleniumId}']");
    }

    protected By GetByClass(string className)
    {
        if (string.IsNullOrWhiteSpace(className))
            throw new ArgumentException("className cannot be null or empty", nameof(className));

        return By.CssSelector($"[class='{className}']");
    }

    public bool IsElementPresentAndEnabled(By locator)
    {
        try
        {
            var element = WaitUntilVisible(locator);

            return element.Displayed && element.Enabled;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
        catch (StaleElementReferenceException)
        {
            return false;
        }
    }
}
