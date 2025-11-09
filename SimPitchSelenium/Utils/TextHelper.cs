using System;
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
}