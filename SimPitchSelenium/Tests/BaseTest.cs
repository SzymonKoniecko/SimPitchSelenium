using OpenQA.Selenium;
using NUnit.Framework;
using SimPitchSelenium.Reports;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Tests;

public abstract class BaseTest
{
    protected IWebDriver Driver = default!;
    protected string BaseUrl = default!;
    protected List<string> _createdSimulationIds = new();

    [ThreadStatic]
    private static IWebDriver? _driverInstance;

    public static IWebDriver DriverInstance
        => _driverInstance ?? throw new InvalidOperationException("DriverInstance not initialized for this thread.");

    [SetUp]
    public void SetUp()
    {
        Driver = WebDriverFactory.CreateDriver();
        _driverInstance = Driver;
        BaseUrl = ConfigReader.GetBaseUrl();
    }

    [TearDown]
    public void TearDown()
    {
        // Cleanup simulations
        foreach (var simId in _createdSimulationIds)
        {
            try
            {
                using var client = new System.Net.Http.HttpClient();
                client.BaseAddress = new Uri(ConfigReader.GetApiBaseUrl());
                var response = client.DeleteAsync($"/api/engine/Simulation/stop/{simId}").Result;
                if (!response.IsSuccessStatusCode)
                {
                    TestContext.WriteLine($"Failed to stop simulation {simId}. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Exception while stopping simulation {simId}: {ex.Message}");
            }
        }
        _createdSimulationIds.Clear();

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
