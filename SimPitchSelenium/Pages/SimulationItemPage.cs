using System;
using System.Globalization;
using OpenQA.Selenium;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Pages;

public class SimulationItemPage : BasePage
{
    internal PaginationPage Pagination;
    internal FilterPage Filter;
    protected By By_Title;
    internal By By_Simulation_Params_Details;
    internal By By_Simulation_Params_Details_List;
    internal By By_Simulation_State;
    internal By By_Simulation_State_Value;
    internal By By_Simulation_Iterations;
    internal By By_Simulation_Iterations_Value;
    internal By By_Iteration;
    internal By By_HeatMap;
    public SimulationItemPage(IWebDriver webDriver) : base(driver: webDriver)
    {
        By_Title = GetBySeleniumId("title-simulation-item");
        By_Simulation_Params_Details = GetBySeleniumId("sim-params-details");
        By_Simulation_Params_Details_List = GetBySeleniumId("sim-params-details-list");
        By_Simulation_State = GetBySeleniumId("state");
        By_Simulation_State_Value = GetBySeleniumId("state-value");
        By_Simulation_Iterations = GetBySeleniumId("iterations");
        By_Simulation_Iterations_Value = GetBySeleniumId("iterations-value");
        By_Iteration = GetByClass("scoreboard-block");
        By_HeatMap = GetBySeleniumId("heatmap-details");

        Pagination = new PaginationPage(Driver);
        Filter = new FilterPage(Driver);
    }

    internal IterationResultPage GoToIteration(int index)
    {
        Click(GetBySeleniumId($"iteration-{index}"));

        return new IterationResultPage(Driver);
    }

    internal string GetCompletedIterationsString()
    {
        return GetElementText(By_Simulation_Iterations_Value).Trim();
    }

    internal string GetSimulationState()
    {
        return GetElementText(By_Simulation_State_Value).Trim();
    }

    internal void WaitForCompletedSimulation()
    {
        string state = GetSimulationState();
        while (state.Contains("Running") || state.Contains("Pending"))
        {
            Thread.Sleep(1000);
            RefreshPage();
            WaitForElement(By_Simulation_State_Value, 60);
            state = GetSimulationState();
        }
    }

    internal void RefreshPage()
    {
        Click(base.By_Button_Primary);
    }

    internal void AssertIfDisplayed(string simulationId)
    {
        AssertHelper.IsTrue(IsElementDisplayed(By_Title), "Page is not loaded", "SimulationItemPage");
        AssertUrlContains(simulationId);
    }

    internal void AssertIterationCount(int expectedCount)
    {
        Wait.Until(drv => drv.FindElements(By_Iteration).Count == expectedCount);
        AssertHelper.AreEqual(expectedCount, GetElementCount(By_Iteration));
    }

    internal void AssertIfIterationsPercentageIsNot100()
    {
        TextHelper.AssertTextNotContains(GetCompletedIterationsString(), "(100%)", "AssertIfIterationsPercentageIsNot100");
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

    internal void AssertSimulationState(string expectedString)
    {
        TextHelper.AssertTextContains(GetSimulationState(), expectedString, "AssertSimulationState");
    }
    public void AssertPercentSumEquals100(int rowNumber, string context) =>
        AssertPercentSumEquals100(GetAnyBySeleniumId($"cell-{rowNumber}-"), context);
    public void AssertPercentSumEquals100(By cellsLocator, string context = "")
    {
        WaitForElement(By_HeatMap, 15);
        if (!IsElementWithTextDisplayed("Pos 1"))
            Click(By_HeatMap);
        try
        {
            var cells = Driver.FindElements(cellsLocator);

            if (cells.Count == 0)
            {
                AssertHelper.Fail($"No elements found for locator: {cellsLocator}", context);
                return;
            }

            double total = 0.0;

            foreach (var cell in cells)
            {
                var text1 = cell.Text;
                string text = cell.Text.Trim().Replace("%", "").Trim();

                if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
                {
                    total += value;
                }
                else
                {
                    AssertHelper.Fail($"Invalid percentage format in cell: '{text}'", context);
                }
            }

            double tolerance = 0.01;

            AssertHelper.IsTrue(
                Math.Abs(total - 100.0) <= tolerance,
                $"Sum of percentages does not equal 100%. Total = {total:F3}%",
                context
            );
        }
        catch (Exception ex)
        {
            AssertHelper.Fail($"Error while verifying percentage sum: {ex.Message}", context);
        }
    }
}
