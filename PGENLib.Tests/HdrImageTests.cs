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
using System.IO;
using PGENLib;
using Xunit.Abstractions;

namespace PGENLib.Tests
{
    public class HdrImageTests
    {
        byte[] byteRef_LE = new byte[84]{
            0x50, 0x46, 0x0a, 0x33, 0x20, 0x32, 0x0a, 0x2d, 0x31, 0x2e, 0x30, 0x0a,
            0x00, 0x00, 0xc8, 0x42, 0x00, 0x00, 0x48, 0x43, 0x00, 0x00, 0x96, 0x43,
            0x00, 0x00, 0xc8, 0x43, 0x00, 0x00, 0xfa, 0x43, 0x00, 0x00, 0x16, 0x44,
            0x00, 0x00, 0x2f, 0x44, 0x00, 0x00, 0x48, 0x44, 0x00, 0x00, 0x61, 0x44,
            0x00, 0x00, 0x20, 0x41, 0x00, 0x00, 0xa0, 0x41, 0x00, 0x00, 0xf0, 0x41,
            0x00, 0x00, 0x20, 0x42, 0x00, 0x00, 0x48, 0x42, 0x00, 0x00, 0x70, 0x42,
            0x00, 0x00, 0x8c, 0x42, 0x00, 0x00, 0xa0, 0x42, 0x00, 0x00, 0xb4, 0x42
        };

        private byte[] byteRef_BE = new byte[83]
        {
            0x50, 0x46, 0x0a, 0x33, 0x20, 0x32, 0x0a, 0x31, 0x2e, 0x30, 0x0a, 0x42,
            0xc8, 0x00, 0x00, 0x43, 0x48, 0x00, 0x00, 0x43, 0x96, 0x00, 0x00, 0x43,
            0xc8, 0x00, 0x00, 0x43, 0xfa, 0x00, 0x00, 0x44, 0x16, 0x00, 0x00, 0x44,
            0x2f, 0x00, 0x00, 0x44, 0x48, 0x00, 0x00, 0x44, 0x61, 0x00, 0x00, 0x41,
            0x20, 0x00, 0x00, 0x41, 0xa0, 0x00, 0x00, 0x41, 0xf0, 0x00, 0x00, 0x42,
            0x20, 0x00, 0x00, 0x42, 0x48, 0x00, 0x00, 0x42, 0x70, 0x00, 0x00, 0x42,
            0x8c, 0x00, 0x00, 0x42, 0xa0, 0x00, 0x00, 0x42, 0xb4, 0x00, 0x00
        };
        
        private readonly ITestOutputHelper _testOutputHelper;

        public HdrImageTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void test_ValidCoord()
        {
            HdrImage img = new HdrImage(2, 3);
            Assert.True(img.ValidCoord(1, 2));
        }
        
        [Fact]
        public void test_GetPixel()
        {
            Color[] pix = new Color[2*2];
            pix[0] = new Color(1.0f, 2.0f, 3.0f);
            pix[1] = new Color(4.0f, 5.0f, 6.0f);
            pix[2] = new Color(7.0f, 8.0f, 9.0f);
            pix[3] = new Color(10.0f, 11.0f, 12.0f);
            HdrImage img = new HdrImage(2, 2, pix);
            Assert.True(Color.are_close(new Color(7.0f, 8.0f, 9.0f), img.GetPixel(0,1)));
        }
        
        
        [Fact]
        public void test_WritePFM()
        {
            //Creo un'immagine hdr e ci scrivo i colori in binario
            HdrImage img = new HdrImage(3, 2);
            Color a = new Color(1.0e1f, 2.0e1f, 3.0e1f);
            Color b = new Color(4.0e1f, 5.0e1f, 6.0e1f);
            Color c = new Color(7.0e1f, 8.0e1f, 9.0e1f);
            Color d = new Color(1.0e2f, 2.0e2f, 3.0e2f);
            Color e = new Color(4.0e2f, 5.0e2f, 6.0e2f);
            Color f = new Color(7.0e2f, 8.0e2f, 9.0e2f);
            img.SetPixel(0, 0, a);
            img.SetPixel(1, 0, b);
            img.SetPixel(2, 0, c);
            img.SetPixel(0, 1, d);
            img.SetPixel(1, 1, e);
            img.SetPixel(2, 1, f);
            
            // scrivo l'immagine in uno stream in BE
            MemoryStream str = new MemoryStream();
            img.WritePFMFile(str, Endianness.BigEndian);
            
            byte[] localvec = str.ToArray();
            byte[] refer = str.ToArray();
            
            // verifico l'uguaglianza
            for (int i = 0; i < 83; i++)
            {
                Assert.True(localvec[i] == refer[i]);
            }
        }
        
        [Fact]
        public void test_ParseImgSize()
        {
            HdrImage img = new HdrImage(1,1);
            string str = "20 30";
            int[] dim = new int[2];
            dim = img.ParseImgSize(str);
            Assert.True(dim[0] == 20 && dim[1] == 30);
        }
        
