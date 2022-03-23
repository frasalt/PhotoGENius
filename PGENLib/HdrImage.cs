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
        public static int Width;
        public static int Height;
        public Color[] Pixels; // un vettore di tipo Color che contiene tutti i pixel

        // Costruttori:
        
        /// <summary>
        /// Costruttore con pixel noti
        /// </summary>
        public HdrImage(int WidthConstr, int HeightConstr, Color[] pixels)
        {
            Width = WidthConstr;
            Height = HeightConstr;
            this.Pixels = pixels;
            Color col = new Color();
            
            // create an empty image
            for (int i = 0; i < Width * Height; i++)
            {
                this.Pixels[i] = col;
            }
        }
        
        /// <summary>
        /// Costruttore con pixel neri
        /// </summary>
        public HdrImage(int WidthConstr, int WeightConstr)
        {
            Width = WidthConstr;
            Height = WeightConstr;
            Color[] pixels = new Color[Width*Height];
            this.Pixels = pixels;
            Color col = new Color();
            
            // create an empty image
            for (int i = 0; i < Width * Height; i++)
            {
                this.Pixels[i] = col;
            }
        }
        
        /// <summary>
        /// Verifica che date coordinate abbiano valori sensati, ovvero compresi tra 0 e il numero di righe/colonne
        /// </summary>
        private bool ValidCoord(int x, int y)
        {
            return (x >= 0 && y >= 0 && x < Width && y < Height);
        }

        /// <summary>
        /// Data una coppia di coordinate, restituisce la posizione del pixel nel vettore di memorizzazione.
        /// </summary>
        private int PixelOffset(int x, int y)
        {
            Debug.Assert(this.ValidCoord(x, y)) ;
            return y * Width + x;
        }
        
        /// <summary>
        /// Imposta il colore di un pixel di date coordinate
        /// </summary>
        public void SetPixel(int x, int y, Color newCol)
        {
            Debug.Assert(ValidCoord(x, y));
            Pixels[PixelOffset(x, y)] = newCol;
        }
        
        /// <summary>
        /// Data una coppia di coordinate, restituisce il colore del pixel corrispondente
        /// </summary>
        private Color GetPixel(int x, int y)
        {
            Debug.Assert(this.ValidCoord(x, y)) ;
            return this.Pixels[this.PixelOffset(x,y)];
        }
        
        // =============== SEGUONO FUNZIONI PER LA LETTURA DA FILE ============
        
        /// <summary>
        /// funzione che legge un file PFM e scrive il contenuto in una nuova HDR image
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
            for (int y = Height-1; y >= 0; y--)
            {
                for (int x = 0; x < Width; x++)
                {
                    float r = ReadFloat(input, end);
                    float g = ReadFloat(input, end);
                    float b = ReadFloat(input, end);
                        
                    Color newcol = new Color(r, g, b);
                    //Console.WriteLine($"    red {newcol.GetR()}     green {newcol.GetG()}       blue {newcol.GetB()}");
                    
                    myimg.SetPixel(x, y, newcol);
                }
            }

            return myimg;
        }

        /// <summary>
        /// funzione che legge un byte e ne fa un carattere ascii
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string ReadLine(Stream input)
        {
            string str = "";
            byte[] mybyte = new byte[1];
            
            while (Encoding.ASCII.GetString(mybyte) != "\n")
            {
                mybyte[0] = (byte)input.ReadByte();
                if (Encoding.ASCII.GetString(mybyte) != "\n")
                {
                    str += Encoding.ASCII.GetString(mybyte);
                }
            }
            return str;
        }
        
        // per esaurimento sono andato a prendere la funzione da colleghi dell'anno scorso:
        // https://github.com/andreasala98/NM4PIG/blob/master/Trace/HdrImage.cs
        /// <summary>
        /// Read a 32bit sequence from a stream and convert it to floating-point number.
        /// </summary>
        /// <param name="input"> The input stream </param>
        /// <param name="end"> -1 if the image is little-endian, -1 if big-endian </param>
        /// <returns> Float value corresponding to 4-byte sequence</returns>
        public static float ReadFloat(Stream input, Endianness end)
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
                Console.WriteLine("ReadFloat: non ce lho fatta");
            //    throw new InvalidPfmFileFormat("Unable to read float!");
            }

            //if (end == Endianness.LittleEndian) Array.Reverse(bytes);
            if (end == Endianness.BigEndian) Array.Reverse(bytes); // così funzionicchia, ma non è al contrario??
            
            // oppure chiedo se il sistema operativo è allineato con la mia endianness. se NO, ribalto i byte.
            //if (end == Endianness.BigEndian && BitConverter.IsLittleEndian) Array.Reverse(bytes);
            //if (end == Endianness.LittleEndian && !BitConverter.IsLittleEndian) Array.Reverse(bytes);
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
            double normEnd = endianness / Math.Abs(endianness); 
            return (int)normEnd;
        }
        
        // RICORDIAMO: RAGIONARE SUL TENERE TUTTI QUESTI METODI PUBLIC
        
        // =============== SEGUONO FUNZIONI PER LA SCRITTURA SU FILE ============

        /// <summary>
        /// funzione che scrive un file PFM a partire da una HDR image
        /// </summary>
        /// <param name="output"></param>
        /// <param name="endian"></param>
        public void WritePFMFile(Stream output, Endianness endian)
        {
            Debug.Assert(endian is Endianness.LittleEndian or Endianness.BigEndian);

            double end = 0;
            if (endian == Endianness.LittleEndian) end = -1.0;
            else end = 1.0;
            
            // convert header into sequence of bytes
            var header = Encoding.ASCII.GetBytes($"PF\n{Width} {Height}\n{end}.0\n");
            output.Write(header);
            
            // write the image pixels
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

    


