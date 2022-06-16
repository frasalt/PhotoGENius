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

        [Fact]
        public void CilinderTests()
        {
            
        }
        
        
    }

/*
    public class PlaneTests
    {
        private readonly Vec Vx = new Vec(1.0f, 0.0f, 0.0f);
        private readonly Vec Vz = new Vec(0.0f, 0.0f, 1.0f);

        public void TestHit()
        {
            var plane = new XyPlane();

            var ray1 = new Ray(new Point(0.0f, 0.0f, 1.0f), -Vz);
            var intersection1 = plane.RayIntersection(ray1);
            Assert.True(intersection1.HasValue);
            var hit = new HitRecord(new Point(0.0f, 0.0f, 0.0f), 
                new Normal(0.0f, 0.0f, 1.0f), 
                new Vec2d(0.0f, 0.0f),
                1.0f, ray1, plane.Material);
            
            HitRecord.are_close()
                
            assert HitRecord(
                world_point=Point(0.0, 0.0, 0.0),
            normal=Normal(0.0, 0.0, 1.0),
            surface_point=Vec2d(0.0, 0.0),
            t=1.0,
            ray=ray1,
            material=plane.material,
                ).is_close(intersection1)

            ray2 = Ray(origin=Point(0, 0, 1), dir=VEC_Z)
            intersection2 = plane.ray_intersection(ray2)
            assert not intersection2

                ray3 = Ray(origin=Point(0, 0, 1), dir=VEC_X)
            intersection3 = plane.ray_intersection(ray3)
            assert not intersection3

                ray4 = Ray(origin=Point(0, 0, 1), dir=VEC_Y)
            intersection4 = plane.ray_intersection(ray4)
            assert not intersection4

        }
        

    }
*/

}