        [Fact]
        public void test_ParseEndianness()
        {
            HdrImage img = new HdrImage(1,1);
            string str = "-2";
            int end;
            end = img.ParseEndianness(str);
            Assert.True(end == -1);
        }
        
        
        [Fact]
        public void test_ReadLine()
        {
            //Preparo il memory stream da leggere
            string a;
            string b;
            string c;
            HdrImage img = new HdrImage(1,1);
            MemoryStream input = new MemoryStream();
            
            using (StreamWriter writer = new StreamWriter(input))
            {
                writer.Write("Hello\nworld!\n\n"); // ATTENZIONE QUI: la funzione andrebbe corretta per riconoscere la fine di uno stream :(
                writer.Flush();
                input.Position = 0;
                a = img.ReadLine(input);
                b = img.ReadLine(input);
                c = img.ReadLine(input);
            }
            
            Assert.True(a == "Hello");
            Assert.True(b == "world!");
            Assert.True(c == "");
        }
        
        /*
        // Test alternativo con la funzione readline disponibile per gli oggetti di tipo streamreader
        [Fact]
        public void test_ReadLine2()
        {
            //Preparo il memory stream da leggere
            HdrImage img = new HdrImage(1,1);
            MemoryStream input = new MemoryStream();
            using (StreamWriter writer = new StreamWriter(input))
            {
                writer.Write("Hello\nworld!");
                //writer.Flush();
                //input.Position = 0;
            }
            
            //Leggo e confronto 
            string? a;
            string? b;
            string? c;
            using (StreamReader reader = new StreamReader(input))
            {
                a = reader.ReadLine();
                b = reader.ReadLine();
                c = reader.ReadLine();
            }
            
            Assert.True(b == "Hello");
            Assert.True(b == "world!");
            Assert.True(c == "");
        }
        //*/

        [Fact]
        public void test_ReadFilePFM()
        {
            Color a = new Color(1.0e1f, 2.0e1f, 3.0e1f);
            Color b = new Color(4.0e1f, 5.0e1f, 6.0e1f);
            Color c = new Color(7.0e1f, 8.0e1f, 9.0e1f);
            Color d = new Color(1.0e2f, 2.0e2f, 3.0e2f);
            Color e = new Color(4.0e2f, 5.0e2f, 6.0e2f);
            Color f = new Color(7.0e2f, 8.0e2f, 9.0e2f);
            HdrImage img = new HdrImage(1, 1);
            using (Stream fileStream = File.OpenRead(@"../../../../PGENLib.tests/reference_le.pfm"))
            { img = img.ReadPFMFile(fileStream); }

            Assert.True(img.Width == 3); //Come fa a sapere che voglio controllare Width di img? Alternativa: img.Width togliendo static
            Assert.True(img.Height == 2);

            Assert.True(Color.are_close(img.GetPixel(0,0), a));
            Assert.True(Color.are_close(img.GetPixel(1,0), b));
            Assert.True(Color.are_close(img.GetPixel(2,0), c));
            Assert.True(Color.are_close(img.GetPixel(0,1), d));
            Assert.True(Color.are_close(img.GetPixel(1,1), e));
            Assert.True(Color.are_close(img.GetPixel(2,1), f));
            
        }
        

        [Fact]
        public void test_AverageLum()
        {
            HdrImage img = new HdrImage(2, 1);
            img.SetPixel(0, 0, new Color(  5.0f,   10.0f,   15.0f)); // Lum = 10.0
            img.SetPixel(1, 0, new Color(  500.0f,   1000.0f,   1500.0f)); //Lum = 1000.0
            Assert.True(Math.Abs(img.AverageLum(0.0f) - 100.0f) < 1e-5);
        }


        [Fact]
        public void test_NormalizeImage()
        {
            HdrImage img = new HdrImage(2, 1);
            img.SetPixel(0, 0, new Color(  5.0f,   10.0f,   15.0f));
            img.SetPixel(1, 0, new Color(  500.0f,   1000.0f,   1500.0f));
            img.NormalizeImage(1000.0f, 100.0f);
            Assert.True(Color.are_close(img.GetPixel(0, 0),new Color(0.5e2f, 1.0e2f, 1.5e2f)));
            Assert.True(Color.are_close(img.GetPixel(1, 0),new Color(0.5e4f, 1.0e4f, 1.5e4f)));
        }


        [Fact]
        public void test_ClampImage()
        {
            HdrImage img = new HdrImage(2, 1);

            Color col1 = new Color(0.5e1f, 1.0e1f, 1.5e1f);
            Color col2 = new Color(0.5e3f, 1.0e3f, 1.5e3f);
            
            img.SetPixel(0, 0, col1);
            img.SetPixel(1, 0, col2);

            img.ClampImage();

            // Just check that the R/G/B values are within the expected boundaries
            for (int i = 0; i < img.Pixels.Length; i++)
            {
                Assert.True((img.Pixels[i].GetR() >= 0) & (img.Pixels[i].GetR() <= 1));
                Assert.True((img.Pixels[i].GetG() >= 0) & (img.Pixels[i].GetG() <= 1));
                Assert.True((img.Pixels[i].GetB() >= 0) & (img.Pixels[i].GetB() <= 1));
            }
        }
        
    }
    
}
