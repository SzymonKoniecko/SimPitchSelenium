using NUnit.Framework;
using SimPitchSelenium.Pages;

namespace SimPitchSelenium.Tests;

[TestFixture]
public class TeamTests : BaseTest
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
    public void Team_Should_Display_Details()
    {
        // Navigate to League Overviews, pick a league, and click the first team card
        _leagueOverviewsPage.AssertIfDisplayed();
        _leagueOverviewsPage.SelectSeason("2023/2024");
        _leagueOverviewsPage.SelectLeague("pko-bp-ekstraklasa");
        _leagueOverviewsPage.AssertScoreboardDisplayed();

        var teamPage = _leagueOverviewsPage.GoToFirstTeam();
        
        // Assert team details
        teamPage.AssertIfDisplayed();
        teamPage.AssertDetailsDisplayed();
    }

    [Test]
    public void Team_Should_Navigate_Back()
    {
        _leagueOverviewsPage.AssertIfDisplayed();
        _leagueOverviewsPage.SelectSeason("2023/2024");
        _leagueOverviewsPage.SelectLeague("pko-bp-ekstraklasa");
        _leagueOverviewsPage.AssertScoreboardDisplayed();

        var teamPage = _leagueOverviewsPage.GoToFirstTeam();
        teamPage.AssertIfDisplayed();
        
        _leagueOverviewsPage = teamPage.NavBar.GoToLeagueOverviewsPage();
        
        _leagueOverviewsPage.AssertIfDisplayed();
    }
}
