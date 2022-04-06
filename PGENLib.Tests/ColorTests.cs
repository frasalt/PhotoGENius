using Xunit;
using System;
using PGENLib;

namespace PGENLib.Tests
{
    public class ColorTests
    {
        [Fact]
        public void test_GetR()
        {
            Color a = new Color(1.0f, 2.0f, 3.0f);
            Assert.True(Math.Abs(a.GetR() - 1.0f) < 1E-5);
        }
        
        [Fact]
        public void test_GetG()
        {
            Color a = new Color(1.0f, 2.0f, 3.0f);
            Assert.True(Math.Abs(a.GetG() - 2.0f) < 1E-5);
        }
        
        [Fact]
        public void test_GetB()
        {
            Color a = new Color(1.0f, 2.0f, 3.0f);
            Assert.True(Math.Abs(a.GetB() - 3.0f) < 1E-5);
        }
        
        [Fact]
        public void test_SetR()
        {
            Color a = new Color(1.0f, 2.0f, 3.0f);
            a.SetR(4.0f);
            Assert.True(Math.Abs(a.GetR() - 4.0f) < 1E-5);
        }
        
        [Fact]
        public void test_SetG()
        {
            Color a = new Color(1.0f, 2.0f, 3.0f);
            a.SetG(4.0f);
            Assert.True(Math.Abs(a.GetG() - 4.0f) < 1E-5);
        }
        
        [Fact]
        public void test_SetB()
        {
            Color a = new Color(1.0f, 2.0f, 3.0f);
            a.SetB(4.0f);
            Assert.True(Math.Abs(a.GetB() - 4.0f) < 1E-5);
        }
        
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
            Assert.True(Color.are_close(new Color(-4.0f, -4.0f, -4.0f), a - b));
        }

        [Fact]
        public void test_ccproduct()
        {
            Color a = new Color(1.0f, 2.0f, 3.0f);
            Color b = new Color(5.0f, 6.0f, 7.0f);
            Assert.True(Color.are_close(new Color(5.0f, 12.0f, 21.0f), a * b));
        }
        
        [Fact]
        public void test_csproduct()
        {
            Color a = new Color(1.0f, 2.0f, 3.0f);
            float b = 2.0f;
            Assert.True(Color.are_close(new Color(2.0f, 4.0f, 6.0f), a * b));
        }
        [Fact]
        public void test_areclose()
        {
            Color a = new Color(1.0f, 2.0f, 3.0f);
            Assert.True(Color.are_close(new Color(1.0f, 2.0f, 3.0f), a));
        }
        
        
        [Fact]
        public void test_luminosity()
        {
            Color col1 = new Color(1.0f, 2.0f, 3.0f);
            float lumi = col1.Lum();
            Assert.True(Math.Abs(lumi - 2.0f) < 1E-5);

        }
        
    }
}
