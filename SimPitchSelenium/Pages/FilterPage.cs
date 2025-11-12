using System;
using OpenQA.Selenium;

namespace SimPitchSelenium.Pages;

public class FilterPage : BasePage
{
    protected By By_SortingOrder_Button;
    public FilterPage(IWebDriver webDriver) : base(driver: webDriver)
    {
        By_SortingOrder_Button = GetBySeleniumId("button-order");

    }

    internal void SetSortingMethod(string sortingName, string condition = null)
    {
        WaitForText("Scoreboard:");
        SelectFromDropdown(GetBySeleniumId($"sorting-select"), sortingName, "SetSortingMethod");
        if (!string.IsNullOrEmpty(condition))
        {
            SelectFromDropdown(GetBySeleniumId($"dynamic-select"), condition, "SetSortingMethod-condition");
        }
    }

    internal void ChangeSortingOrder()
    {
        Click(By_SortingOrder_Button);
    }
}
