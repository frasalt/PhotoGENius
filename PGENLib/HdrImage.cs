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
        // attributi dell'immagine
        public int Width;
        public int Height;
        public Color[] Pixels; // Vettore di tipo Color che contiene tutti i pixel

        /// <summary>
        /// Costruttore, vuoto o con pixel.
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
        /// Verifica che date coordinate abbiano valori compresi tra 0 e il numero di righe/colonne.
        /// </summary>
        public bool ValidCoord(int x, int y)
        {
            return (x >= 0 && y >= 0 && x < Width && y < Height);
        }

        /// <summary>
        /// Data una coppia di coordinate, restituisce la posizione del pixel nel vettore di memorizzazione.
        /// </summary>
        private int PixelOffset(int x, int y)
        {
            Debug.Assert(this.ValidCoord(x, y));
            return y * Width + x;
        }

        /// <summary>
        /// Imposta il colore di un pixel di date coordinate.
        /// </summary>
        public void SetPixel(int x, int y, Color newCol)
        {
            Debug.Assert(ValidCoord(x, y));
            Pixels[PixelOffset(x, y)] = newCol;
        }

        /// <summary>
        /// Data una coppia di coordinate, restituisce il colore del pixel corrispondente.
        /// </summary>
        public Color GetPixel(int x, int y)
        {
            Debug.Assert(this.ValidCoord(x, y));
            return this.Pixels[this.PixelOffset(x, y)];
        }

        //=========================== FUNZIONI PER LA LETTURA DA FILE ======================================

        /// <summary>
        /// Funzione che legge un file PFM e scrive il contenuto in una nuova immagine HDR.
        /// </summary>

        public HdrImage ReadPFMFile(Stream input)
        {
            string magic = ReadLine(input);
            Debug.Assert(magic == "PF");

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
        /// Funzione che legge un byte e lo trasforma in un carattere ASCII.
        /// </summary>

        public string ReadLine(Stream input)
        {
            string str = "";
            byte[] mybyte = new byte[1];

            while (Encoding.ASCII.GetString(mybyte) != "\n")
            {
                mybyte[0] = (byte) input.ReadByte();
                if (Encoding.ASCII.GetString(mybyte) != "\n")
                {
                    str += Encoding.ASCII.GetString(mybyte);
                }
            }

            return str;
        }
        
        /// <summary>
        /// Legge una sequenza di 32 bit da uno stream e la converte in un numero floating-point.
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
                //throw new InvalidPfmFileFormat("Unable to read float!");
            }

            // Chiedo se il sistema operativo è allineato con l'endianness, altriemnti ribalto i byte.
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
        /// Funzione di lettura delle dimensioni delle immagini.
        /// </summary>
        public int[] ParseImgSize(string str)
        {
            int[] dim = new int[2];
            try
            {
                string[] sub = str.Split();
                dim[0] = int.Parse(sub[0]);
                dim[1] = int.Parse(sub[1]);
            }
            catch
            {
                //throw new InvalidPfmFileFormat("Second line of the header must be two spaced integers");
            }

            return dim;
        }

        /// <summary>
        ///  Funzione che legge l'endianness e restituisce se è little o big.
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
                Console.WriteLine(ex.Message);
                Console.WriteLine("Missing Endianness specification");
            }

            Debug.Assert(endianness != 0);
            double normEnd = endianness / Math.Abs(endianness);
            return (int) normEnd;
        }
        

        //=========================== FUNZIONI PER LA SCRITTURA SU FILE ====================================

        /// <summary>
        /// Funzione che scrive un file PFM a partire da un'immagine HDR.
        /// </summary>

        public void WritePFMFile(Stream output, Endianness endian)
        {
            Debug.Assert(endian is Endianness.LittleEndian or Endianness.BigEndian);

            double end = 0;
            if (endian == Endianness.LittleEndian) end = -1.0;
            else end = 1.0;

            // Converte l' header in una sequenza di byte.
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
        /// Metodo di scrittura di un numero floating-point a 32 bit in binario.
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

        //=========================== LUMINOSITA' DEI PIXEL==================================================
        /// <summary>
        /// Restituisce la luminosità media dell'immagine.
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
        /// Calcola la luminosità media di un’immagine secondo la formula axRi/<l>.
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
        /// Mappa un float da [0,+inf) a [0,1].
        /// </summary>
        public float ClampFloat(float x)
        {
            return x / (1 + x);
        }
        
        /// <summary>
        /// Applica la correzione per i punti luminosi.
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
        /// Converte un'immagine HDR in LDR.
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

    


