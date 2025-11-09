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
    internal By By_Button_Primary;
    internal By By_Button_Secondary;

    protected BasePage(IWebDriver driver, int defaultTimeoutSeconds = 10)
    {
        Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(defaultTimeoutSeconds));
        BaseUrl = ConfigReader.GetBaseUrl();

        By_AppDiv = GetByClass("app");

        By_Button_Primary = GetByClass("button-primary");
        By_Button_Secondary = GetByClass("button-secondary");
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
        var element = WaitUntilVisible(locator);
        string tagName = element.TagName.ToLower();

        if (tagName == "input" || tagName == "textarea")
        {
            return element.GetAttribute("value") ?? string.Empty;
        }

        if (tagName == "select")
        {
            var select = new SelectElement(element);
            return select.SelectedOption?.Text ?? string.Empty;
        }

        return element.Text?.Trim() ?? string.Empty;
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

    internal By GetBySeleniumId(string seleniumId)
    {
        if (string.IsNullOrWhiteSpace(seleniumId))
            throw new ArgumentException("seleniumId cannot be null or empty", nameof(seleniumId));

        return By.CssSelector($"[selenium-id='{seleniumId}']");
    }

    internal By GetByClass(string className)
    {
        if (string.IsNullOrWhiteSpace(className))
            throw new ArgumentException("className cannot be null or empty", nameof(className));

        return By.CssSelector($"[class='{className}']");
    }

    internal By GetById(string idName)
    {
        if (string.IsNullOrWhiteSpace(idName))
            throw new ArgumentException("idName cannot be null or empty", nameof(idName));

        return By.CssSelector($"[id='{idName}']");
    }

    internal By GetByValue(string valueName)
    {
        if (string.IsNullOrWhiteSpace(valueName))
            throw new ArgumentException("valueName cannot be null or empty", nameof(valueName));

        return By.CssSelector($"[value='{valueName}']");
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

    public void AssertIfSelected(By locator, bool shouldBeSelected = true, string context = "")
    {
        try
        {
            var element = Driver.FindElement(locator);
            bool isSelected = element.Selected;

            if (shouldBeSelected)
            {
                AssertHelper.IsTrue(
                    isSelected,
                    $"Element {locator} is not selected, but should be",
                    context
                );
            }
            else
            {
                AssertHelper.IsFalse(
                    isSelected,
                    $"Element {locator} is selected, but should not be",
                    context
                );
            }

        }
        catch (NoSuchElementException)
        {
            AssertHelper.Fail($"Element {locator} has not been selected.", context);
        }
        catch (Exception ex)
        {
            AssertHelper.Fail($"Error during asserting if locator{locator} is selected: {ex.Message}", context);
        }
    }

    public void SelectFromDropdown(By locator, string option, string context = "")
    {
        try
        {
            var element = Driver.FindElement(locator);
            var select = new SelectElement(element);

            try
            {
                select.SelectByText(option);
                return;
            }
            catch (NoSuchElementException) { }

            try
            {
                select.SelectByValue(option);
                return;
            }
            catch (NoSuchElementException) { }

            var options = element.FindElements(By.TagName("option"));
            var targetOption = options.FirstOrDefault(o =>
                o.GetAttribute("selenium-id")?.Equals(option, StringComparison.OrdinalIgnoreCase) == true);

            if (targetOption != null)
            {
                targetOption.Click();
                return;
            }

            AssertHelper.Fail($"Cannot find '{option}' in dropdown {locator}.", context);
        }
        catch (NoSuchElementException)
        {
            AssertHelper.Fail($"Dropdown {locator} has not been found.", context);
        }
        catch (Exception ex)
        {
            AssertHelper.Fail($"Error during the looking for value: {locator} in dropdown. {ex.Message}", context);
        }
    }

    public void AssertDropdownValue(By locator, string expectedValue, string context = "")
    {
        try
        {
            var element = Driver.FindElement(locator);
            var select = new SelectElement(element);

            var selectedOption = select.SelectedOption;
            var text = selectedOption.Text?.Trim();
            var value = selectedOption.GetAttribute("value")?.Trim();
            var seleniumId = selectedOption.GetAttribute("selenium-id")?.Trim();

            bool match = string.Equals(text, expectedValue, StringComparison.OrdinalIgnoreCase)
                         || string.Equals(value, expectedValue, StringComparison.OrdinalIgnoreCase)
                         || string.Equals(seleniumId, expectedValue, StringComparison.OrdinalIgnoreCase);

            AssertHelper.IsTrue(
                match,
                $"Expected: '{expectedValue}' is not selected in {locator}. " +
                $"(Found: text='{text}', value='{value}', selenium-id='{seleniumId}')",
                context
            );
        }
        catch (NoSuchElementException)
        {
            AssertHelper.Fail($"Dropdown {locator} has not been found.", context);
        }
        catch (Exception ex)
        {
            AssertHelper.Fail($"Error during the assertion of dropdown value {locator}: {ex.Message}", context);
        }
    }
}
