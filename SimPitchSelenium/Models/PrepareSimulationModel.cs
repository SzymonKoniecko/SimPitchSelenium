using System;

namespace SimPitchSelenium.Models;

public class PrepareSimulationModel
{
    public bool isSeason2022_2023 { get; set; } = false;
    public bool isSeason2023_2024 { get; set; } = false;
    public bool isSeason2024_2025 { get; set; } = false;
    public bool isSeason2025_2026 { get; set; } = false;

    public string? Title { get; set; }
    public string League { get; set; }
    public int NumberOfIterations { get; set; }
    public int? Seed { get; set; }
    public int? GamesToReachTrust { get; set; }
    public float? ConfidenceLevel { get; set; }
    public float? NoiseFactor { get; set; }
    public float? HomeAdvantage { get; set; }
    public bool CreateScoreboards { get; set; }
}
