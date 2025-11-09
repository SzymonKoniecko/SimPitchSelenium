using System;
using OpenQA.Selenium;

namespace SimPitchSelenium.Pages;

public class NavBarPage : BasePage
{
    protected By By_Logo;
    protected By By_Nav_Home_Btn;
    protected By By_Nav_Prepare_Sim_Btn;
    protected By By_Nav_All_Sim_Btn;
    protected By By_Nav_About_Btn;
    public NavBarPage(IWebDriver webDriver) : base(driver: webDriver)
    {
        By_Logo = GetByClass("logo");
        By_Nav_Home_Btn = GetBySeleniumId("home-nav");
        By_Nav_Prepare_Sim_Btn = GetBySeleniumId("prepareSimulation-nav");
        By_Nav_All_Sim_Btn = GetBySeleniumId("simulations-nav");
        By_Nav_About_Btn = GetBySeleniumId("about-nav");
    }

    internal MainPage GoToMainPage()
    {
        Click(By_Nav_Home_Btn);
        return new MainPage(Driver);
    }

    internal PrepareSimulationPage GoToPrepareSimualationPage()
    {
        Click(By_Nav_Prepare_Sim_Btn);
        return new PrepareSimulationPage(Driver);
    }

    internal AllSimulationsPage GoToAllSimulationsPagePage()
    {
        Click(By_Nav_All_Sim_Btn);
        return new AllSimulationsPage(Driver);
    }

    internal AboutPage GoToAboutPage()
    {
        Click(By_Nav_About_Btn);
        return new AboutPage(Driver);
    }

    internal void AssertIfNavBarDisplayed()
    {
        IsElementDisplayed(By_Logo);
        IsElementDisplayed(By_Nav_Home_Btn);
        IsElementDisplayed(By_Nav_Prepare_Sim_Btn);
        IsElementDisplayed(By_Nav_All_Sim_Btn);
        IsElementDisplayed(By_Nav_About_Btn);
    }

    internal string GetLogoText()
    {
        return GetElementText(By_Logo);
    }

    internal string GetHomeBtnText()
    {
        return GetElementText(By_Nav_Home_Btn);
    }

    internal string GetPrepareSimulationBtnText()
    {
        return GetElementText(By_Nav_Prepare_Sim_Btn);
    }

    internal string GetAllSimulationsBtnText()
    {
        return GetElementText(By_Nav_All_Sim_Btn);
    }

    internal string GetAboutBtnText()
    {
        return GetElementText(By_Nav_About_Btn);
    }
}
