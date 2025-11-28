using System;
using OpenQA.Selenium;

namespace SimPitchSelenium.Pages;

public class PaginationPage : BasePage
{
    protected By By_Prev_Button;
    protected By By_Next_Button;
    protected By By_TotalCount;
    public PaginationPage(IWebDriver webDriver) : base(driver: webDriver)
    {
        By_Prev_Button = GetBySeleniumId("prev-button");
        By_Next_Button = GetBySeleniumId("next-button");
        By_TotalCount = GetBySeleniumId("total-count");

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
        if (!IsButtonDisabled(By_Next_Button))
        {
            Click(By_Next_Button);
        }
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
            Thread.Sleep(500);
        }
    }

    internal void SelectPageSize(string size)
    {
        SelectFromDropdown(GetBySeleniumId($"size-select"), size, "SelectPageSize");
        Thread.Sleep(500);
    }
}
