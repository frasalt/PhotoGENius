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
            
            Assert.True(Color.are_close(pigm.getColor(new Vec2d(0.0f, 0.0f)),new Color(1.0f, 2.0f, 3.0f)));
            Assert.True(Color.are_close(pigm.getColor(new Vec2d(0.0f, 1.0f)),new Color(2.0f, 1.0f, 3.0f)));
            Assert.True(Color.are_close(pigm.getColor(new Vec2d(1.0f, 0.0f)),new Color(2.0f, 3.0f, 1.0f)));
            Assert.True(Color.are_close(pigm.getColor(new Vec2d(1.0f, 1.0f)),new Color(3.0f, 2.0f, 1.0f)));

        }
        
    }
    
}