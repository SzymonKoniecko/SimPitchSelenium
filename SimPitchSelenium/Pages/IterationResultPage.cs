using System;
using OpenQA.Selenium;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Pages;

public class IterationResultPage : BasePage
{
    protected By By_Table_Scoreboard;
    protected By By_Match;
    protected By By_Li_SimulatedMatches;
    public IterationResultPage(IWebDriver webDriver) : base(webDriver)
    {
        By_Table_Scoreboard = GetBySeleniumId("scoreboard_complete_details");
        By_Match = GetBySeleniumId("match");
        By_Li_SimulatedMatches = GetBySeleniumId("number-simulated-matches");
    }

    public int GetNumberOfSimulatedMatches()
    {
        var value = GetElementText(By_Li_SimulatedMatches)
                        .Split("Number of simulated matches: ")[1]
                        .ToString()
                        .Trim();
        return int.Parse(value);
    }

    public void AssertIfDisplayed()
    {
        WaitForElement(By_Table_Scoreboard);
    }

    public void AssertNumOfTeamsInTable(int expectedCount)
    {
        AssertHelper.AreEqual(expectedCount, GetTableCellCount(By_Table_Scoreboard, "AssertNumOfTeamsInTable"), "AssertNumOfTeamsInTable");
    }

    public void AssertNumOfSimulatedMatches()
    {
        AssertHelper.AreEqual(
            GetNumberOfSimulatedMatches(), 
            GetElementCount(By_Match), 
            "AssertNumOfSimulatedMatches"
        );
    }
}
