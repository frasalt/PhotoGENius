using Xunit;
using System;
using System.Net.Sockets;
using System.Numerics;
using PGENLib;

namespace PGENLib.Tests
{
    public class GeometryTests
    {
        private readonly Vec Vx = new Vec(1.0f, 0.0f, 0.0f);
        private readonly Vec Vy = new Vec(0.0f, 1.0f, 0.0f);
        private readonly Vec Vz = new Vec(0.0f, 0.0f, 1.0f);
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
            Vec aNorm = a.NormalizeVec();
            Normal n = new Normal(1.0f, 2.0f, 3.0f);
            
            Assert.True(Vec.are_close(new Vec(5.0f, 8.0f, 11.0f), a + b));
            Assert.True(Vec.are_close(new Vec(3.0f, 4.0f, 5.0f), b - a));
            Assert.True(Vec.are_close(new Vec(2.0f, 4.0f, 6.0f), a * 2));
            Assert.True(Vec.are_close(new Vec(-1.0f, -2.0f, -3.0f), a.Neg()));
            Assert.True(Vec.are_close(new Vec(-2.0f, 4.0f, -2.0f), Vec.CrossProduct(a,b)));
            Assert.True(Vec.are_close(new Vec(2.0f, -4.0f, 2.0f), Vec.CrossProduct(b,a)));
            Assert.True(Math.Abs(Vec.DotProd(a,b) - 40.0f) < 1E-5);
            Assert.True(Math.Abs(Vec.Norm(a)*Vec.Norm(a) - 14.0f) < 1E-5);
            Assert.True(Math.Abs(Vec.SquaredNorm(a) - 14.0f) < 1E-5);
            Assert.True(Math.Abs(Vec.Norm(aNorm)-1.0f) < 1E-5);
            Assert.True(Normal.are_close(n, a.VecToNorm()));
        }
        
        [Fact]
        public void test_are_point_close()
        {
            Point a = new Point(1.0f, 2.0f, 3.0f);
            Point b = new Point(4.0f, 6.0f, 8.0f);
            Assert.True(Point.are_close(a, a));
            Assert.False(Point.are_close(b, a));
        }
        
        
        [Fact]
        public void test_point_operations()
        {
            Point a = new Point(1.0f, 2.0f, 3.0f);
            Point b = new Point(4.0f, 6.0f, 8.0f);
            Vec v = new Vec(4.0f, 6.0f, 8.0f);

            Assert.True(Point.are_close(new Point(5.0f, 8.0f, 11.0f), a + v));
            Assert.True(Vec.are_close(new Vec(-3.0f, -4.0f, -5.0f), a - b));
            Assert.True(Point.are_close(new Point(-3.0f, -4.0f, -5.0f), a - v));
            Assert.True(Point.are_close(new Point(2.0f, 4.0f, 6.0f), a * 2));
            Assert.True(Vec.are_close(b.PointToVec(), v));
        }
        
        [Fact]
        public void test_normal_operations()
        {
            Normal a = new Normal(1.0f, 2.0f, 3.0f);
            Normal b = new Normal(4.0f, 6.0f, 8.0f);
            Vec v = new Vec(1.0f, 3.0f, 2.0f);
            Normal a_norm = a.NormalizeNormal();
            
            Assert.True(Normal.are_close(new Normal(2.0f, 4.0f, 6.0f), a * 2));
            Assert.True(Normal.are_close(new Normal(-1.0f, -2.0f, -3.0f), a.Neg()));
            Assert.True(Math.Abs(Normal.VecDotNormal(v,a) - 13.0f) < 1E-5);
            Assert.True(Vec.are_close(new Vec(5.0f,-1.0f,-1.0f), Normal.VecCrossNormal(v,a)));
            Assert.True(Normal.are_close(new Normal(-2.0f,4.0f,-2.0f), Normal.NormalCrossNormal(a,b)));
            Assert.True(Math.Abs(Normal.Norm(a)*Normal.Norm(a) - 14.0f) < 1E-5);
            Assert.True(Math.Abs(Normal.SquaredNorm(a) - 14.0f) < 1E-5);
            Assert.True(Math.Abs(Normal.Norm(a_norm)-1.0f) < 1E-5);
        }
        
        
        [Fact]
        public void test_are_transf_close()
        {
            Matrix4x4 m = new Matrix4x4(
                1.0f, 2.0f, 3.0f, 4.0f,
                5.0f, 6.0f, 7.0f, 8.0f,
                9.0f, 9.0f, 8.0f, 7.0f,
                6.0f, 5.0f, 4.0f, 1.0f);

            Matrix4x4 invm = new Matrix4x4(
                -3.75f, 2.75f, -1f, 0.0f,
                4.375f, -3.875f, 2.0f, -0.5f,
                0.5f, 0.5f, -1.0f, 1.0f,
                -1.375f, 0.875f, 0.0f, -0.5f);

            Transformation t1 = new Transformation(m, invm);
            Assert.True(t1.IsConsistent());

            Transformation t2 = new Transformation(invm, m);
            Assert.True(t2.IsConsistent());
            
            Transformation t3 = new Transformation(invm, m);
            t3.m.M33 += 1.0f;
            Assert.False(t3.IsConsistent());
            
        }

