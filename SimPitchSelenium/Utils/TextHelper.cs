using System;
using System.Globalization;
using SimPitchSelenium.Reports;
using SimPitchSelenium.Tests;

namespace SimPitchSelenium.Utils;

public static class TextHelper
{
    public static void AssertTextEquals(string actual, string expected, string context = "")
    {
        try
        {
            Assert.That(actual?.Trim(), Is.EqualTo(expected?.Trim()),
                $"Text mismatch in {context}\nExpected: '{expected}'\nActual:   '{actual}'");
        }
        catch (AssertionException ex)
        {
            AssertHelper.HandleAssertionFailure(ex, context);
            throw;
        }
    }

    public static void AssertTextContains(string actual, string expectedSubstring, string context = "")
    {
        try
        {
            Assert.That(actual, Does.Contain(expectedSubstring),
                $"Text does not contain expected substring in {context}\nExpected to contain: '{expectedSubstring}'\nActual: '{actual}'");
        }
        catch (AssertionException ex)
        {
            AssertHelper.HandleAssertionFailure(ex, context);
            throw;
        }
    }

    public static void AssertTextNotContains(string actual, string unexpectedSubstring, string context = "")
    {
        try
        {
            Assert.That(actual, Does.Not.Contain(unexpectedSubstring),
                $"Text should not contain substring in {context}\nUnexpected substring: '{unexpectedSubstring}'\nActual: '{actual}'");
        }
        catch (AssertionException ex)
        {
            AssertHelper.HandleAssertionFailure(ex, context);
            throw;
        }
    }

    public static void AssertTextNotEmpty(string actual, string context = "")
    {
        try
        {
            Assert.That(string.IsNullOrWhiteSpace(actual), Is.False,
                $"Text is empty or null in {context}");
            Console.WriteLine($"Text is not empty ({context})");
        }
        catch (AssertionException ex)
        {
            AssertHelper.HandleAssertionFailure(ex, context);
            throw;
        }
    }

    public static string GetFormattedCurrentDate(string format = "dd/MM/yyyy")
    {
        try
        {
            return DateTime.Now.ToString(format, CultureInfo.InvariantCulture);
        }
        catch (FormatException)
        {
            Console.WriteLine($"Incorrect date format: '{format}'. Returned default: (dd/MM/yyyy).");
            return DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
    }
}