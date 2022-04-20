using Xunit;
using System;
using System.Reflection.PortableExecutable;
using PGENLib;

namespace PGENLib.Tests
{
    public class CamerasTest
    {
        [Fact]
        public void test_isclose()
        {
            Ray a = new Ray(new Point(1.0f, 2.0f, 3.0f), new Vec(5.0f, 4.0f, -1.0f));
            Ray b = new Ray(new Point(1.0f, 2.0f, 3.0f), new Vec(5.0f, 4.0f, -1.0f));
            Ray c = new Ray(new Point(5.0f, 1.0f, 4.0f), new Vec(3.0f, 9.0f, 4.0f));
            Assert.True(Ray.is_close(a, b));
        }
  
        [Fact]
        public void test_at()
        {
            Ray d = new Ray(new Point(1.0f, 2.0f, 4.0f), new Vec(4.0f, 2.0f, 1.0f)); 
            Assert.True(Point.are_close(Ray.at(d, 0.0f), d.Origin));
            Assert.True(Point.are_close(Ray.at(d, 1.0f), new Point(5.0f, 4.0f, 5.0f)));
            Assert.True(Point.are_close(Ray.at(d, 2.0f), new Point(9.0f, 6.0f, 6.0f)));
        }
    }
}