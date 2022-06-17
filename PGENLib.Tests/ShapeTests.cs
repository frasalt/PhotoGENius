/*
PhotoGENius : photorealistic images generation.
Copyright (C) 2022  Lamorte Teresa, Salteri Francesca, Zanetti Martino

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using Xunit;

namespace PGENLib.Tests
{
    public class ShapeTests
    {
        private readonly Vec Vx = new Vec(1.0f, 0.0f, 0.0f);
        private readonly Vec Vz = new Vec(0.0f, 0.0f, 1.0f);

        [Fact]
        public void TestHitSphere()
        {
            Sphere sphere = new Sphere();
            var epsilon = 1E-5;
            Vec2d p, q;
            
            Ray ray1 = new Ray(origin: new Point(0f, 0f, 2f), dir: -Vz);
            HitRecord? int1 = sphere.RayIntersection(ray1);
            Assert.True(int1 != null);
            HitRecord hit1 = new HitRecord(
                new Point(0.0f, 0.0f, 1.0f),
                new Normal(0.0f, 0.0f, 1.0f),
                new Vec2d(0.0f, 0.0f),
                1.0f,
                ray1,
                sphere.Material
            );

            Assert.True(HitRecord.are_close(hit1, int1.Value));
            
            
            Ray ray2 = new Ray(new Point(3f, 0f, 0f), -Vx);
            HitRecord? int2 = sphere.RayIntersection(ray2);
            Assert.True(int2 != null);
            HitRecord hit2 = new HitRecord(
                new Point(1.0f, 0.0f, 0.0f),
                new Normal(1.0f, 0.0f, 0.0f),
                new Vec2d(0.0f, 0.5f),
                2.0f,
                ray2,
                sphere.Material
            );

            Assert.True(HitRecord.are_close(hit2, int2.Value));
            Assert.True(sphere.RayIntersection(new Ray(new Point(0f, 10f, 2f), -Vz)) == null); // cosìè questo???
                       
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
                ray,
                sphere.Material
            );
            Assert.True(HitRecord.are_close(hit, int3.Value));
        }

        [Fact]
        public void TestTransformation()
        {
            Sphere sphere = new Sphere(Transformation.Translation(new Vec(10.0f, 0.0f, 0.0f)));

            Ray ray1 = new Ray(new Point(10f, 0f, 2f), -Vz);
            HitRecord? int1 = sphere.RayIntersection(ray1);
            Assert.True(int1 != null);
            HitRecord hit1 = new HitRecord(
                new Point(10.0f, 0.0f, 1.0f),
                new Normal(0.0f, 0.0f, 1.0f),   
                new Vec2d(0.0f, 0.0f),
                1.0f,
                ray1,
                sphere.Material
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
                ray2,
                sphere.Material
            );
            Assert.True(HitRecord.are_close(hit2, int2.Value));

            // Check if the sphere translation failed
            Assert.True(sphere.RayIntersection(new Ray(new Point(0f, 0f, 2f), -Vz)) == null);
            Assert.True(sphere.RayIntersection(new Ray(new Point(-10f, 0f, 0f), -Vz)) == null);
        }

        private readonly Vec _vx = new Vec(1.0f, 0.0f, 0.0f);
        private readonly Vec _vy = new Vec(0.0f, 1.0f, 0.0f);
        private readonly Vec _vz = new Vec(0.0f, 0.0f, 1.0f);


        [Fact]
        public void TestHitPlane()
        {
            var plane = new XyPlane();

            var ray1 = new Ray(new Point(0.0f, 0.0f, 1.0f), -_vz);
            var intersection1 = plane.RayIntersection(ray1);
            Assert.True(intersection1.HasValue);
            var hit = new HitRecord(new Point(0.0f, 0.0f, 0.0f), 
                new Normal(0.0f, 0.0f, 1.0f), 
                new Vec2d(0.0f, 0.0f),
                1.0f, ray1, plane.Material);
            if (intersection1.HasValue)
            {
                HitRecord.are_close(hit, intersection1.Value);
            }

            var ray2 = new Ray(new Point(0f, 0f, 1f), _vz);
            var intersection2 = plane.RayIntersection(ray2); 
                Assert.False(intersection2.HasValue);

            var ray3 = new Ray(new Point(0f, 0f, 1f), _vx);
            var intersection3 = plane.RayIntersection(ray3);
            Assert.False(intersection3.HasValue);

            var ray4 = new Ray(new Point(0f, 0f, 1f), _vy);
            var intersection4 = plane.RayIntersection(ray4);
            Assert.False(intersection4.HasValue);

        }
        
    }
  
  public class WorldTest
    {
        [Fact]
        public void TestRayIntersection()
        {
            var world = new World();
            var vx = new Vec(1.0f, 0f, 0f);
            var sphere1 = new Sphere(Transformation.Translation(vx * 2f));
            var sphere2 = new Sphere(Transformation.Translation(vx * 8f));
            world.AddShape(sphere1);
            world.AddShape(sphere2);

            var intersection1 = world.RayIntersection(new Ray(new Point(), vx));
            Assert.True(intersection1.HasValue &&
                        Point.are_close(intersection1.Value.WorldPoint, new Point(1.0f, 0.0f, 0.0f)));

            var intersection2 = world.RayIntersection(new Ray(new Point(10f, 0f, 0f), -vx));

            Assert.True(intersection2.HasValue &&
                        Point.are_close(intersection2.Value.WorldPoint, new Point(9f, 0.0f, 0.0f)));
        }
    }

}

