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

using System.Diagnostics;
using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace PGENLib
{
    public enum Endianness
    {
        LittleEndian,
        BigEndian,
    }

    public class HdrImage
    {
        public int Width;
        public int Height;
        public Color[] Pixels; // Color type vector that contains all the pixels
        /// <summary>
        /// Constructor, empty or with pixels.
        /// </summary>
        public HdrImage(int WidthConstr, int HeightConstr, Color[]? pixels = null)
        {
            Width = WidthConstr;
            Height = HeightConstr;
            if (pixels == null)
            {
                pixels = new Color[Width * Height];
            }

            Pixels = pixels;
        }

        /// <summary>
        /// Check that given coordinates have values between 0 and the number of rows / columns.
        /// </summary>
        public bool ValidCoord(int x, int y)
        {
            return (x >= 0 && y >= 0 && x < Width && y < Height);
        }

        /// <summary>
        /// Given a pair of coordinates, it returns the position of the pixel in the storage vector.
        /// </summary>
        private int PixelOffset(int x, int y)
        {
            Debug.Assert(this.ValidCoord(x, y));
            return y * Width + x;
        }

        /// <summary>
        /// Sets the color of a pixel of given coordinates.
        /// </summary>
        public void SetPixel(int x, int y, Color newCol)
        {
            Debug.Assert(ValidCoord(x, y));
            Pixels[PixelOffset(x, y)] = newCol;
        }

        /// <summary>
        /// Given a pair of coordinates, it returns the color of the corresponding pixel.
        /// </summary>
        public Color GetPixel(int x, int y)
        {
            Debug.Assert(this.ValidCoord(x, y));
            return this.Pixels[this.PixelOffset(x, y)];
        }

        //=========================== FUNCTIONS FOR READING FROM FILE ======================================

        /// <summary>
        /// Function that reads a PFM file and writes the content to a new HDR image.
        /// </summary>

        public HdrImage ReadPFMFile(Stream input)
        {
            string magic = ReadLine(input);
            if (magic != "PF")
            {
                throw new InvalidPfmFileFormat("Invalid magic in PFM file");
            }

            string imgsize = ReadLine(input);
            int[] dim = ParseImgSize(imgsize);
            Width = dim[0];
            Height = dim[1];

            string endianness = ReadLine(input);
            int endi = ParseEndianness(endianness);
            Endianness end = Endianness.BigEndian;
            if (endi == -1) end = Endianness.LittleEndian;

            HdrImage myimg = new HdrImage(Width, Height);
            for (int y = Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < Width; x++)
                {
                    float r = ReadFloat(input, end);
                    float g = ReadFloat(input, end);
                    float b = ReadFloat(input, end);

                    Color newcol = new Color(r, g, b);

                    myimg.SetPixel(x, y, newcol);
                }
            }

            return myimg;
        }

        /// <summary>
        /// Function that reads a byte and transforms it into an ASCII character.
        /// </summary>
        
        public string ReadLine(Stream input) //DA SISTEMARE!
        {
            string str = "";
            byte[] mybyte = new byte[1];

            while (Encoding.ASCII.GetString(mybyte) != "\n") // verify that previous byte was not a return
            {
                mybyte[0] = (byte) input.ReadByte(); // overwrite current byte on previous byte
                
                // provare ad aggiungere la seconda condizione (lo stream è finito) (?)
                //if (Encoding.ASCII.GetString(mybyte) != "\n" && Encoding.ASCII.GetString(mybyte) != "")
                
                if (Encoding.ASCII.GetString(mybyte) != "\n") // verify current byte
                {
                    str += Encoding.ASCII.GetString(mybyte);
                }
            }

            return str;
        }
        
        /// <summary>
        /// Reads a 32-bit sequence from a stream and converts it to a floating-point number.
        /// </summary>

        public static float ReadFloat(Stream input, Endianness end)
        {
            byte[] bytes = new byte[4];

            try
            {
                bytes[0] = (byte) input.ReadByte();
                bytes[1] = (byte) input.ReadByte();
                bytes[2] = (byte) input.ReadByte();
                bytes[3] = (byte) input.ReadByte();
            }
            catch
            {
                throw new InvalidPfmFileFormat("Unable to read float!");
            }

            // I ask if the operating system is aligned with endianness, otherwise I overturn the bytes.
            if (end == Endianness.BigEndian && BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            if (end == Endianness.LittleEndian && !BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return BitConverter.ToSingle(bytes, 0);
        }

        /// <summary>
        /// Image size reading function.
        /// </summary>
        public int[] ParseImgSize(string str)
        {
            int[] dim = new int[2];
            string[] sub = str.Split();
            if (sub.Length != 2)
            {
                throw new InvalidPfmFileFormat("Invalid image size specification");
            }
            try
            {
                dim[0] = int.Parse(sub[0]);
                dim[1] = int.Parse(sub[1]);
            }
            catch
            {
                throw new InvalidPfmFileFormat("Second line of the header must be two spaced integers");
            }

            return dim;
        }

        /// <summary>
        ///  Function that reads the endianness and returns if it is little or big.
        /// </summary>
        public int ParseEndianness(string input)
        {
            double endianness = 0;
            try
            {
                endianness = Convert.ToDouble(input);
            }
            catch (ArgumentNullException ex)
            {
                throw new InvalidPfmFileFormat("Invalid Endianness specification");
            }

            if (endianness == 0)
            {
                throw new InvalidPfmFileFormat("Invalid Endianness specification");
            }
            double normEnd = endianness / Math.Abs(endianness);
            return (int) normEnd;
        }
        

        //=========================== FUNCTIONS FOR WRITING TO FILE ====================================

        /// <summary>
        /// Function that writes a PFM file from an HDR image.
        /// </summary>

        public void WritePFMFile(Stream output, Endianness endian)
        {
            Debug.Assert(endian is Endianness.LittleEndian or Endianness.BigEndian);

            double end = 0;
            if (endian == Endianness.LittleEndian) end = -1.0;
            else end = 1.0;

            // Convert the header to a sequence of bytes.
            var header = Encoding.ASCII.GetBytes($"PF\n{Width} {Height}\n{end}.0\n");
            output.Write(header);
            
            for (int y = Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < Width; x++)
                {
                    Color color = GetPixel(x, y);
                    WriteFloat(output, color.GetR(), endian);
                    WriteFloat(output, color.GetG(), endian);
                    WriteFloat(output, color.GetB(), endian);
                }
            }
        }

        /// <summary>
        /// Method of writing a 32-bit floating-point number in binary.
        /// </summary>

        private static void WriteFloat(Stream outputStream, float value, Endianness end)
        {
            var seq = BitConverter.GetBytes(value);
            if (end == Endianness.BigEndian && BitConverter.IsLittleEndian)
            {
                Array.Reverse(seq);
            }

            if (end == Endianness.LittleEndian && !BitConverter.IsLittleEndian)
            {
                Array.Reverse(seq);
            }

            outputStream.Write(seq, 0, seq.Length);
        }

        //=========================== LUMINOSITY OF THE PIXELS ==============================================
        /// <summary>
        /// Returns the average brightness of the image.
        /// </summary>
        public float AverageLum(double delta = 1e-10)
        {

            double average = 0.0;

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    average += (Math.Log(delta + this.GetPixel(i, j).Lum(), 10));
                }
            }

            return (float) Math.Pow(10, average / (Width * Height));

        }

        /// <summary>
        /// Calculate the average brightness of an image according to the axRi / <l> formula.
        /// </summary>
        public void NormalizeImage(float factor, float? luminosity = null)
        {
            var lum = luminosity ?? AverageLum();
            for (int i = 0; i < Pixels.Length; ++i)
            {
                Pixels[i] = Pixels[i] * (factor / lum);
            }
        }

        /// <summary>
        /// Maps a float from [0, + inf) to [0,1].
        /// </summary>
        public float ClampFloat(float x)
        {
            return x / (1 + x);
        }
        
        /// <summary>
        /// Apply correction for bright spots.
        /// </summary>
        public void ClampImage()
        {
            for (int i = 0; i < Pixels.Length; i++)
            {
                Pixels[i].SetR(ClampFloat(Pixels[i].GetR()));
                Pixels[i].SetG(ClampFloat(Pixels[i].GetG()));
                Pixels[i].SetB(ClampFloat(Pixels[i].GetB()));
            }
        }
        
        /// <summary>
        /// Convert an HDR image to LDR.
        /// </summary>
        
        public void WriteLdrImage(String output, String format, float gamma = 1.0f)
        {
            var img = new Image<Rgb24>(this.Width, this.Height);
            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    var curColor = this.GetPixel(x, y);
                    var red = (int)(255 * Math.Pow(curColor.r, 1.0f / gamma));
                    var green = (int)(255 * Math.Pow(curColor.g, 1.0f / gamma));
                    var blue = (int)(255 * Math.Pow(curColor.b, 1.0f / gamma));
                    img[x, y] = new Rgb24( Convert.ToByte(red), Convert.ToByte(green), Convert.ToByte(blue));
                }
            }
            
            using (Stream output1 = File.OpenWrite(output))
            {
                img.SaveAsPng(output1);
            }
        }
    }
}

    


