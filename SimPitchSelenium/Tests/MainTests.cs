using System;
using NUnit.Framework.Internal;
using SimPitchSelenium.Pages;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Tests;

[TestFixture]
[Timeout(30000)]
public class MainTests : BaseTest
{
    private MainPage _mainPage;

    [SetUp]
    public void Init()
    {
        _mainPage = new MainPage(Driver).Open();
    }

    [Test]
    public void Main_Should_See_NavBar()
    {
        _mainPage.NavBar.AssertIfNavBarDisplayed();
    }

    [Test]
    public void Main_Should_Display_Sections_Text()
    {
        TextHelper.AssertTextContains(
            _mainPage.GetSimPitchSectionText(),
            "Match and league round simulation based on Poisson and Gamma models"
        );

        AssertHelper.IsTrue(_mainPage.IsElementPresentAndEnabled(_mainPage.By_PrepareSimulation_Btn), "Prepare simulation button is not displayed!", TestContext.CurrentContext.Test.Name);

        TextHelper.AssertTextEquals(
            _mainPage.GetSystemGoalSectionText(),
            "🎯 System Goal\n" +
            "Analysis of team history (matches, goals scored and conceded)\n" +
            "Assessment of offensive and defensive strength\n" +
            "Consideration of the random nature of results\n" +
            "Simulation of goal distribution using probabilistic models"
        );

        TextHelper.AssertTextEquals(
            _mainPage.GetHeatMapSectionText(),
            "Heat Map"
        );

        TextHelper.AssertTextEquals(
            _mainPage.GetPosteriorSectionText(),
            "Posterior (Gamma) (not developed)"
        );
    }
}