        [Fact]
        public void test_multiplication()
        {
            Matrix4x4 m1 = new Matrix4x4(
                1.0f, 2.0f, 3.0f, 4.0f,
                5.0f, 6.0f, 7.0f, 8.0f,
                9.0f, 9.0f, 8.0f, 7.0f,
                6.0f, 5.0f, 4.0f, 1.0f);
            Matrix4x4 invm1 = new Matrix4x4(
                -3.75f, 2.75f, -1f, 0.0f,
                4.375f, -3.875f, 2.0f, -0.5f,
                0.5f, 0.5f, -1.0f, 1.0f,
                -1.375f, 0.875f, 0.0f, -0.5f);
            Matrix4x4 m2 = new Matrix4x4(
                3.0f, 5.0f, 2.0f, 4.0f,
                4.0f, 1.0f, 0.0f, 5.0f,
                6.0f, 3.0f, 2.0f, 0.0f,
                1.0f, 4.0f, 2.0f, 1.0f);
            Matrix4x4 invm2 = new Matrix4x4(
                0.4f, -0.2f, 0.2f, -0.6f,
                2.9f, -1.7f, 0.2f, -3.1f,
                -5.55f, 3.15f, -0.4f, 6.45f,
                -0.9f, 0.7f, -0.2f, 1.1f);
            Matrix4x4 m3 = new Matrix4x4(
                33.0f, 32.0f, 16.0f, 18.0f,
                89.0f, 84.0f, 40.0f, 58.0f,
                118.0f, 106.0f, 48.0f, 88.0f,
                63.0f, 51.0f, 22.0f, 50.0f);
            Matrix4x4 invm3 = new Matrix4x4(
                -1.45f, 1.45f, -1.0f, 0.6f,
                -13.95f, 11.95f, -6.5f, 2.6f,
                25.525f, -22.025f, 12.25f, -5.2f,
                4.825f, -4.325f, 2.5f, -1.1f);
            Transformation t1 = new Transformation(m1, invm1);
            Transformation t2 = new Transformation(m2, invm2);
            Transformation expected = new Transformation(m3, invm3);
            Assert.True(t1.IsConsistent());
            Assert.True(t2.IsConsistent());
            Assert.True(expected.IsConsistent());
            Assert.True(Transformation.are_close(expected, t1*t2));

        }

        [Fact]
        public void test_multiplication_transf()
        {
            Matrix4x4 m1 = new Matrix4x4(
                1.0f, 2.0f, 3.0f, 4.0f,
                5.0f, 6.0f, 7.0f, 8.0f,
                9.0f, 9.0f, 8.0f, 7.0f,
                0.0f, 0.0f, 0.0f, 1.0f);
            Transformation t1 = new Transformation(m1);
            Assert.True(t1.IsConsistent());
            Vec u = new Vec(1.0f, 2.0f, 3.0f);
            Vec expectedV = new Vec(14.0f, 38.0f, 51.0f);
            Assert.True(Vec.are_close(expectedV,  t1 * u));

            Point expectedP = new Point(18.0f, 46.0f, 58.0f);
            Point p = new Point(1.0f, 2.0f, 3.0f);
            Assert.True(Point.are_close(expectedP, t1*p));
        }

        [Fact]
        public void test_inverse()
        {
            Matrix4x4 m1 = new Matrix4x4(
                1.0f, 2.0f, 3.0f, 4.0f,
                5.0f, 6.0f, 7.0f, 8.0f,
                9.0f, 9.0f, 8.0f, 7.0f,
                6.0f, 5.0f, 4.0f, 1.0f);
            Matrix4x4 invm1 = new Matrix4x4(
                -3.75f, 2.75f, -1.0f, 0.0f,
                4.375f, -3.875f, 2.0f, -0.5f,
                0.5f, 0.5f, -1.0f, 1.0f,
                -1.375f, 0.875f, 0.0f, -0.5f);
            Transformation t1 = new Transformation(m1, invm1);
            Transformation t2 = t1.Inverse();
            Assert.True(t2.IsConsistent());
            Assert.True(Transformation.are_close(t1*t2, new Transformation()));
        }
        
        [Fact]
        public void test_traslation()
        {
            Transformation tr1 = Transformation.Traslation(new Vec(1.0f, 2.0f, 3.0f));
            Transformation tr2 = Transformation.Traslation(new Vec(4.0f, 6.0f, 8.0f));
            Transformation prod = tr1 * tr2;
            Transformation expected = Transformation.Traslation(new Vec(5.0f, 8.0f, 11.0f));
            Assert.True(tr1.IsConsistent());
            Assert.True(tr2.IsConsistent());
            Assert.True(tr2.IsConsistent());
            Assert.True(Transformation.are_close(expected, prod));
        }
        
        [Fact]
        public void test_rotations()
        {
            Assert.True(Transformation.RotationX(0.1f).IsConsistent());
            Assert.True(Transformation.RotationY(0.1f).IsConsistent());
            Assert.True(Transformation.RotationZ(0.1f).IsConsistent()); 
            Assert.True(Vec.are_close(Transformation.RotationX(90.0f)*Vy, Vz) );
            Assert.True(Vec.are_close(Transformation.RotationY(90.0f)*Vz, Vx) );
            Assert.True(Vec.are_close(Transformation.RotationZ(90.0f)*Vx, Vy) );
            
        }
        
        
    }
}