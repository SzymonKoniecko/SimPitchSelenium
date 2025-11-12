using System;
using System.Security.Cryptography;
using SimPitchSelenium.Pages;

namespace SimPitchSelenium.Tests;

[TestFixture]
[Timeout(30000)]
public class AllSimulationsTests : BaseTest
{
    private AllSimulationsPage _allSimulationsPage;
    [SetUp]
    public void Init()
    {
        var mainPage = new MainPage(Driver).Open();
        _allSimulationsPage = mainPage.NavBar.GoToAllSimulationsPage();

        var totalCount = _allSimulationsPage.Pagination.GetTotalCount();
        if (totalCount < 6)
        {
            var prepPage = _allSimulationsPage.NavBar.GoToPrepareSimualationPage();
            for (int i = totalCount; i < 7; i++) // add simulations to have more than 6 for pagination
            {
                prepPage.StartAnySimulation();
            }
            prepPage.NavBar.GoToAllSimulationsPage();
        }

        if (_allSimulationsPage.Pagination.GetTotalCount() < 6)
        {
            throw new Exception("Someting went wrong with adding simulations for pagging.");
        }

        _allSimulationsPage.RefreshPage();
    }

    [Test]
    public void AllSimulations_Assert_Filter()
    {
        _allSimulationsPage = _allSimulationsPage.NavBar.GoToAllSimulationsPage();

        // going to the latest page
        _allSimulationsPage.Pagination.CheckIfItsFirstPage();
        _allSimulationsPage.AssertSimulationCount(5);

        _allSimulationsPage.Filter.SetSortingMethod("league", "pko-bp-ekstraklasa");
        _allSimulationsPage.AssertTextDisplayed("PKO BP Ekstraklasa");

        _allSimulationsPage.Filter.SetSortingMethod("execution-time");
        _allSimulationsPage.AssertSimulationCount(5);
        _allSimulationsPage.Filter.ChangeSortingOrder();
        _allSimulationsPage.AssertSimulationCount(5);
        _allSimulationsPage.AssertTextDisplayed("Toggle Ascending");
    }

    [Test]
    public void AllSimulations_Assert_Pagination()
    {
        _allSimulationsPage = _allSimulationsPage.NavBar.GoToAllSimulationsPage();

        // going to the latest page
        _allSimulationsPage.Pagination.CheckIfItsFirstPage();
        _allSimulationsPage.AssertSimulationCount(5);
        _allSimulationsPage.Pagination.GoToLatestPage();
        _allSimulationsPage.Pagination.SelectPageSize("10");
        _allSimulationsPage.Pagination.GoToLatestPage();
    }
}
