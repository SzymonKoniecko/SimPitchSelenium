using System;
using SimPitchSelenium.Pages;

namespace SimPitchSelenium.Tests;

[TestFixture]
public class IterationResultTests : BaseTest
{
    private IterationResultPage _iterationResultPage;
    private MainPage _mainPage;
    private string SimulationId;
    private static string StaticSimulationId = String.Empty;
    private Random rand;

    [SetUp]
    public void Init()
    {
        rand = new();
        _mainPage = new MainPage(Driver).Open();
    }

    [Test]
    public void IterationResult_Simple_Navigation_Test()
    {
        // Navigate directly to a fake iteration URL to test the page skeleton without invoking the backend
        var fakeSimulationId = Guid.NewGuid().ToString();
        Driver.Navigate().GoToUrl($"{BaseUrl}/simulation/{fakeSimulationId}/iteration/0");
        
        // At this point we just want to ensure we didn't crash.
        // We can create the page object and maybe check its title, but it's going to display 404 or empty state.
        Assert.DoesNotThrow(() => {
            _iterationResultPage = new IterationResultPage(Driver);
        });
    }

    private void EnsureSimulationExists()
    {
        if (String.IsNullOrEmpty(StaticSimulationId))
        {
            var prepPage = _mainPage.GoToPrepareSimulationViaSectionButton();
            prepPage.StartAnySimulation(1);
            StaticSimulationId = prepPage.GetSimulationId();
            _createdSimulationIds.Add(StaticSimulationId);
            var simulationItemPage = prepPage.GoToSimulationItemPage();
            simulationItemPage.AssertIfDisplayed(StaticSimulationId);
            simulationItemPage.WaitForCompletedSimulation();
            _iterationResultPage = simulationItemPage.GoToIteration(0);
        }
        else
        {
            var simulationItemPage = _mainPage.GoToSimulationItemPageViaUrl(StaticSimulationId);
            simulationItemPage.AssertIfDisplayed(StaticSimulationId);
            simulationItemPage.WaitForCompletedSimulation();
            _iterationResultPage = simulationItemPage.GoToIteration(0);
        }
        SimulationId = StaticSimulationId;
    }



    [Test]
    public void IterationResult_Assert_RadarChart()
    {
        EnsureSimulationExists();
        
        _iterationResultPage.AssertIfDisplayed();
        _iterationResultPage.AssertRadarChartDisplayed();
    }
}
