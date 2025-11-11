using System;
using OpenQA.Selenium;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Pages;

public class AllSimulationsPage : BasePage
{
    protected By By_Title;
    protected By By_Prev_Button;
    protected By By_Next_Button;
    protected By By_TotalCount;
    protected By By_SimulationClass;
    protected By By_Simulation_Details(int index) => GetBySeleniumId($"title-details-{index}");
    protected By By_Simulation_Details_Close(int index) => GetBySeleniumId($"title-details-close-{index}");
    public AllSimulationsPage(IWebDriver webDriver) : base(driver: webDriver)
    {
        By_Title = GetBySeleniumId("title-all-simulations");
        By_Prev_Button = GetBySeleniumId("prev-button");
        By_Next_Button = GetBySeleniumId("next-button");
        By_TotalCount = GetBySeleniumId("total-count");
        By_SimulationClass = GetByClass("simulation");
    }

    internal int GetTotalCount()
    {
        var value = GetElementText(By_TotalCount)
            .Split("Total count: ")[1]
            .ToString()
            .Trim();

        return int.Parse(value);
    }

    internal void GoToPreviousPage()
    {
        Click(By_Prev_Button);
    }

    internal void GoToNextPage()
    {
        Click(By_Next_Button);
    }

    internal void CheckIfItsFirstPage()
    {
        AssertIfButtonDisabled(By_Prev_Button, true, "CheckIfItsFirstPage");
    }

    internal void GoToLatestPage()
    {
        while (!IsButtonDisabled(By_Next_Button))
        {
            GoToNextPage();
        }
    }

    internal void SelectPageSize(string size)
    {
        SelectFromDropdown(GetBySeleniumId($"size-select"), size, "SelectPageSize");
    }


    internal void AssertIfDisplayed()
    {
        AssertHelper.IsTrue(IsElementDisplayed(By_Title), "Page is not loaded", "AllSimulationsPage");
    }

    internal void AssertSimulationDetails(int index, params string[] expectedString)
    {
        var text = GetElementText(By_Simulation_Details(index));
        foreach (var expText in expectedString)
        {
            TextHelper.AssertTextContains(text, expText,$"AssertSimulationDetails-Index:{index}");
        }
    }

    internal void AssertClosedSimulationDetails(int index, params string[] expectedString)
    {

        Click(By_Simulation_Details_Close(index));

        var text = GetElementText(By_Simulation_Details_Close(index));
        foreach (var expText in expectedString)
        {
            TextHelper.AssertTextContains(text, expText, $"AssertSimulationDetails-Index:{index}");
        }
    }
    
    internal void AssertSimulationCount(int expectedCount)
    {
        AssertHelper.AreEqual(expectedCount, GetElementCount(By_SimulationClass));
    }
}
