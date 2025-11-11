using System;
using OpenQA.Selenium;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Pages;

public class SimulationItemPage : BasePage
{
    protected By By_Title;
    internal By By_Simulation_Params_Details;
    internal By By_Simulation_Params_Details_List;
    public SimulationItemPage(IWebDriver webDriver) : base(driver: webDriver)
    {
        By_Title = GetBySeleniumId("title-simulation-item");
        By_Simulation_Params_Details = GetBySeleniumId("sim-params-details");
        By_Simulation_Params_Details_List = GetBySeleniumId("sim-params-details-list");
    }

    internal void AssertIfDisplayed(string simulationId)
    {
        AssertHelper.IsTrue(IsElementDisplayed(By_Title), "Page is not loaded", "SimulationItemPage");
        AssertUrlContains(simulationId);
    }

    internal void AssertSimulationParams(params string[] expectedText)
    {
        Click(By_Simulation_Params_Details);

        var text = GetElementText(By_Simulation_Params_Details_List);
        foreach (var expText in expectedText)
        {
            TextHelper.AssertTextContains(text, expText, "AssertSimulationParams");
        }
    }
}
