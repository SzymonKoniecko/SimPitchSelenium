using System;
using OpenQA.Selenium;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Pages;

public class NotFoundPage : BasePage
{
    private By By_Heading = By.TagName("h1");

    public NotFoundPage(IWebDriver driver) : base(driver)
    {
    }

    public void AssertIfDisplayed()
    {
        var heading = WaitForElement(By_Heading);
        AssertHelper.AreEqual("NotFound", heading.Text, "Heading text should be NotFound", "AssertIfDisplayed - NotFoundPage");
    }
}
