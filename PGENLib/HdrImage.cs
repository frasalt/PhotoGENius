using System.Diagnostics;
using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

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
        public static int width;
        public static int height;
        public Color[] pixels; // un vettore di tipo Color che contiene tutti i pixel

        // Costruttori:
        
        /// <summary>
        /// Costruttore con pixel noti
        /// </summary>
        public HdrImage(int width_constr, int height_constr, Color[] pixels)
        {
            width = width_constr;
            height = height_constr;
            this.pixels = pixels;
            Color col = new Color();
            // create an empty image
            for (int i = 0; i < width * height; i++)
            {
                this.pixels[i] = col;
            }
        }
        
        /// <summary>
        /// Costruttore con pixel neri
        /// </summary>
        public HdrImage(int width_constr, int height_constr)
        {
            width = width_constr;
            height = height_constr;
            Color[] pixels = new Color[width*height];
            this.pixels = pixels;
            Color col = new Color();
            // create an empty image
            for (int i = 0; i < width * height; i++)
            {
                this.pixels[i] = col;
            }
        }
        
        /// <summary>
        /// Verifica che date coordinate abbiano valori sensati, ovvero compresi tra 0 e il numero di righe/colonne
        /// </summary>
        private bool ValidCoord(int x, int y)
        {
            return (x >= 0 && y >= 0 && x < width && y < height);
        }

        /// <summary>
        /// Data una coppia di coordinate, restituisce la posizione del pixel nel vettore di memorizzazione.
        /// </summary>
        private int PixelOffset(int x, int y)
        {
            Debug.Assert(this.ValidCoord(x, y)) ;
            return y * width + x;
        }
        
        /// <summary>
        /// Imposta il colore di un pixel di date coordinate
        /// </summary>
        private void SetPixel(int x, int y, Color new_col)
        {
            Debug.Assert(this.ValidCoord(x, y));
            this.pixels[this.PixelOffset(x, y)] = new_col;
        }
        
        /// <summary>
        /// Data una coppia di coordinate, restituisce il colore del pixel corrispondente
        /// </summary>
        private Color GetPixel(int x, int y)
        {
            Debug.Assert(this.ValidCoord(x, y)) ;
            return this.pixels[this.PixelOffset(x,y)];
        }
        
        // =============== SEGUONO FUNZIONI PER LA LETTURA DA FILE ============
        
        // funzione grossa che legge un file e lo scrive in una nuova HDR image
        public HdrImage ReadPFMFile(Stream input)
        {
            StreamReader reader = new StreamReader(input);
            
            string magic = reader.ReadLine();
            Debug.Assert(magic == "PF"); /*InvalidPfmFileFormat*/  
            Console.WriteLine(magic);                // commentabile
            
            string imgsize = reader.ReadLine();
            int[] dim = ParseImgSize(imgsize);
            Console.WriteLine($"{dim[0]} {dim[1]}"); // commentabile

            string endianness = reader.ReadLine();
            int end = ParseEndianness(endianness);
            Console.WriteLine(end);                  // commentabile
            
            // infine la lettura della seq di 4 bytes
            // che deve scrivere il vettore dei colori
            input.Position = reader.BaseStream.Position; // impunto lo stream alla stessa posizione a cui ero
                                                         // con lo StreamReader.  

            HdrImage myimg = new HdrImage(dim[0], dim[1]);
            for (int y = height-1; y >= 0; y--)
            {
                for (int x = 0; x < width; x++)
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
        
        // per esaurimento sono andato a prendere la funzione da colleghi dell'anno scorso:
        // https://github.com/andreasala98/NM4PIG/blob/master/Trace/HdrImage.cs
        /// <summary>
        /// Read a 32bit sequence from a stream and convert it to floating-point number.
        /// </summary>
        /// <param name="input"> The input stream </param>
        /// <param name="end"> -1 if the image is little-endian, -1 if big-endian </param>
        /// <returns> Float value corresponding to 4-byte sequence</returns>
        public static float ReadFloat(Stream input, int end)
        {
            byte[] bytes = new byte[4]; 

            try
            {
                bytes[0] = (byte)input.ReadByte();
                bytes[1] = (byte)input.ReadByte();
                bytes[2] = (byte)input.ReadByte();
                bytes[3] = (byte)input.ReadByte();
            }
            catch
            {
            //    throw new InvalidPfmFileFormat("Unable to read float!");
            }

            if (end == -1) Array.Reverse(bytes);
            return BitConverter.ToSingle(bytes, 0); // il dubbio rimane: la funzione ToSingle prende la
                                                            // endianness dal sistema operativo su cui sto eseguendo
        }

        /// <summary>
        /// funzione lettura dimensioni img - FRA
        /// </summary>
        public int[] ParseImgSize(string str)
        {
            string[] sub = str.Split();
            int[] dim = new int[2];
            
            dim[0] = int.Parse(sub[0]); 
            dim[1] = int.Parse(sub[1]);
            return dim;
        }
        // Va fatto meglio inserendo dei try per sollevare eccezioni

        /// <summary>
        ///  Funzione che legge l'endianness e restituisce se è little o big.
        /// </summary>
        public int ParseEndianness(string input)
        {
            double endianness = Convert.ToDouble(input);
            
            Debug.Assert(endianness != 0); 
            double norm_end = endianness / Math.Abs(endianness); 
            return (int)norm_end;
        }
        // DUBBIO: HO TRASFORMATO DA PRIVATE A PUBLIC PER VEDERE DAL MAIN SE FUNZIONA LA MIA PARTE
        // LA TERREI COMUNQUE PUBLIC.
        
        // =============== SEGUONO FUNZIONI PER LA SCRITTURA SU FILE ============

        public void WritePFMFile(Stream output, Endianness endian)
        {
            Debug.Assert(endian == Endianness.LittleEndian || endian == Endianness.BigEndian);

            double end = 0;
            if (endian == Endianness.LittleEndian) end = -1.0;
            else end = 1.0;
            
            // convert header into sequence of bytes
            var header = Encoding.ASCII.GetBytes($"PF\n{width} {height}\n{end}\n");
            output.Write(header);
            
            // write the image
            for (int y = height - 1; y >= 0; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    Color color = GetPixel(x, y);
                    WriteFloat(output, color.GetR(), endian); 
                    WriteFloat(output, color.GetG(), endian);
                    WriteFloat(output, color.GetB(), endian);
                }
            }
        }
        
        /// <summary>
        /// metodo di scrittura di un numero floating-point a 32 bit in binario
        /// </summary>
        /// <param name="outputStream"></param>
        /// <param name="value"></param>
        private static void WriteFloat(Stream outputStream, float value, Endianness end)
        {
            var seq = BitConverter.GetBytes(value);
            if (end == Endianness.BigEndian && BitConverter.IsLittleEndian) Array.Reverse(seq);
            if (end == Endianness.LittleEndian && !BitConverter.IsLittleEndian) Array.Reverse(seq);
            outputStream.Write(seq, 0, seq.Length);
        }

    }

} 

    


