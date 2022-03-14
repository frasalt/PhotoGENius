using Xunit;
using System;
using PGENLib;

namespace PGENLib.Tests
{
    public class ColorTests
    {
        [Fact]
        public void test_sum()
        {
            Color a = new Color(1.0f, 2.0f, 3.0f);
            Color b = new Color(5.0f, 6.0f, 7.0f);
            // C# convention: *first* the expected value, *then* the test value
            Assert.True(Color.are_close(new Color(6.0f, 8.0f, 10.0f), a + b));
        }

        [Fact]
        public void test_diff()
        {
            Color a = new Color(1.0f, 2.0f, 3.0f);
            Color b = new Color(5.0f, 6.0f, 7.0f);
            // C# convention: *first* the expected value, *then* the test value
            Assert.True(Color.are_close(new Color(-4.0f, -4.0f, -4.0f), a - b));
        }

        [Fact]
        public void test_ccproduct()
        {
            Color a = new Color(1.0f, 2.0f, 3.0f);
            Color b = new Color(5.0f, 6.0f, 7.0f);
            // C# convention: *first* the expected value, *then* the test value
            Assert.True(Color.are_close(new Color(5.0f, 12.0f, 21.0f), a * b));
        }
        
        [Fact]
        public void test_csproduct()
        {
            Color a = new Color(1.0f, 2.0f, 3.0f);
            float b = 2.0f;
            // C# convention: *first* the expected value, *then* the test value
            Assert.True(Color.are_close(new Color(2.0f, 4.0f, 6.0f), a * b));
        }
        [Fact]
        public void test_areclose()
        {
            Color a = new Color(1.0f, 2.0f, 3.0f);
            // C# convention: *first* the expected value, *then* the test value
            Assert.True(Color.are_close(new Color(1.0f, 2.0f, 3.0f), a));
        }

    }
}
