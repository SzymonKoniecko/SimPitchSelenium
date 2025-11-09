using System;
using OpenQA.Selenium;

namespace SimPitchSelenium.Reports;

public static class ErrorReporter
{
    private static string ReportsDir =>
        Path.Combine(AppContext.BaseDirectory, "Reports", DateTime.Now.ToString("yyyy-MM-dd"));

    public static void CaptureFailure(IWebDriver driver, string testName)
    {
        try
        {
            Directory.CreateDirectory(ReportsDir);

            string timestamp = DateTime.Now.ToString("HH-mm-ss");
            string safeName = string.Concat(testName.Split(Path.GetInvalidFileNameChars()));
            string screenshotPath = Path.Combine(ReportsDir, $"{safeName}_{timestamp}.png");
            string htmlPath = Path.Combine(ReportsDir, $"{safeName}_{timestamp}.html");

            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(screenshotPath);


            File.WriteAllText(htmlPath, driver.PageSource);

            TestContext.WriteLine($"Test '{testName}' failed at {DateTime.Now}");
            TestContext.WriteLine($"Screenshot: {screenshotPath}");
            TestContext.WriteLine($"HTML source: {htmlPath}");
        }
        catch (Exception ex)
        {
            TestContext.WriteLine($"Failed to capture test info: {ex.Message}");
        }
    }
}