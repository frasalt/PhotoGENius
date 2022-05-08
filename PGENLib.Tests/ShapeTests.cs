using Xunit;
using System;
using System.Net.Sockets;
using System.Numerics;
using System;
using PGENLib;

namespace PGENLib.Tests
{
    public class ShapeTest
    {
        private static Vec vecZ = new Vec(0.0f, 0.0f, 1.0f);
        private static Sphere sphere = new Sphere();
        static Ray ray1 = new Ray(origin: new Point(0f, 0f, 2f), dir: -vecZ);
        HitRecord? intersection1 = sphere.RayIntersection(ray1);
        Assert.True(intersection1 != null);
        HitRecord hit1 = new HitRecord(
            new Point(0.0f, 0.0f, 1.0f),
            new Normal(0.0f, 0.0f, 1.0f),
            new Vec2d(0.0f, 0.0f),
            1.0f,
            ray1
        );
        Assert.True(hit1.are_close(intersection1));
        
    }
    
}