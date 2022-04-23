using Xunit;
using System;
using System.Numerics;
using System.Reflection.PortableExecutable;
using PGENLib;

namespace PGENLib.Tests
{
    public class CamerasTest
    {
        [Fact]
        public void test_areclose()
        {
            Ray a = new Ray(new Point(1.0f, 2.0f, 3.0f), new Vec(5.0f, 4.0f, -1.0f));
            Ray b = new Ray(new Point(1.0f, 2.0f, 3.0f), new Vec(5.0f, 4.0f, -1.0f));
            Ray c = new Ray(new Point(5.0f, 1.0f, 4.0f), new Vec(3.0f, 9.0f, 4.0f));
            Assert.True(Ray.are_close(a, b));
        }
  
        [Fact]
        public void test_at()
        {
            Ray d = new Ray(new Point(1.0f, 2.0f, 4.0f), new Vec(4.0f, 2.0f, 1.0f)); 
            Assert.True(Point.are_close(d.At(0.0f), d.Origin));
            Assert.True(Point.are_close(d.At(1.0f), new Point(5.0f, 4.0f, 5.0f)));
            Assert.True(Point.are_close(d.At(2.0f), new Point(9.0f, 6.0f, 6.0f)));
        }
        
        [Fact]
        public void test_ortCamera()
        {
            var cam = new OrthogonalCamera(2.0f);
            var ray1 = cam.FireRay(0.0f, 0.0f);
            var ray2 = cam.FireRay(1.0f, 0.0f);
            var ray3 = cam.FireRay(0.0f, 1.0f);
            var ray4 = cam.FireRay(1.0f, 1.0f);
            
            //Are rays parallel (as they should be)?
            //Assert.True(Math.Abs(Vec.SquaredNorm(Vec.CrossProduct(ray1.Dir, ray2.Dir)), 0.0f)<1E-5);
            Assert.True(Vec.SquaredNorm(Vec.CrossProduct(ray1.Dir, ray2.Dir)) == 0.0f);
            Assert.True(Vec.SquaredNorm(Vec.CrossProduct(ray1.Dir, ray3.Dir)) == 0.0f);
            Assert.True(Vec.SquaredNorm(Vec.CrossProduct(ray1.Dir, ray4.Dir)) == 0.0f);
            
            //Right coordinates?
            Assert.True(Point.are_close(ray1.At(1.0f), new Point(0.0f, 2.0f, -1.0f)));
            Assert.True(Point.are_close(ray2.At(1.0f), new Point(0.0f, -2.0f, -1.0f)));
            Assert.True(Point.are_close(ray3.At(1.0f), new Point(0.0f, 2.0f, 1.0f)));
            Assert.True(Point.are_close(ray4.At(1.0f), new Point(0.0f, -2.0f, 1.0f)));
        }

        [Fact]
        public void test_ortCameraTransform()
        {
            var Vy = new Vec(0.0f, 1.0f, 0.0f);
            var tr = Transformation.Traslation(-Vy * 2.0f)*Transformation.RotationZ(90);
            var cam = new OrthogonalCamera(2.0f, tr);
            var Ray = cam.FireRay(0.5f, 0.5f);
            Assert.True(Point.are_close(Ray.At(1.0f), new Point(0.0f, -2.0f, 0.0f)));
        }
        
        [Fact]
        public void test_PerspCamera()
        {
            var cam = new PerspectiveCamera(1.0f, 2.0f);
            var ray1 = cam.FireRay(0.0f, 0.0f);
            var ray2 = cam.FireRay(1.0f, 0.0f);
            var ray3 = cam.FireRay(0.0f, 1.0f);
            var ray4 = cam.FireRay(1.0f, 1.0f);
            
            //Do all the rays have he same origin?
            Assert.True(Point.are_close(ray1.Origin, ray2.Origin));
            Assert.True(Point.are_close(ray1.Origin, ray3.Origin));
            Assert.True(Point.are_close(ray1.Origin, ray4.Origin));
            
            //Right coordinates?
            Assert.True(Point.are_close(ray1.At(1.0f), new Point(0.0f, 2.0f, -1.0f)));
            Assert.True(Point.are_close(ray2.At(1.0f), new Point(0.0f, -2.0f, -1.0f)));
            Assert.True(Point.are_close(ray3.At(1.0f), new Point(0.0f, 2.0f, 1.0f)));
            Assert.True(Point.are_close(ray4.At(1.0f), new Point(0.0f, -2.0f, 1.0f)));
        }
        
        //==============================================================================================================
        //ImageTracer
        //==============================================================================================================

        [Fact]
        public void test_image_tracer()
        {
            HdrImage image = new HdrImage(4, 2);
            PerspectiveCamera camera = new PerspectiveCamera(2);
            ImageTracer tracer = new ImageTracer(image, camera);

            Ray ray1 = tracer.FireRay(0, 0, 2.5f, 1.5f);
            Ray ray2 = tracer.FireRay(2, 1, 0.5f, 0.5f);
            Assert.True(Ray.are_close(ray1,ray2));

            tracer.FireAllRays(lambda);
            
            Color mycolor = new Color(1.0f, 2.0f, 3.0f);
            Color mypixel;
            for (int row = 0; row < image.Height; row++)
            {
                for (int col = 0; col < image.Width; col++)
                {
                    mypixel = image.GetPixel(col, row);
                    Assert.True(mypixel.GetR() == mycolor.GetR());
                    Assert.True(mypixel.GetG() == mycolor.GetG());
                    Assert.True(mypixel.GetB() == mycolor.GetB());
                }
            }
        }

        Color lambda(Ray ray)
        {
            return new Color(1.0f, 2.0f, 3.0f);
        }

    }
}
