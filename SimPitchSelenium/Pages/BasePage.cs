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

    protected IWebElement WaitForElement(By locator, int? timeoutSeconds = null)
    {
        var wait = timeoutSeconds.HasValue
            ? new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds.Value))
            : Wait;

        return wait.Until(ExpectedConditions.ElementIsVisible(locator));
    }

    internal SimulationItemPage GoToSimulationItemPageViaUrl(string simulationId)
    {
        Thread.Sleep(500);
        Driver.Navigate().GoToUrl(BaseUrl + "/simulation/" + simulationId);
        return new SimulationItemPage(Driver);
    }

    internal IterationResultPage GoToIterationResultPage(string simulationId, string iterationResultId)
    {
        Thread.Sleep(500);
        Driver.Navigate().GoToUrl(BaseUrl + "/simulation/" + simulationId + "/iteration/" + iterationResultId);
        return new IterationResultPage(Driver);
    }

    protected IWebElement WaitUntilVisible(By locator)
    {
        return Wait.Until(ExpectedConditions.ElementIsVisible(locator));
    }

    protected void Click(By locator)
    {
        var element = WaitForElement(locator);
        element.Click();
    }

    protected void Type(By locator, string text, bool clear = true)
    {
        var element = WaitForElement(locator);
        if (clear) element.Clear();
        element.SendKeys(text);
    }

    protected string GetElementText(By locator)
    {
        var element = WaitForElement(locator);
        string tagName = element.TagName.ToLower();

        if (tagName == "input" || tagName == "textarea")
            return element.GetAttribute("value") ?? string.Empty;

        if (tagName == "select")
        {
            var select = new SelectElement(element);
            return select.SelectedOption?.Text ?? string.Empty;
        }

        return element.Text?.Trim() ?? string.Empty;
    }

    public int GetElementCount(By locator)
    {
        var elements = Driver.FindElements(locator);
        return elements.Count;
    }

    protected bool IsElementDisplayed(By locator)
    {
        try
        {
            return WaitForElement(locator).Displayed;
        }
        catch
        {
            return false;
        }
    }

    protected void ScrollToElement(By locator)
    {
        var element = WaitForElement(locator);
        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
    }

    public string GetTitle() => Driver.Title;

    internal By GetBySeleniumId(string seleniumId)
    {
        if (string.IsNullOrWhiteSpace(seleniumId))
            throw new ArgumentException("seleniumId cannot be null or empty", nameof(seleniumId));

        return By.CssSelector($"[selenium-id='{seleniumId}']");
    }

    internal By GetAnyBySeleniumId(string seleniumId)
    {
        if (string.IsNullOrWhiteSpace(seleniumId))
            throw new ArgumentException("seleniumId cannot be null or empty", nameof(seleniumId));

        return By.CssSelector($"[selenium-id*='{seleniumId}']");
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
            var element = WaitForElement(locator);
            return element.Displayed && element.Enabled;
        }
        catch
        {
            return false;
        }
    }

    public void AssertTextDisplayed(string text, string context = "")
    {
        try
        {
            var appDiv = WaitForElement(By_AppDiv);
            string html = appDiv.GetAttribute("innerHTML");
            bool containsText = html != null && html.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0;

            AssertHelper.IsTrue(
                containsText,
                $"Expected text '{text}' was not found in the application HTML.",
                context
            );
        }
        catch (Exception ex)
        {
            AssertHelper.Fail($"Error while checking if text '{text}' is displayed: {ex.Message}", context);
        }
    }

    internal void WaitForText(string text)
    {
        int retry = 0;
        while (!IsElementWithTextDisplayed(text))
        {
            Thread.Sleep(500);
            retry++;
            if (retry == 100)
                throw new Exception("Waiting in loop?");
        }
    }

    internal bool IsElementWithTextDisplayed(string text, string? tag = null)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Text cannot be null or empty.", nameof(text));

        try
        {
            string safeText = text.Replace("'", "&apos;");
            string xpath = string.IsNullOrWhiteSpace(tag)
                ? $"//*[contains(normalize-space(text()), '{safeText}')]"
                : $"//{tag}[contains(normalize-space(text()), '{safeText}')]";

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            var element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xpath)));
            return element.Displayed;
        }
        catch
        {
            return false;
        }
    }

    public void AssertIfSelected(By locator, bool shouldBeSelected = true, string context = "")
    {
        try
        {
            var element = WaitForElement(locator);
            bool isSelected = element.Selected;

            if (shouldBeSelected)
                AssertHelper.IsTrue(isSelected, $"Element {locator} is not selected, but should be", context);
            else
                AssertHelper.IsFalse(isSelected, $"Element {locator} is selected, but should not be", context);
        }
        catch (Exception ex)
        {
            AssertHelper.Fail($"Error during asserting if locator {locator} is selected: {ex.Message}", context);
        }
    }

    public void SelectFromDropdown(By locator, string option, string context = "")
    {
        try
        {
            var element = WaitForElement(locator);
            var select = new SelectElement(element);

            try { select.SelectByText(option); return; }
            catch (NoSuchElementException) { }

            try { select.SelectByValue(option); return; }
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
        catch (Exception ex)
        {
            AssertHelper.Fail($"Error during the looking for value in dropdown {locator}: {ex.Message}", context);
        }
    }

    public void AssertDropdownValue(By locator, string expectedValue, string context = "")
    {
        try
        {
            var element = WaitForElement(locator);
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
        catch (Exception ex)
        {
            AssertHelper.Fail($"Error during the assertion of dropdown value {locator}: {ex.Message}", context);
        }
    }

    public void AssertUrlContains(string expectedPart, string context = "")
    {
        try
        {
            string currentUrl = Driver.Url;
            AssertHelper.IsTrue(
                currentUrl.Contains(expectedPart, StringComparison.OrdinalIgnoreCase),
                $"URL '{currentUrl}' does not contain: '{expectedPart}'.",
                context
            );
        }
        catch (Exception ex)
        {
            AssertHelper.Fail($"Error during URL check: {ex.Message}", context);
        }
    }

    public bool IsButtonDisabled(By locator)
    {
        try
        {
            var element = WaitForElement(locator);
            return !element.Enabled
                   || element.GetAttribute("disabled") != null
                   || element.GetAttribute("aria-disabled") == "true";
        }
        catch (Exception)
        {
            return false;
        }
    }

    public void AssertIfButtonDisabled(By locator, bool shouldBeDisabled = true, string context = "")
    {
        try
        {
            var element = WaitForElement(locator);
            bool isDisabled = !element.Enabled
                              || element.GetAttribute("disabled") != null
                              || element.GetAttribute("aria-disabled") == "true";

            if (shouldBeDisabled)
                AssertHelper.IsTrue(isDisabled, $"Button {locator} should be disabled but it is enabled.", context);
            else
                AssertHelper.IsFalse(isDisabled, $"Button {locator} should be enabled but it is disabled.", context);
        }
        catch (Exception ex)
        {
            AssertHelper.Fail($"Error while checking button state for {locator}: {ex.Message}", context);
        }
    }

    internal bool ScrollToText(string text, string? tag = null)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Text cannot be null or empty.", nameof(text));

        try
        {
            string safeText = text.Replace("'", "&apos;");
            string xpath = string.IsNullOrWhiteSpace(tag)
                ? $"//*[contains(normalize-space(text()), '{safeText}')]"
                : $"//{tag}[contains(normalize-space(text()), '{safeText}')]";

            var element = WaitForElement(By.XPath(xpath));
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center' });", element);
            Thread.Sleep(300);
            return true;
        }
        catch
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
