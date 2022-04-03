using Xunit;
using System;
using PGENLib;

namespace PGENLib.Tests
{
    public class GeometryTests
    {
        [Fact]
        public void test_are_vector_close()
        {
            Vec a = new Vec(1.0f, 2.0f, 3.0f);
            Vec b = new Vec(4.0f, 6.0f, 8.0f);
            Assert.True(Vec.are_close(a, a));
            Assert.False(Vec.are_close(b, a));
        }
        
        [Fact]
        public void test_vector_operations()
        {
            Vec a = new Vec(1.0f, 2.0f, 3.0f);
            Vec b = new Vec(4.0f, 6.0f, 8.0f);
            Vec a_norm = a.NormalizeVec();
            
            Assert.True(Vec.are_close(new Vec(5.0f, 8.0f, 11.0f), a + b));
            Assert.True(Vec.are_close(new Vec(3.0f, 4.0f, 5.0f), b - a));
            Assert.True(Vec.are_close(new Vec(2.0f, 4.0f, 6.0f), a * 2));
            Assert.True(Vec.are_close(new Vec(-1.0f, -2.0f, -3.0f), a.Neg()));
            Assert.True(Vec.are_close(new Vec(-2.0f, 4.0f, -2.0f), Vec.CrossProduct(a,b)));
            Assert.True(Vec.are_close(new Vec(2.0f, -4.0f, 2.0f), Vec.CrossProduct(b,a)));
            Assert.True(Math.Abs(Vec.DotProd(a,b) - 40.0f) < 1E-5);
            Assert.True(Math.Abs(Vec.Norm(a)*Vec.Norm(a) - 14.0f) < 1E-5);
            Assert.True(Math.Abs(Vec.SquaredNorm(a) - 14.0f) < 1E-5);
            Assert.True(Math.Abs(Vec.Norm(a_norm)-1.0f) < 1E-5);
        }

        
        
        
    }
}