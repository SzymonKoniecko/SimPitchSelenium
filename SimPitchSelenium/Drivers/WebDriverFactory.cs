using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using SimPitchSelenium.Utils;

public static class WebDriverFactory
{
    public static IWebDriver CreateDriver()
    {
        var browser = ConfigReader.GetBrowser().ToLowerInvariant();
        var headless = ConfigReader.GetHeadless();

        var mode = (ConfigReader.GetDriverMode() ?? "Auto").Trim();
        var remoteUrl = ConfigReader.GetRemoteUrl();

        return browser switch
        {
            "chrome" => CreateChrome(headless, mode, remoteUrl),
            _ => throw new ArgumentException($"Unsupported browser: {browser}")
        };
    }

    private static IWebDriver CreateChrome(bool headless, string mode, string remoteUrl)
    {
        var options = new ChromeOptions();

        if (headless)
            options.AddArgument("--headless=new");

        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--window-size=1920,1080");

        // Auto => Remote jeśli remoteUrl ustawione, inaczej Local
        var resolvedMode = ResolveMode(mode, remoteUrl);

        return resolvedMode switch
        {
            "local" => new ChromeDriver(options),
            "remote" => new RemoteWebDriver(new Uri(NormalizeRemoteUrl(remoteUrl)), options),
            _ => throw new ArgumentException($"Invalid driverMode: {mode}")
        };
    }

    private static string ResolveMode(string mode, string remoteUrl)
    {
        if (string.Equals(mode, "local", StringComparison.OrdinalIgnoreCase)) return "local";
        if (string.Equals(mode, "remote", StringComparison.OrdinalIgnoreCase)) return "remote";

        // Auto
        return string.IsNullOrWhiteSpace(remoteUrl) ? "local" : "remote";
    }

    private static string NormalizeRemoteUrl(string remoteUrl)
        => string.IsNullOrWhiteSpace(remoteUrl)
            ? throw new ArgumentException("remoteUrl is required when driverMode=Remote.")
            : remoteUrl.TrimEnd('/');
}
