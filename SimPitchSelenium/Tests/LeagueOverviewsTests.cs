using NUnit.Framework;
using SimPitchSelenium.Pages;

namespace SimPitchSelenium.Tests;

[TestFixture]
public class LeagueOverviewsTests : BaseTest
{
    private MainPage _mainPage;
    private LeagueOverviewsPage _leagueOverviewsPage;

    [SetUp]
    public void Init()
    {
        _mainPage = new MainPage(Driver).Open();
        _leagueOverviewsPage = _mainPage.NavBar.GoToLeagueOverviewsPage();
    }

    [Test]
    public void LeagueOverviews_Should_Navigate_Via_NavBar()
    {
        _leagueOverviewsPage.AssertIfDisplayed();
    }

    [Test]
    public void LeagueOverviews_Should_Select_League_And_Display_Rounds()
    {
        _leagueOverviewsPage.AssertIfDisplayed();
        
        // Default season is Current Season. Select PKO BP Ekstraklasa.
        _leagueOverviewsPage.SelectLeague("pko-bp-ekstraklasa");
        
        // Now it should display rounds or "Loading rounds..."
        _leagueOverviewsPage.WaitForText("Started simulation by");
    }

    [Test]
    public void LeagueOverviews_Should_Display_Scoreboard_For_Past_Season()
    {
        _leagueOverviewsPage.AssertIfDisplayed();
        
        _leagueOverviewsPage.SelectSeason("2024/2025");
        _leagueOverviewsPage.SelectLeague("pko-bp-ekstraklasa");
        
        // Assert the complete details scoreboard shows up
        _leagueOverviewsPage.AssertScoreboardDisplayed();
    }
}
