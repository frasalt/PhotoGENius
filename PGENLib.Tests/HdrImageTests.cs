using Xunit;
using System;
using PGENLib;

namespace PGENLib.Tests
{
    public class HdrImageTests
    {
        [Fact]
        public void test_ValidCoord()
        {
            HdrImage img = new HdrImage(2, 3);
            Assert.True(img.ValidCoord(1, 2));
        }
        
        
    }
}
