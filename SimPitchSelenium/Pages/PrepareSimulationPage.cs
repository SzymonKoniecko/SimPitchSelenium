using OpenQA.Selenium;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Pages;

public class PrepareSimulationPage : BasePage
{
    protected By By_Title;
    internal By By_Season_2022_2023_CheckBox;
    internal By By_Season_2023_2024_CheckBox;
    internal By By_Season_2024_2025_CheckBox;
    internal By By_Season_2025_2026_CheckBox;
    internal By By_Title_Input;
    internal By By_League_Input;
    internal By By_NumberIteration_Input;
    internal By By_CreateScoreboards_Checkbox;
    internal By By_CreatedSimulation_Message;
    internal By By_CreatedSimulation_Button;
    internal By By_Validation_Error;
    public PrepareSimulationPage(IWebDriver webDriver) : base(driver: webDriver)
    {
        By_Title = GetBySeleniumId("title-prepare-simulation");

        By_Season_2022_2023_CheckBox = GetByValue("2022/2023");
        By_Season_2023_2024_CheckBox = GetByValue("2023/2024");
        By_Season_2024_2025_CheckBox = GetByValue("2024/2025");
        By_Season_2025_2026_CheckBox = GetByValue("2025/2026");
        By_Title_Input = GetBySeleniumId("input-title");
        By_League_Input = GetById("leagueId");
        By_NumberIteration_Input = GetBySeleniumId("input-iterations");
        By_CreateScoreboards_Checkbox = GetBySeleniumId("input-create-scoreboards");
        By_CreatedSimulation_Message = GetBySeleniumId("simulation-id-text");
        By_CreatedSimulation_Button = GetBySeleniumId("simulation-result");
        By_Validation_Error = GetByClass("validation-error");

    }

    internal string GetSimulationId()
    {
        return GetElementText(By_CreatedSimulation_Message)
            .Split("Simulation ID:")[1]
            .ToString()
            .Trim();
    }

    internal void SelectSeasonYears(
        bool isSeason2022_2023 = false,
        bool isSeason2023_2024 = false,
        bool isSeason2024_2025 = false,
        bool isSeason2025_2026 = false)
    {
        if (isSeason2022_2023)
        {
            Click(By_Season_2022_2023_CheckBox);
        }
        if (isSeason2023_2024)
        {
            Click(By_Season_2023_2024_CheckBox);
        }
        if (isSeason2024_2025)
        {
            Click(By_Season_2024_2025_CheckBox);
        }
        if (isSeason2025_2026)
        {
            Click(By_Season_2025_2026_CheckBox);
        }
    }

    internal void SelectTitle(string title)
    {
        Type(By_Title_Input, title, true);
        Click(By_Title);
    }

    internal void SelectLeague(string leagueName)
    {
        SelectFromDropdown(By_League_Input, leagueName, "PrepareSimulationPage");
    }

    internal void SelectNumberOfIterations(string iterationsNumber)
    {
        Type(By_NumberIteration_Input, iterationsNumber, true);
    }

    internal void SelectCreateScoreboardsCheckbox()
    {
        Click(By_CreateScoreboards_Checkbox);
    }

    internal void ClickStartSimulation()
    {
        Click(base.By_Button_Primary);
    }

    internal void ClickResetForm()
    {
        Click(base.By_Button_Secondary);
    }

    internal SimulationItemPage GoToSimulationItemPage()
    {
        Click(By_CreatedSimulation_Button);
        return new SimulationItemPage(Driver);
    }

    internal void AssertIfDisplayed()
    {
        var el = IsElementDisplayed(By_Title);
        AssertHelper.IsTrue(IsElementDisplayed(By_Title), "Page is not loaded", "PrepareSimulationPage");
    }

    internal void AssertSelectedSeasonYears(
        bool isSeason2022_2023 = false,
        bool isSeason2023_2024 = false,
        bool isSeason2024_2025 = false,
        bool isSeason2025_2026 = false)
    {
        AssertIfSelected(By_Season_2022_2023_CheckBox, isSeason2022_2023);
        AssertIfSelected(By_Season_2023_2024_CheckBox, isSeason2023_2024);
        AssertIfSelected(By_Season_2024_2025_CheckBox, isSeason2024_2025);
        AssertIfSelected(By_Season_2025_2026_CheckBox, isSeason2025_2026);
    }

    internal void AssertTitle(string expectedTitle)
    {
        TextHelper.AssertTextEquals(GetElementText(By_Title_Input), expectedTitle, "PrepareSimulationPage");
    }

    internal void AssertNumberOfIterations(string expectedIterationsNumber)
    {
        TextHelper.AssertTextEquals(GetElementText(By_NumberIteration_Input), expectedIterationsNumber, "PrepareSimulationPage");
    }

    internal void AssertLeague(string expectedLeague)
    {
        AssertDropdownValue(By_League_Input, expectedLeague, "PrepareSimulationPage");
    }

    internal void AssertCreateScoreboardsCheckbox(bool isSelected)
    {
        AssertIfSelected(By_CreateScoreboards_Checkbox, isSelected);
    }

    internal void AssertValidationErrors(params string[] validationErrors)
    {
        var validationText = GetElementText(By_Validation_Error);
        foreach (var valErrors in validationErrors)
        {
            TextHelper.AssertTextContains(validationText, valErrors, "Missing validation error text!");
        }
    }

    internal void AssertStartedSimulationMessage()
    {
        TextHelper.AssertTextContains(
            GetElementText(By_CreatedSimulation_Message),
            "Simulation ID: ",
            "Missing or incorrect created simulation text!");
    }

    internal void StartAnySimulation(string iterations = "2")
    {
        SelectSeasonYears(
            isSeason2022_2023: true,
            isSeason2025_2026: true
        );
        SelectTitle("Any test - " + DateTime.Now);
        SelectLeague("pko-bp-ekstraklasa");
        SelectNumberOfIterations(iterations);

        ClickStartSimulation();

        AssertStartedSimulationMessage();
    }
}
