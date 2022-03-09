using Xunit;
using System;
using PGENLib;

namespace PGENLib.Tests;

public class ColorTests
{
    [Fact]
    public void TestSum()
    {
        Color a = new Color(1.0f, 2.0f, 3.0f);
        Color b = new Color(5.0f, 6.0f, 7.0f);
        // C# convention: *first* the expected value, *then* the test value
        Assert.True(Color.are_close(new Color(6.0f, 8.0f, 10.0f), a + b));
    }
}

