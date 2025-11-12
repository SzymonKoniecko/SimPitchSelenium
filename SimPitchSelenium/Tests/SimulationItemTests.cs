using System;
using SimPitchSelenium.Pages;

namespace SimPitchSelenium.Tests;

[TestFixture]
[Timeout(30000)]
public class SimulationItemTests : BaseTest
{
    private SimulationItemPage _simulationItemPage;
    private MainPage _mainPage;
    private string SimulationId = String.Empty;

    [SetUp]
    public void Init()
    {
        _mainPage = new MainPage(Driver).Open();
        var prepPage = _mainPage.GoToPrepareSimulationViaSectionButton();
        prepPage.StartAnySimulation("50");
        SimulationId = prepPage.GetSimulationId();
        _simulationItemPage = prepPage.GoToSimulationItemPageViaUrl(SimulationId);
        _simulationItemPage.AssertIfDisplayed(SimulationId);
    }

    [Test]
    public void SimulationItem_Assert_Status_And_Refresh()
    {
        // Preparation (large iteration number to cover changes in statuses)
        _mainPage = new MainPage(Driver).Open();
        var prepPage = _mainPage.GoToPrepareSimulationViaSectionButton();
        prepPage.StartAnySimulation("200");
        SimulationId = prepPage.GetSimulationId();
        _simulationItemPage = prepPage.GoToSimulationItemPageViaUrl(SimulationId);
        _simulationItemPage.AssertIfDisplayed(SimulationId);

        _simulationItemPage.AssertSimulationState("Running");
        _simulationItemPage.AssertIfIterationsPercentageIsNot100();
        _simulationItemPage.WaitForCompletedSimulation();
    }

    [Test]
    public void SimulatonItem_Assert_Pagination_Filter()
    {
        if (String.IsNullOrEmpty(SimulationId))
            throw new Exception("Init not completed? Init() - SimulatonItem_Assert_Pagination_Filter");

        _simulationItemPage = _mainPage.GoToSimulationItemPageViaUrl(SimulationId);
        _simulationItemPage.AssertIfDisplayed(SimulationId);
        _simulationItemPage.WaitForCompletedSimulation();

        // Pagination
        _simulationItemPage.Pagination.CheckIfItsFirstPage();
        _simulationItemPage.AssertIterationCount(10);
        _simulationItemPage.Pagination.GoToLatestPage();
        _simulationItemPage.Pagination.SelectPageSize("5");
        _simulationItemPage.AssertIterationCount(5);

        // Filter
        _simulationItemPage.Pagination.CheckIfItsFirstPage();
        _simulationItemPage.Pagination.SelectPageSize("10");
        _simulationItemPage.Filter.SetSortingMethod("order-by-iteration");
        _simulationItemPage.AssertIterationCount(10);
        _simulationItemPage.Filter.ChangeSortingOrder();
        _simulationItemPage.AssertIterationCount(10);
        _simulationItemPage.AssertTextDisplayed("Toggle Ascending");

        _simulationItemPage.Filter.SetSortingMethod("team", "jagiellonia-bialystok");
        _simulationItemPage.AssertTextDisplayed("Scoreboard:");
    }

    [Test]
    public void SimulationItem_Assert_HeatMap()
    {
        if (String.IsNullOrEmpty(SimulationId))
            throw new Exception("Init not completed? Init() - SimulatonItem_Assert_Pagination_Filter");
        _simulationItemPage = _mainPage.GoToSimulationItemPageViaUrl(SimulationId);
        _simulationItemPage.AssertIfDisplayed(SimulationId);
        _simulationItemPage.WaitForCompletedSimulation();
        
        _simulationItemPage.AssertPercentSumEquals100(0, "Team row index 0");
        _simulationItemPage.AssertPercentSumEquals100(10, "Team row index 10");
    }
}
