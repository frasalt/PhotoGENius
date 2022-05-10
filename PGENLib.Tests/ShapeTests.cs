using Xunit;
using System;
using System.Net.Sockets;
using System.Numerics;
using System.Runtime.Intrinsics;
using PGENLib;

namespace PGENLib.Tests
{
    public class ShapeTests
    {
        private readonly Vec Vx = new Vec(1.0f, 0.0f, 0.0f);
        private readonly Vec Vz = new Vec(0.0f, 0.0f, 1.0f);

        [Fact]
        public void Dumb()
        {
            Assert.True(1 == 1);
        }

        [Fact]
        public void TestHitSphere()
        {
            Sphere sphere = new Sphere();
            Ray ray1 = new Ray(origin: new Point(0f, 0f, 2f), dir: -Vz);
            HitRecord? int1 = sphere.RayIntersection(ray1);
            Assert.True(int1 != null);
            HitRecord hit1 = new HitRecord(
                new Point(0.0f, 0.0f, 1.0f),
                new Normal(0.0f, 0.0f, 1.0f),
                new Vec2d(0.0f, 0.0f),
                1.0f,
                ray1
            );
            Assert.True(HitRecord.are_close(hit1,int1.Value));

            Ray ray2 = new Ray(new Point(3f, 0f, 0f), -Vx);
            HitRecord? int2 = sphere.RayIntersection(ray2);
            Assert.True(int2 != null);
            HitRecord hit2 = new HitRecord(
                new Point(1.0f, 0.0f, 0.0f),
                new Normal(1.0f, 0.0f, 0.0f),
                new Vec2d(0.0f, 0.5f),
                2.0f,
                ray2
            );
            Assert.True(HitRecord.are_close(hit2,int2.Value));

            Assert.True(sphere.RayIntersection(new Ray(new Point(0f, 10f, 2f), -Vz)) == null);
            
        }
        
        [Fact]
        public void TestHitSphereIn()
        {
            Sphere sphere = new Sphere();
            Ray ray = new Ray(origin: new Point(0f, 0f, 0f), dir: Vx);
            HitRecord? int3 = sphere.RayIntersection(ray);
            Assert.True(int3 != null);

            HitRecord hit = new HitRecord(
                new Point(1.0f, 0.0f, 0.0f),
                new Normal(-1.0f, 0.0f, 0.0f),
                new Vec2d(0.0f, 0.5f),
                1.0f,
                ray
            );
            Assert.True(HitRecord.are_close(hit, int3.Value));
        }

        [Fact]
        public void TestTransformation()
        {
            Sphere sphere = new Sphere(Transformation.Traslation(new Vec(10.0f, 0.0f, 0.0f)));

            Ray ray1 = new Ray(new Point(10f, 0f, 2f), -Vz);
            HitRecord? int1 = sphere.RayIntersection(ray1);
            Assert.True(int1 != null);
            HitRecord hit1 = new HitRecord(
                new Point(10.0f, 0.0f, 1.0f),
                new Normal(0.0f, 0.0f, 1.0f),
                new Vec2d(0.0f, 0.0f),
                1.0f,
                ray1
            );
            Assert.True(HitRecord.are_close(hit1, int1.Value));

            Ray ray2 = new Ray(new Point(13f, 0f, 0f), -Vx);
            HitRecord? int2 = sphere.RayIntersection(ray2);
            Assert.True(int2 != null);
            HitRecord hit2 = new HitRecord(
                new Point(11.0f, 0.0f, 0.0f),
                new Normal(1.0f, 0.0f, 0.0f),
                new Vec2d(0.0f, 0.5f),
                2.0f,
                ray2
            );
            Assert.True(HitRecord.are_close(hit2, int2.Value));

            // Check if the sphere translation failed
            Assert.True(sphere.RayIntersection(new Ray(new Point(0f, 0f, 2f), -Vz)) == null);
            Assert.True(sphere.RayIntersection(new Ray(new Point(-10f, 0f, 0f), -Vz)) == null);
        }
    }
}