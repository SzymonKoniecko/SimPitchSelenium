using System;
using OpenQA.Selenium;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Pages;

public class LeagueOverviewsPage : BasePage
{
    private By By_Season_Select;
    private By By_League_Select;
    private By By_Scoreboard_Complete_Details;

    public LeagueOverviewsPage(IWebDriver driver) : base(driver)
    {
        By_Season_Select = GetBySeleniumId("season-select");
        By_League_Select = GetBySeleniumId("league-select");
        By_Scoreboard_Complete_Details = GetBySeleniumId("scoreboard_complete_details");
    }

    public void AssertIfDisplayed()
    {
        WaitForElement(By_Season_Select);
        AssertHelper.IsTrue(Driver.FindElement(By_Season_Select).Displayed, "Season select should be displayed on League Overviews page", "AssertIfDisplayed - LeagueOverviews");
    }

    public void SelectSeason(string seasonYear)
    {
        var seasonSelect = WaitForElement(By_Season_Select);
        seasonSelect.Click();
        var option = WaitForElement(GetBySeleniumId(seasonYear));
        option.Click();
    }

    public void SelectLeague(string leagueNameId)
    {
        var leagueSelect = WaitForElement(By_League_Select);
        leagueSelect.Click();
        var option = WaitForElement(GetBySeleniumId(leagueNameId));
        option.Click();
    }

    public void AssertScoreboardDisplayed()
    {
        var scoreboard = WaitForElement(By_Scoreboard_Complete_Details);
        AssertHelper.IsTrue(scoreboard.Displayed, "Scoreboard complete details should be displayed", "AssertScoreboardDisplayed");
    }

    public TeamPage GoToFirstTeam()
    {
        var firstTeamCard = WaitForElement(By.CssSelector(".team-card__link"));
        firstTeamCard.Click();
        return new TeamPage(Driver);
    }
}
