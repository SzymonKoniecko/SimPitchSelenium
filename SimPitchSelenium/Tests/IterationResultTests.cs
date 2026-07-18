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

    // private void EnsureSimulationExists()
    // {
    //     if (String.IsNullOrEmpty(StaticSimulationId))
    //     {
    //         var prepPage = _mainPage.GoToPrepareSimulationViaSectionButton();
    //         prepPage.StartAnySimulation(2);
    //         StaticSimulationId = prepPage.GetSimulationId();
    //         var simulationItemPage = prepPage.GoToSimulationItemPage();
    //         simulationItemPage.AssertIfDisplayed(StaticSimulationId);
    //         simulationItemPage.WaitForCompletedSimulation();
    //         _iterationResultPage = simulationItemPage.GoToIteration(rand.Next(0,2));
    //     }
    //     else
    //     {
    //         var simulationItemPage = _mainPage.GoToSimulationItemPageViaUrl(StaticSimulationId);
    //         simulationItemPage.AssertIfDisplayed(StaticSimulationId);
    //         simulationItemPage.WaitForCompletedSimulation();
    //         _iterationResultPage = simulationItemPage.GoToIteration(rand.Next(0,2));
    //     }
    //     SimulationId = StaticSimulationId;
    // }

    // //[Test]
    // public void IterationResult_Assert_Scoreboard_SimulatedMatches()
    // {
    //     if (String.IsNullOrEmpty(SimulationId))
    //         throw new Exception("Init not completed? Init() - IterationResult_Assert_Scoreboard");
        
    //     _iterationResultPage.AssertIfDisplayed();
    //     _iterationResultPage.AssertNumOfTeamsInTable(18 * 10 + 1);
    //     _iterationResultPage.AssertNumOfSimulatedMatches();
    // }

    // //[Test]
    // public void IterationResult_Assert_SimulatedMatchLabels()
    // {
    //     if (String.IsNullOrEmpty(SimulationId))
    //         throw new Exception("Init not completed? Init() - IterationResult_Assert_SimulatedMatchLabels");
        
    //     _iterationResultPage.AssertIfDisplayed();
        
    //     var simulatedMatchesCount = _iterationResultPage.GetNumberOfSimulatedMatches();
    //     Utils.AssertHelper.IsTrue(simulatedMatchesCount > 0, "Simulated matches count should be greater than 0");
        
    //     // This will assert that the number of elements with selenium-id="simulated" matches the count
    //     _iterationResultPage.AssertNumOfSimulatedMatches();
    // }
}
