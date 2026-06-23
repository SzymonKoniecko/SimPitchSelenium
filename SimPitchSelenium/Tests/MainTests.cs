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
        var heroText = _mainPage.GetSimPitchSectionText();
        TextHelper.AssertTextContains(heroText, "SimPitch");
        TextHelper.AssertTextContains(heroText, "Advanced football match and league simulation based on Bayesian models.");
        TextHelper.AssertTextContains(heroText, "Prepare a new simulation");

        AssertHelper.IsTrue(_mainPage.IsElementPresentAndEnabled(_mainPage.By_PrepareSimulation_Btn), "Prepare simulation button is not displayed!", TestContext.CurrentContext.Test.Name);

        TextHelper.AssertTextContains(
            _mainPage.GetSimulationModelsSectionText(),
            "Simulation Models"
        );

        TextHelper.AssertTextContains(
            _mainPage.GetVisualizationsSectionText(),
            "Heat Map"
        );
    }
}
