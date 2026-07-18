using System;
using OpenQA.Selenium;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Pages;

public class IterationResultPage : BasePage
{
    protected By By_Table_Scoreboard;
    protected By By_Match;
    protected By By_Li_SimulatedMatches;
    protected By By_SimulatedMatch_label;
    protected By By_Scatter_Details;
    protected By By_Reset_Scatter_Button;
    protected By By_Team_Radar_Details;

    public IterationResultPage(IWebDriver webDriver) : base(webDriver)
    {
        By_Table_Scoreboard = GetBySeleniumId("scoreboard_complete_details");
        By_Match = GetBySeleniumId("match");
        By_Li_SimulatedMatches = GetBySeleniumId("number-simulated-matches");
        By_SimulatedMatch_label = GetBySeleniumId("simulated");
        By_Scatter_Details = GetBySeleniumId("scatter-details");
        By_Reset_Scatter_Button = GetBySeleniumId("reset-scatter-button");
        By_Team_Radar_Details = GetBySeleniumId("team-radar-details");
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
            GetElementCount(By_SimulatedMatch_label), 
            "AssertNumOfSimulatedMatches"
        );
    }

    public void AssertScatterPlotDisplayedAndReset()
    {
        var scatterDetails = WaitForElement(By_Scatter_Details);
        AssertHelper.IsTrue(scatterDetails.Displayed, "Scatter Plot details should be displayed", "AssertScatterPlot");

        // The <details> element might need to be opened manually if not open
        // But the button is inside it. We try to click the summary or just click the reset button if visible.
        var resetButton = WaitForElement(By_Reset_Scatter_Button);
        resetButton.Click();
    }

    public void AssertRadarChartDisplayed()
    {
        // First open the first match round details to expose the match list
        var matchRoundSummary = WaitForElement(By.CssSelector(".match-details summary"));
        matchRoundSummary.Click();

        // Then open the team strengths details inside the first match
        var teamStrengthSummary = WaitForElement(By.XPath("//span[text()='Teams strengths']/.."));
        teamStrengthSummary.Click();

        var radarDetails = WaitForElement(By_Team_Radar_Details);
        AssertHelper.IsTrue(radarDetails.Displayed, "Radar Chart details should be displayed", "AssertRadarChart");
    }
}
