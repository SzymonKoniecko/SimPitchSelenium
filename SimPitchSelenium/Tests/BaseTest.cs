using OpenQA.Selenium;
using NUnit.Framework;
using SimPitchSelenium.Reports;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Tests;

public abstract class BaseTest
{
    protected IWebDriver Driver = default!;
    protected string BaseUrl = default!;

    [ThreadStatic]
    private static IWebDriver? _driverInstance;

    public static IWebDriver DriverInstance
        => _driverInstance ?? throw new InvalidOperationException("DriverInstance not initialized for this thread.");

    [SetUp]
    public void SetUp()
    {
        Driver = WebDriverFactory.CreateDriver();
        _driverInstance = Driver;
    }

    [TearDown]
    public void TearDown()
    {
        try
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var testName = TestContext.CurrentContext.Test.Name;

            if (status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                try
                {
                    ErrorReporter.CaptureFailure(Driver, testName);
                }
                catch
                {
                }
            }
        }
        finally
        {
            try
            {
                Driver?.Quit();
            }
            catch { /* ignore */ }
            finally
            {
                Driver?.Dispose();
                _driverInstance = null;
            }
        }
    }
}
