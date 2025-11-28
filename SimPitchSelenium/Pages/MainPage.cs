using System;
using OpenQA.Selenium;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Pages;

public class MainPage : BasePage
{
    protected By By_SimPitchSection;
    protected By By_SimulationModelsSection;
    protected By By_VisualizationsSection;
    protected By By_PostieriorSection;
    internal By By_PrepareSimulation_Btn;

    public MainPage(IWebDriver webDriver) : base(driver: webDriver)
    {
        By_SimPitchSection = GetByClass("hero");
        By_SimulationModelsSection = GetBySeleniumId("simulation-models");

        By_VisualizationsSection = GetBySeleniumId("visualizations");
        By_PostieriorSection = GetBySeleniumId("using-example-2");

        By_PrepareSimulation_Btn = GetBySeleniumId("prepareSimulation");
    }

    public MainPage Open()
    {
        Driver.Navigate().GoToUrl(BaseUrl);

        return this;
    }

    public PrepareSimulationPage GoToPrepareSimulationViaSectionButton()
    {
        Click(By_PrepareSimulation_Btn);
        return new PrepareSimulationPage(Driver);
    }
    
    internal void AssertIfDisplayed()
    {
        AssertHelper.IsTrue(IsElementDisplayed(By_SimulationModelsSection), "Page is not loaded", "MainPage");
    }

    public string GetSimPitchSectionText()
    {
        return GetElementText(By_SimPitchSection);
    }

    public string GetSimulationModelsSectionText()
    {
        return GetElementText(By_SimulationModelsSection);
    }

    public string GetVisualizationsSectionText()
    {
        return GetElementText(By_VisualizationsSection);
    }
    
    public string GetPosteriorSectionText()
    {
        return GetElementText(By_PostieriorSection);
    }
}
