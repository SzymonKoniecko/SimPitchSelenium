using System;
using System.Globalization;
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

    public int GetTableCellCount(By tableLocator, string context = "")
    {
        try
        {
            if (tableLocator == null)
                throw new ArgumentException("Table  cannot be null or empty.", nameof(tableLocator));

            var tableElement = WaitForElement(tableLocator);

            var cells = tableElement.FindElements(By.TagName("td"));
            return cells.Count;
        }
        catch (NoSuchElementException)
        {
            AssertHelper.Fail($"Table not found.", context);
            return 0;
        }
        catch (Exception ex)
        {
            AssertHelper.Fail($"Error while counting <td> elements in table: {ex.Message}", context);
            return 0;
        }
    }

    public void SetRangeValue(By locator, int value, string context = "")
    {
        try
        {
            var element = WaitForElement(locator);

            string minStr = element.GetAttribute("min") ?? "0";
            string maxStr = element.GetAttribute("max") ?? "100";
            string stepStr = element.GetAttribute("step") ?? "1";

            int min = int.Parse(minStr, CultureInfo.InvariantCulture);
            int max = int.Parse(maxStr, CultureInfo.InvariantCulture);
            int step = int.Parse(stepStr, CultureInfo.InvariantCulture);

            if (value < min || value > max)
                AssertHelper.Fail($"Value {value} is outside range {min}-{max} for slider {locator}.", context);

            if ((value - min) % step != 0)
                AssertHelper.Fail($"Value {value} does not match slider step {step} for {locator}.", context);

            ((IJavaScriptExecutor)Driver).ExecuteScript(@"
            arguments[0].value = arguments[1];
            arguments[0].dispatchEvent(new Event('input', { bubbles: true }));
            arguments[0].dispatchEvent(new Event('change', { bubbles: true }));
        ", element, value);

            Thread.Sleep(200);
        }
        catch (Exception ex)
        {
            AssertHelper.Fail($"Failed to set range input value for {locator}: {ex.Message}", context);
        }
    }
    
    public void SetRangeValue(By locator, float value, string context = "")
    {
        try
        {
            var slider = WaitUntilVisible(locator);

            ((IJavaScriptExecutor)Driver).ExecuteScript(
                "arguments[0].value = arguments[1]; arguments[0].dispatchEvent(new Event('input')); arguments[0].dispatchEvent(new Event('change'));",
                slider,
                value.ToString(CultureInfo.InvariantCulture)
            );
        }
        catch (NoSuchElementException)
        {
            AssertHelper.Fail($"Slider {locator} was not found on the page.", context);
        }
        catch (Exception ex)
        {
            AssertHelper.Fail($"Error while setting slider value for {locator}: {ex.Message}", context);
        }
    }
}
