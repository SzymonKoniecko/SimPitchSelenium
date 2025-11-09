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

    public void AssertIfNavBarDisplayed()
    {
        IsElementDisplayed(By_Logo);
        IsElementDisplayed(By_Nav_Home_Btn);
        IsElementDisplayed(By_Nav_Prepare_Sim_Btn);
        IsElementDisplayed(By_Nav_All_Sim_Btn);
        IsElementDisplayed(By_Nav_About_Btn);
    }

    protected MainPage GoToMainPage()
    {
        Click(By_Nav_Home_Btn);
        return new MainPage(Driver);
    }
}
