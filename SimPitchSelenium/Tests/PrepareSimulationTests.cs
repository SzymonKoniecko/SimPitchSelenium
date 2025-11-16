using System;
using SimPitchSelenium.Models;
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
        PrepareSimulationModel model = new()
        {
            // Add leagueRound (in future)
            isSeason2022_2023 = true,
            isSeason2025_2026 = true,

            Title = "AB",
            League = "betclic-1-liga",
            NumberOfIterations = 999,
            CreateScoreboards = true,
        };

        _prepareSimulationPage.PrepareSimulationByModel(model, startSimulation: false);
        _prepareSimulationPage.AssertPrepareSimulationByModel(model);

        _prepareSimulationPage.ClickResetForm();
        _prepareSimulationPage.AssertSelectedSeasonYears();
    }

    [Test]
    public void PrepareSimulation_Should_Create_BASE_Simulation()
    {
        PrepareSimulationModel model = new()
        {
            // Add leagueRound (in future)
            isSeason2022_2023 = true,
            isSeason2025_2026 = true,

            Title = $"Selenium {TestContext.CurrentContext.Test.Name} - {rand.Next(0, 99)}",
            League = "pko-bp-ekstraklasa",
            NumberOfIterations = 2,
            CreateScoreboards = false,
            Seed = 1000
        };

        _prepareSimulationPage.PrepareSimulationByModel(model, true);

        string simulationId = _prepareSimulationPage.GetSimulationId();


        SimulationItemPage simulationItemPage = _prepareSimulationPage.GoToSimulationItemPage();
        simulationItemPage.AssertIfDisplayed(simulationId);
        simulationItemPage.AssertSimulationParams(
            "League: PKO BP Ekstraklasa",
            "Iterations: 2",
            "Seasons: 2022/2023, 2025/2026",
            "Seed: 1000",
            "Games to reach trust: 15",
            "Confidence level: 1.05",
            "Noise factor: 0.12",
            "Home Advantage: 1.05",
            "Created scoreboards during the simulation? -> true");

        AllSimulationsPage allSimulationsPage = simulationItemPage.NavBar.GoToAllSimulationsPage();
        allSimulationsPage.AssertSimulationDetails(0,
            model.Title,
            "State: Completed",
            "League: PKO BP Ekstraklasa",
            $"Created: {TextHelper.GetFormattedCurrentDate()}",
            "Completed iterations: 2 / 2",
            "Percentage: 100%",
            "Scoreboards are created during the simulation? -> true <-");
        allSimulationsPage.AssertClosedSimulationDetails(0,
            "Iterations:\n2\nSeed:\n1000\nGames to reach trust:\n15\nConfidence level:\n1.05\nNoise factor:"+
            "\n0.12\nHome advantage:\n1.05\nSeason years used in simulation:\n2022/2023\n2025/2026");
    }

    [Test]
    public void PrepareSimulation_Should_Create_ALL_Simulation()
    {
        PrepareSimulationModel model = new()
        {
            // Add leagueRound (in future)
            isSeason2022_2023 = true,
            isSeason2023_2024 = true,
            isSeason2024_2025 = true,
            isSeason2025_2026 = true,

            Title = $"Selenium {TestContext.CurrentContext.Test.Name} - {rand.Next(0, 99)}",
            League = "pko-bp-ekstraklasa",
            NumberOfIterations = 2,
            CreateScoreboards = true,
            Seed = 1000,
            GamesToReachTrust = 18,
            ConfidenceLevel = 0.98f,
            NoiseFactor = 0.13f,
            HomeAdvantage = 1.06f
        };

        _prepareSimulationPage.PrepareSimulationByModel(model, true);

        string simulationId = _prepareSimulationPage.GetSimulationId();


        SimulationItemPage simulationItemPage = _prepareSimulationPage.GoToSimulationItemPage();
        simulationItemPage.AssertIfDisplayed(simulationId);
        simulationItemPage.AssertSimulationParams(
            "League: PKO BP Ekstraklasa",
            "Iterations: 2",
            "Seasons: 2022/2023, 2023/2024, 2024/2025, 2025/2026",
            "Seed: 1000",
            "Games to reach trust: 18",
            "Confidence level: 0.98",
            "Noise factor: 0.13",
            "Home Advantage: 1.06",
            "Created scoreboards during the simulation? -> true");

        AllSimulationsPage allSimulationsPage = simulationItemPage.NavBar.GoToAllSimulationsPage();
        allSimulationsPage.AssertSimulationDetails(0,
            model.Title,
            "State: Completed",
            "League: PKO BP Ekstraklasa",
            $"Created: {TextHelper.GetFormattedCurrentDate()}",
            "Completed iterations: 2 / 2",
            "Percentage: 100%",
            "Scoreboards are created during the simulation? -> true <-");
        allSimulationsPage.AssertClosedSimulationDetails(0,
            "Iterations:\n2\nSeed:\n1000\nGames to reach trust:\n18\nConfidence level:\n0.98\nNoise factor:"+
            "\n0.13\nHome advantage:\n1.06\nSeason years used in simulation:\n2022/2023\n2023/2024\n2024/2025\n2025/2026");
    }
}
