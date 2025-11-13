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
    [SetUp]
    public void Init()
    {
        _mainPage = new MainPage(Driver).Open();
        var prepPage = _mainPage.GoToPrepareSimulationViaSectionButton();
        prepPage.StartAnySimulation("50");
        SimulationId = prepPage.GetSimulationId();
        var simulationItemPage = prepPage.GoToSimulationItemPageViaUrl(SimulationId);
        simulationItemPage.AssertIfDisplayed(SimulationId);
        // go to first noticed iterationResult
    }

    [Test]
    public void IterationResult_Assert_Scoreboard()
    {
        if (String.IsNullOrEmpty(SimulationId))
            throw new Exception("Init not completed? Init() - IterationResult_Assert_Scoreboard");
        
    }

    [Test]
    public void IterationResult_Assert_SimulatedMatches()
    {
        if (String.IsNullOrEmpty(SimulationId))
            throw new Exception("Init not completed? Init() - IterationResult_Assert_SimulatedMatches");
        
    }
}
