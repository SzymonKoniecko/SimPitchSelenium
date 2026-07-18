using System;
using OpenQA.Selenium;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Pages;

public class TeamPage : BasePage
{
    private By By_Title_Team;
    private By By_Team_League_Badge;
    private By By_Team_Stadium;
    private By By_Back_To_Leagues_Button;

    public TeamPage(IWebDriver driver) : base(driver)
    {
        By_Title_Team = GetBySeleniumId("title-team");
        By_Team_League_Badge = GetBySeleniumId("team-league-badge");
        By_Team_Stadium = GetBySeleniumId("team-stadium");
        By_Back_To_Leagues_Button = By.XPath("//a[contains(@class, 'button-secondary') and contains(@href, '/league')]");
    }

    public void AssertIfDisplayed()
    {
        WaitForElement(By_Title_Team);
        AssertHelper.IsTrue(Driver.FindElement(By_Title_Team).Displayed, "Team title should be displayed", "AssertIfDisplayed - TeamPage");
    }

    public void AssertDetailsDisplayed()
    {
        WaitForElement(By_Team_League_Badge);
        WaitForElement(By_Team_Stadium);

        AssertHelper.IsTrue(Driver.FindElement(By_Team_League_Badge).Displayed, "League badge should be displayed", "AssertDetailsDisplayed");
        AssertHelper.IsTrue(Driver.FindElement(By_Team_Stadium).Displayed, "Stadium card should be displayed", "AssertDetailsDisplayed");
    }

    public LeagueOverviewsPage GoBackToLeagues()
    {
        Click(By_Back_To_Leagues_Button);
        return new LeagueOverviewsPage(Driver);
    }
}
