using System;
using SimPitchSelenium.Pages;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Tests;

[TestFixture]
[NonParallelizable]
public class SimulationItemTests : BaseTest
{
    private SimulationItemPage _simulationItemPage;
    private MainPage _mainPage;
    private string SimulationId = String.Empty;
    private static string StaticSimulationId = String.Empty;

    [SetUp]
    public void Init()
    {
        _mainPage = new MainPage(Driver).Open();
    }

    [Test]
    public void SimulationItem_Simple_Creation_Test()
    {
        var prepPage = _mainPage.GoToPrepareSimulationViaSectionButton();
        prepPage.StartAnySimulation(1); // just 1 iteration so it's fast
        SimulationId = prepPage.GetSimulationId();
        _createdSimulationIds.Add(SimulationId);
        _simulationItemPage = prepPage.GoToSimulationItemPage();
        _simulationItemPage.AssertIfDisplayed(SimulationId);
    }

    private void EnsureSimulationExists()
    {
        if (String.IsNullOrEmpty(StaticSimulationId))
        {
            var prepPage = _mainPage.GoToPrepareSimulationViaSectionButton();
            prepPage.StartAnySimulation(20);
            StaticSimulationId = prepPage.GetSimulationId();
            _createdSimulationIds.Add(StaticSimulationId);
            _simulationItemPage = prepPage.GoToSimulationItemPage();
            _simulationItemPage.AssertIfDisplayed(StaticSimulationId);
            _simulationItemPage.WaitForCompletedSimulation();
        }
        else
        {
            _simulationItemPage = _mainPage.GoToSimulationItemPageViaUrl(StaticSimulationId);
            _simulationItemPage.AssertIfDisplayed(StaticSimulationId);
            _simulationItemPage.WaitForCompletedSimulation();
        }
        SimulationId = StaticSimulationId;
    }

    [Test]
    public void SimulationItem_Assert_Status_And_Refresh()
    {
        System.Threading.Thread.Sleep(4000); // Give it a moment to start
        // Preparation (large iteration number to cover changes in statuses)
        _mainPage = new MainPage(Driver).Open();
        var prepPage = _mainPage.GoToPrepareSimulationViaSectionButton();
        prepPage.StartAnySimulation(200);
        SimulationId = prepPage.GetSimulationId();
        _createdSimulationIds.Add(SimulationId);
        _simulationItemPage = prepPage.GoToSimulationItemPageViaUrl(SimulationId);
        _simulationItemPage.AssertIfDisplayed(SimulationId);

        _simulationItemPage.AssertSimulationState("Running");
        _simulationItemPage.AssertIfIterationsPercentageIsNot100();
    }

    [Test]
    public void SimulatonItem_Assert_Pagination()
    {
        EnsureSimulationExists();
        if (String.IsNullOrEmpty(SimulationId))
            throw new Exception("Init not completed? Init() - SimulatonItem_Assert_Pagination");

        // The simulation is already loaded and completed in Init()

        // Pagination
        _simulationItemPage.Pagination.CheckIfItsFirstPage();
        _simulationItemPage.AssertIterationCount(10);
        _simulationItemPage.Pagination.GoToLatestPage();
        _simulationItemPage.Pagination.SelectPageSize("5");
        WaitHelper.Sleep();
        _simulationItemPage.WaitForText("Check complete iteration details");
        _simulationItemPage.AssertIterationCount(5);
    }
    
    [Test]
    public void SimulatonItem_Assert_Filter()
    {
        EnsureSimulationExists();
        if (String.IsNullOrEmpty(SimulationId))
            throw new Exception("Init not completed? Init() - SimulatonItem_Assert_Filter");

        // The simulation is already loaded and completed in Init()
        
        // Filter
        _simulationItemPage.Pagination.CheckIfItsFirstPage();
        _simulationItemPage.Pagination.SelectPageSize("10");
        WaitHelper.Sleep();
        _simulationItemPage.WaitForText("Check complete iteration details");
        _simulationItemPage.Filter.SetSortingMethod("order-by-iteration");
        WaitHelper.Sleep();
        _simulationItemPage.WaitForText("Check complete iteration details");
        _simulationItemPage.Filter.SetSortingMethod("order-by-iteration");
        _simulationItemPage.AssertIterationCount(10);
        _simulationItemPage.Filter.ChangeSortingOrder();
        _simulationItemPage.AssertIterationCount(10);
        _simulationItemPage.AssertTextDisplayed("Toggle Ascending");

        _simulationItemPage.Filter.SetSortingMethod("team", "jagiellonia-bialystok");
        _simulationItemPage.WaitForText("Check complete iteration details");
    }

    [Test]
    public void SimulationItem_Assert_HeatMap()
    {
        EnsureSimulationExists();
        if (String.IsNullOrEmpty(SimulationId))
            throw new Exception("Init not completed? Init() - SimulationItem_Assert_HeatMap");
            
        // The simulation is already loaded and completed in Init()
        WaitHelper.Sleep();
        
        _simulationItemPage.AssertPercentSumEquals100(0, "Team row index 0");
        _simulationItemPage.AssertPercentSumEquals100(10, "Team row index 10");
    }

    [Test]
    public void SimulationItem_Assert_Stop_Simulation()
    {
        System.Threading.Thread.Sleep(4000); // Give it a moment to start
        _mainPage = new MainPage(Driver).Open();
        var prepPage = _mainPage.GoToPrepareSimulationViaSectionButton();
        prepPage.StartAnySimulation(400);
        SimulationId = prepPage.GetSimulationId();
        _createdSimulationIds.Add(SimulationId);
        _simulationItemPage = prepPage.GoToSimulationItemPageViaUrl(SimulationId);
        _simulationItemPage.AssertIfDisplayed(SimulationId);

        _simulationItemPage.AssertSimulationState("Running");

        // Cancel via API directly
        using (var client = new System.Net.Http.HttpClient()) 
        {
            client.BaseAddress = new Uri(ConfigReader.GetApiBaseUrl());
            var response = client.DeleteAsync($"/api/engine/Simulation/stop/{SimulationId}").Result;
            Assert.That(response.IsSuccessStatusCode, Is.True, "API call to stop simulation failed.");
        }
        
        System.Threading.Thread.Sleep(1000); // Give it a moment to stop
        
        // Assert state via API instead of UI
        using (var client = new System.Net.Http.HttpClient())
        {
            client.BaseAddress = new Uri(ConfigReader.GetApiBaseUrl());
            var getResponse = client.GetAsync($"/api/engine/Simulation/{SimulationId}").Result;
            Assert.That(getResponse.IsSuccessStatusCode, Is.True, "API call to get simulation failed.");
            var json = getResponse.Content.ReadAsStringAsync().Result;
            var jObject = Newtonsoft.Json.Linq.JObject.Parse(json);
            var state = jObject["state"]?.ToString();
            
            if (state != null)
            {
                TextHelper.AssertTextContains(state, "Cancelled", "Simulation state in API is not Cancelled.");
            }
            else
            {
                throw new Exception("Missing Simulation state");
            }
        }
    }

    [Test]
    public void SimulationItem_Should_Navigate_To_IterationItem()
    {
        EnsureSimulationExists();
        if (String.IsNullOrEmpty(SimulationId))
            throw new Exception("Init not completed? Init() - SimulationItem_Should_Navigate_To_IterationItem");

        // The simulation is already loaded and completed in Init()

        // Click on the first iteration
        var iterationPage = _simulationItemPage.GoToIteration(0);
        
        // Assert that we navigated to the iteration page
        iterationPage.AssertIfDisplayed();
    }
}
