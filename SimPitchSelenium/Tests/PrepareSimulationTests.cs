using System;
using SimPitchSelenium.Pages;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Tests;

[TestFixture]
[Timeout(30000)]
public class PrepareSimulationTests : BaseTest
{
    private PrepareSimulationPage _prepareSimulationPage;
    private Random rand = new();
    [SetUp]
    public void Init()
    {
        var mainPage = new MainPage(Driver).Open();
        _prepareSimulationPage = mainPage.GoToPrepareSimulationViaSectionButton();
    }

    [Test]
    public void PrepareSimulation_Should_Display_Validation_Errors()
    {
        _prepareSimulationPage.ClickStartSimulation();
        _prepareSimulationPage.AssertValidationErrors(
            "You must select a league", "Select at least one season");
        _prepareSimulationPage.SelectTitle("AB");
        _prepareSimulationPage.ClickStartSimulation();
        _prepareSimulationPage.AssertValidationErrors(
            "Title must have at least 3 characters",
            "You must select a league",
            "Select at least one season");
    }

    [Test]
    public void PrepareSimulation_Should_Reset_Form_To_Default_State()
    {
        _prepareSimulationPage.SelectSeasonYears(
            isSeason2022_2023: true,
            isSeason2025_2026: true
        );
        _prepareSimulationPage.SelectTitle("AB");
        _prepareSimulationPage.SelectLeague("betclic-1-liga");
        _prepareSimulationPage.SelectNumberOfIterations("999");
        _prepareSimulationPage.SelectCreateScoreboardsCheckbox();
        // Add leagueRound (in future)


        _prepareSimulationPage.AssertSelectedSeasonYears(
            isSeason2022_2023: true,
            isSeason2025_2026: true
        );
        _prepareSimulationPage.AssertTitle("AB");
        _prepareSimulationPage.AssertLeague("Betclic 1 Liga");
        _prepareSimulationPage.AssertNumberOfIterations("999");
        _prepareSimulationPage.AssertCreateScoreboardsCheckbox(isSelected: false);

        _prepareSimulationPage.ClickResetForm();
        _prepareSimulationPage.AssertSelectedSeasonYears();
    }

    [Test]
    public void PrepareSimulation_Should_Create_Simulation()
    {
        string title = $"Selenium {TestContext.CurrentContext.Test.Name} - {rand.Next(0, 99)}";

        _prepareSimulationPage.SelectSeasonYears(
            isSeason2022_2023: true,
            isSeason2025_2026: true
        );
        _prepareSimulationPage.SelectTitle(title);
        _prepareSimulationPage.SelectLeague("pko-bp-ekstraklasa");
        _prepareSimulationPage.SelectNumberOfIterations("2");

        _prepareSimulationPage.ClickStartSimulation();

        _prepareSimulationPage.AssertStartedSimulationMessage();
        string simulationId = _prepareSimulationPage.GetSimulationId();

        Thread.Sleep(1000); // to give a time to complete the simulation

        SimulationItemPage simulationItemPage = _prepareSimulationPage.GoToSimulationItemPage();
        simulationItemPage.AssertIfDisplayed(simulationId);
        simulationItemPage.AssertSimulationParams(
            "League: PKO BP Ekstraklasa",
            "Iterations: 2",
            "Seasons: 2022/2023, 2025/2026",
            "Created scoreboards during the simulation? -> true");

        AllSimulationsPage allSimulationsPage = simulationItemPage.NavBar.GoToAllSimulationsPage();
        allSimulationsPage.AssertSimulationDetails(0,
            title,
            "State: Completed",
            "League: PKO BP Ekstraklasa",
            $"Created: {TextHelper.GetFormattedCurrentDate()}",
            "Completed iterations: 2 / 2",
            "Percentage: 100%",
            "Scoreboards are created during the simulation? -> true <-");
        allSimulationsPage.AssertClosedSimulationDetails(0,
            "Iterations:\n2\nSeason years used in simulation:\n2022/2023\n2025/2026");
    }
}
