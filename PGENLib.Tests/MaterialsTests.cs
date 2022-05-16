using Xunit;
using System;
using System.Net.Sockets;
using System.Numerics;
using PGENLib;

namespace PGENLib.Tests{

    public class MaterialsTests
    {
        [Fact]
        public void testUniformPigment()
        {
            Color UColor = new Color(1.0f, 2.0f, 3.0f);
            UniformPigment pigm = new UniformPigment(UColor);
            Assert.True(Color.are_close(pigm.GetColor(new Vec2d(0.0f,1.0f)),UColor));
            Assert.True(Color.are_close(pigm.GetColor(new Vec2d(0.0f,0.0f)),UColor));
            Assert.True(Color.are_close(pigm.GetColor(new Vec2d(1.0f,0.0f)),UColor));
            Assert.True(Color.are_close(pigm.GetColor(new Vec2d(1.0f,1.0f)),UColor));
        }

        [Fact]
        public void TestCheckeredPigment()
        {
            Color col1 = new Color(1.0f, 2.0f, 3.0f);
            Color col2 = new Color(10.0f, 20.0f, 30f);

            CheckeredPigment pigm = new CheckeredPigment(col1, col2, 2);
            
            Assert.True(Color.are_close(pigm.GetColor(new Vec2d(0.25f,0.25f)),col1));
            Assert.True(Color.are_close(pigm.GetColor(new Vec2d(0.75f,0.25f)),col2));
            Assert.True(Color.are_close(pigm.GetColor(new Vec2d(0.25f,0.75f)),col2));
            Assert.True(Color.are_close(pigm.GetColor(new Vec2d(0.75f,0.75f)),col1));
            
        }
        
        [Fact]
        public void TestImagePigment()
        {
            HdrImage image = new HdrImage(2, 2);
            image.SetPixel(0, 0, new Color(1.0f, 2.0f, 3.0f));
            image.SetPixel(1, 0, new Color(2.0f, 3.0f, 1.0f));
            image.SetPixel(0, 1, new Color(2.0f, 1.0f, 3.0f));
            image.SetPixel(1, 1, new Color(3.0f, 2.0f, 1.0f));

            ImagePigment pigm = new ImagePigment(image);
            
            Assert.True(Color.are_close(pigm.GetColor(new Vec2d(0.0f, 0.0f)),new Color(1.0f, 2.0f, 3.0f)));
            Assert.True(Color.are_close(pigm.GetColor(new Vec2d(0.0f, 1.0f)),new Color(2.0f, 1.0f, 3.0f)));
            Assert.True(Color.are_close(pigm.GetColor(new Vec2d(1.0f, 0.0f)),new Color(2.0f, 3.0f, 1.0f)));
            Assert.True(Color.are_close(pigm.GetColor(new Vec2d(1.0f, 1.0f)),new Color(3.0f, 2.0f, 1.0f)));

        }
        
    }
    
    public class RendererTests
    {
        [Fact]
        public void TestOnOffRenderer()
        {
            var t = Transformation.Traslation(new Vec(2.0f, 0.0f, 0.0f))
                    * Transformation.Scaling(new Vec(0.2f, 0.2f, 0.2f));
            var uPig = new UniformPigment(Color.White());
            var brdf = new DiffuseBRDF(uPig);
            var sphere = new Sphere(t, new Material(brdf));
            var image = new HdrImage(3, 3);
            var camera = new OrthogonalCamera();
            var tracer = new ImageTracer(image, camera);
            var world = new World();
            world.AddShape(sphere);
            var renderer = new OnOffRenderer(world);
            tracer.FireAllRays(renderer.Call);

            var p = image.GetPixel(0, 0);
            var black = Color.Black();
            Assert.True(Color.are_close(p, black));
            Assert.True(Color.are_close(image.GetPixel(1, 0), Color.Black()));
            Assert.True(Color.are_close(image.GetPixel(2, 0), Color.Black()));
            
            Assert.True(Color.are_close(image.GetPixel(0, 1), Color.Black()));
            Assert.True(Color.are_close(image.GetPixel(1, 1), Color.Black()));
            Assert.True(Color.are_close(image.GetPixel(2, 1), Color.Black()));
            
            Assert.True(Color.are_close(image.GetPixel(0, 2), Color.Black()));
            Assert.True(Color.are_close(image.GetPixel(1, 2), Color.Black()));
            Assert.True(Color.are_close(image.GetPixel(2, 2), Color.Black()));
               
        }

        [Fact]
        public void TestFlatRenderer()
        {
            var sphereColor = new Color(1.0f, 2.0f, 3.0f);
            var t = Transformation.Traslation(new Vec(2.0f, 0.0f, 0.0f)) *
                    Transformation.Scaling(new Vec(0.2f, 0.2f, 0.2f));
            var uPig = new UniformPigment(sphereColor);
            var brdf = new DiffuseBRDF(uPig);
            var sphere = new Sphere(t, new Material(brdf));
            var image = new HdrImage(3, 3);
            var camera = new OrthogonalCamera();
            var tracer = new ImageTracer(image, camera);
            var world = new World();
            world.AddShape(sphere);
            var renderer = new FlatRenderer(world);
            tracer.FireAllRays(renderer.Call);
            
            Assert.True(Color.are_close(image.GetPixel(1, 0), Color.Black()));
            Assert.True(Color.are_close(image.GetPixel(1, 0), Color.Black()));
            Assert.True(Color.are_close(image.GetPixel(2, 0), Color.Black()));
            
            Assert.True(Color.are_close(image.GetPixel(0, 1), Color.Black()));
            Assert.True(Color.are_close(image.GetPixel(1, 1), Color.Black()));
            Assert.True(Color.are_close(image.GetPixel(2, 1), Color.Black()));
            
            Assert.True(Color.are_close(image.GetPixel(0, 2), Color.Black()));
            Assert.True(Color.are_close(image.GetPixel(1, 2), Color.Black()));
            Assert.True(Color.are_close(image.GetPixel(2, 2), Color.Black()));

        }
        
        
    }
    
}