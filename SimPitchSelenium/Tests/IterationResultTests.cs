using System;
using SimPitchSelenium.Pages;

namespace SimPitchSelenium.Tests;

[TestFixture]
[Timeout(30000)]
public class IterationResultTests : BaseTest
{
    private IterationResultPage _iterationResultPage;
    private MainPage _mainPage;
    private string SimulationId;
    private Random rand;
    [SetUp]
    public void Init()
    {
        rand = new();
        _mainPage = new MainPage(Driver).Open();
        var prepPage = _mainPage.GoToPrepareSimulationViaSectionButton();
        prepPage.StartAnySimulation("2");
        SimulationId = prepPage.GetSimulationId();
        var simulationItemPage = prepPage.GoToSimulationItemPageViaUrl(SimulationId);
        simulationItemPage.AssertIfDisplayed(SimulationId);
        _iterationResultPage = simulationItemPage.GoToIteration(rand.Next(0,2));
        // go to first noticed iterationResult
    }

    [Test]
    public void IterationResult_Assert_Scoreboard_SimulatedMatches()
    {
        if (String.IsNullOrEmpty(SimulationId))
            throw new Exception("Init not completed? Init() - IterationResult_Assert_Scoreboard");
        
        _iterationResultPage.AssertIfDisplayed();
        _iterationResultPage.AssertNumOfTeamsInTable(18 * 10);
        _iterationResultPage.AssertNumOfSimulatedMatches();
    }
}
