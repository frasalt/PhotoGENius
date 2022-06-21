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
using System;

namespace PGENLib.Tests{

    public class MaterialsTests
    {
        [Fact]
        public void TestUniformPigment()
        {
            Color uColor = new Color(1.0f, 2.0f, 3.0f);
            UniformPigment pigm = new UniformPigment(uColor);
            Assert.True(Color.are_close(pigm.GetColor(new Vec2d(0.0f,1.0f)),uColor));
            Assert.True(Color.are_close(pigm.GetColor(new Vec2d(0.0f,0.0f)),uColor));
            Assert.True(Color.are_close(pigm.GetColor(new Vec2d(1.0f,0.0f)),uColor));
            Assert.True(Color.are_close(pigm.GetColor(new Vec2d(1.0f,1.0f)),uColor));
        }

        [Fact]
        public void TestCheckeredPigment()
        {
            Color col1 = new Color(1.0f, 2.0f, 3.0f);
            Color col2 = new Color(10.0f, 20.0f, 30.0f);

            CheckeredPigment pigm = new CheckeredPigment(col1, col2, 2);
            
            Assert.True(Color.are_close(pigm.GetColor(new Vec2d(0.25f,0.25f)),col1));
            Assert.True(Color.are_close(pigm.GetColor(new Vec2d(0.75f, 0.25f)), col2));
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
            var t = Transformation.Translation(new Vec(2.0f, 0.0f, 0.0f))
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
            var t = Transformation.Translation(new Vec(2.0f, 0.0f, 0.0f)) *
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
    
    public class PathTracerTest
    {
        [Fact]
        public void TestFurnace()
        {
            var pcg = new PCG();
            for (int i = 0; i < 5; i++)
            {
                var emittedRadiance = pcg.RandomFloat();
                var reflectance = pcg.RandomFloat();
                
                var brdf = new DiffuseBRDF(new UniformPigment(Color.White() * reflectance));
                var enclosureMaterial = new Material(new UniformPigment(Color.White() * emittedRadiance), brdf);
                
                var world = new World();
                world.AddShape(new Sphere(enclosureMaterial));
                
                var pathTracer = new PathTracer(world, pcg, 1, 100, 101);
                var ray = new Ray(new Point(0, 0, 0), new Vec(1, 0, 0));
               
                var color = pathTracer.Call(ray);
                var expected = (float) emittedRadiance / (1.0 - reflectance); //result of the geometric series
                Assert.True(Math.Abs(color.r - expected) < 1E-03);
                Assert.True(Math.Abs(color.g - expected) < 1E-03);
                Assert.True(Math.Abs(color.b - expected) < 1E-03);
                
            }
        }
    }
}