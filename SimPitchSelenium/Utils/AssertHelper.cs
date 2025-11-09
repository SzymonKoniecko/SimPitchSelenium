using System;
using SimPitchSelenium.Reports;
using SimPitchSelenium.Tests;
using NUnit.Framework;

namespace SimPitchSelenium.Utils;

public static class AssertHelper
{
    public static void IsTrue(bool condition, string message, string context = "")
    {
        try
        {
            Assert.That(condition, Is.True, message);
        }
        catch (AssertionException ex)
        {
            HandleAssertionFailure(ex, context);
            throw;
        }
    }

    public static void IsFalse(bool condition, string message, string context = "")
    {
        try
        {
            Assert.That(condition, Is.False, message);
        }
        catch (AssertionException ex)
        {
            HandleAssertionFailure(ex, context);
            throw;
        }
    }

    public static void AreEqual(object expected, object actual, string message = "", string context = "")
    {
        try
        {
            Assert.That(actual, Is.EqualTo(expected), message);
        }
        catch (AssertionException ex)
        {
            HandleAssertionFailure(ex, context);
            throw;
        }
    }

    public static void AreNotEqual(object notExpected, object actual, string message = "", string context = "")
    {
        try
        {
            Assert.That(actual, Is.Not.EqualTo(notExpected), message);
        }
        catch (AssertionException ex)
        {
            HandleAssertionFailure(ex, context);
            throw;
        }
    }

    public static void IsNotNull(object value, string message = "", string context = "")
    {
        try
        {
            Assert.That(value, Is.Not.Null, message);
        }
        catch (AssertionException ex)
        {
            HandleAssertionFailure(ex, context);
            throw;
        }
    }

    public static void IsNull(object value, string message = "", string context = "")
    {
        try
        {
            Assert.That(value, Is.Null, message);
        }
        catch (AssertionException ex)
        {
            HandleAssertionFailure(ex, context);
            throw;
        }
    }

    public static void Fail(string message, string context = "")
    {
        try
        {
            Assert.Fail(message);
        }
        catch (AssertionException ex)
        {
            HandleAssertionFailure(ex, context);
            throw;
        }
    }

    public static void HandleAssertionFailure(AssertionException ex, string context)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ASSERTION FAILED] {ex.Message}");
        Console.ResetColor();

        try
        {
            var driver = BaseTest.DriverInstance;
            if (driver != null)
            {
                ErrorReporter.CaptureFailure(driver, context);
                Console.WriteLine($"[SCREENSHOT CAPTURED] Context: {context}");
            }
            else
            {
                Console.WriteLine("⚠️  DriverInstance is null — screenshot not captured.");
            }
        }
        catch (Exception captureEx)
        {
            Console.WriteLine($"!! Failed to report error: {captureEx.Message}");
        }
    }
}