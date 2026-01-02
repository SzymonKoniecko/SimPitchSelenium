using System;
using SimPitchSelenium.Pages;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Tests;

[TestFixture]
public class NavigationTests : BaseTest
{
    private MainPage _mainPage;

    [SetUp]
    public void Init()
    {
        _mainPage = new MainPage(Driver).Open();
    }

    [Test]
    public void Nav_Should_Display_NavBar_Text()
    {
        _mainPage.NavBar.AssertIfNavBarDisplayed();

        TextHelper.AssertTextEquals(
            _mainPage.NavBar.GetLogoText(),
            "SimPitch ⚽"
        );
        TextHelper.AssertTextEquals(
            _mainPage.NavBar.GetHomeBtnText(),
            "Home"
        );
        TextHelper.AssertTextEquals(
            _mainPage.NavBar.GetPrepareSimulationBtnText(),
            "Prepare a new simulation"
        );
        TextHelper.AssertTextEquals(
            _mainPage.NavBar.GetAllSimulationsBtnText(),
            "All simulations"
        );
        TextHelper.AssertTextEquals(
            _mainPage.NavBar.GetAboutBtnText(),
            "About"
        );
    }

    [Test]
    public void Nav_Should_Each_Element_Work()
    {
        _mainPage.NavBar.AssertIfNavBarDisplayed();
        
        var prepPage = _mainPage.NavBar.GoToPrepareSimualationPage();
        prepPage.AssertIfDisplayed();

        var allSimulationsPage = _mainPage.NavBar.GoToAllSimulationsPage();
        allSimulationsPage.AssertIfDisplayed();

        var aboutPage = _mainPage.NavBar.GoToAboutPage();
        aboutPage.AssertIfDisplayed();

        var mainPage = _mainPage.NavBar.GoToMainPage();
        mainPage.AssertIfDisplayed();
    }
}