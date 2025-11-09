using System;
using SimPitchSelenium.Pages;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Tests;

[TestFixture]
[Timeout(30000)]
public class PrepareSimulationTests : BaseTest
{
    private PrepareSimulationPage _prepareSimulationPage;

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
        _prepareSimulationPage.SelectLeague("Betclic 1 Liga");
        _prepareSimulationPage.SelectNumberOfIterations("999");
        // Add leagueRound (in future)


        _prepareSimulationPage.AssertSelectedSeasonYears(
            isSeason2022_2023: true,
            isSeason2025_2026: true
        );
        _prepareSimulationPage.AssertTitle("AB");
        _prepareSimulationPage.AssertLeague("Betclic 1 Liga");
        _prepareSimulationPage.AssertNumberOfIterations("999");

        _prepareSimulationPage.ClickResetForm();
        _prepareSimulationPage.AssertSelectedSeasonYears();
    }
}
