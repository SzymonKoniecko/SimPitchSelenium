using System;
using Newtonsoft.Json.Linq;

namespace SimPitchSelenium.Utils;

public static class ConfigReader
{
    private static readonly string configPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

    public static string GetBaseUrl() => Get("baseUrl");
    public static string GetBrowser() => Get("browser");
    public static bool GetHeadless() => bool.Parse(Get("headless"));
    public static string Get(string key)
    {
        var json = File.ReadAllText(configPath);
        var jObject = JObject.Parse(json);
        var token = jObject.SelectToken(key);
        return token?.ToString() ?? string.Empty;
    }
}