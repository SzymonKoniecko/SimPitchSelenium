using NUnit.Framework;
using SimPitchSelenium.Pages;

namespace SimPitchSelenium.Tests;

[TestFixture]
public class NotFoundTests : BaseTest
{
    private MainPage _mainPage;

    [SetUp]
    public void Init()
    {
        _mainPage = new MainPage(Driver).Open();
    }

    [Test]
    public void NotFound_Should_Display_For_Invalid_URL()
    {
        Driver.Navigate().GoToUrl($"{SimPitchSelenium.Utils.ConfigReader.GetBaseUrl()}/this-page-does-not-exist");
        
        var notFoundPage = new NotFoundPage(Driver);
        notFoundPage.AssertIfDisplayed();
    }
}
