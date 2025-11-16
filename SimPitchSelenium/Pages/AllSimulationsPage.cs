using System;
using OpenQA.Selenium;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Pages;

public class AllSimulationsPage : BasePage
{
    internal PaginationPage Pagination;
    internal FilterPage Filter;
    protected By By_Title;
    protected By By_SimulationClass;
    protected By By_Simulation_Details(int index) => GetBySeleniumId($"title-details-{index}");
    protected By By_Simulation_Details_Close(int index) => GetBySeleniumId($"title-details-close-{index}");
    public AllSimulationsPage(IWebDriver webDriver) : base(driver: webDriver)
    {
        By_Title = GetBySeleniumId("title-all-simulations");
        By_SimulationClass = GetByClass("simulation");

        Pagination = new PaginationPage(Driver);
        Filter = new FilterPage(Driver);
    }

    internal void RefreshPage()
    {
        Click(base.By_Button_Primary);
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
            TextHelper.AssertTextContains(text, expText, $"AssertSimulationDetails-Index:{index}");
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
