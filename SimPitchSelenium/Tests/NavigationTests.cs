using System;

namespace SimPitchSelenium.Tests;

[TestFixture]
public class NavigationTests : BaseTest
{
    [Test]
    public void Should_Open_BaseUrl_From_Config()
    {
        Driver.Navigate().GoToUrl(BaseUrl);

        Assert.That(Driver.Title, Is.Not.Empty,
            $"The page '{BaseUrl}' should have a valid title.");
    }
}