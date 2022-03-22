using System.Diagnostics;
using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace PGENLib
{
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
            return (x > 0 && y > 0 && x < width && y < width);
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
            
            //Color[] colori = new Color[dim[0]*dim[1]];
            // infine la lettura della seq di 4 bytes
            // che deve scrivere il vettore dei colori
            input.Position = reader.BaseStream.Position; // impunto lo stream alla stessa posizione a cui ero
                                                         // con lo StreamReader.  

            HdrImage myimg = new HdrImage(dim[0], dim[1]);
            for (int y = height -1; y >=0; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    //(r, g, b) =  [ReadFloat(stream, endianness) for i in range(3)]
                    //result.set_pixel(x, y, Color(r, g, b))
                }
            }

            return myimg;
        }

        /// <summary>
        /// funzione lettura di sequenza di 4 byte - CHI FINISCE PER PRIMO
        /// </summary>
        /*public float ReadFloat(Stream input, int endianness)
        {
            unsafe
            {
                float num = 0; // che dovrò ritornare alla fine
            
                Int32 x = 0; // variabile di appoggio in cui mettere i 32 bit di stream  
            
                // un po' di giri per avere un vettore di byte a partire dall'input
                MemoryStream ms;
                ms = new MemoryStream();
                input.CopyTo(ms);
                byte[] bytes = ms.ToArray(); // problema: in questo modo non prende solo 32 bit, bensì tutti
                //Console.WriteLine(bytes[0]);

                // PROBLEMA: IN VERITà LA ENDIANNESS VIENE USATA NEL METODO ToUInt32 (https://referencesource.microsoft.com/#mscorlib/system/bitconverter.cs
                // righe 206-233), MA PRENDENDOLA DALLA CODIFICA CHE USA IL SISTEMA SU CUI SI STA OPERANDO (IsLittleEndian, riga 225).
                // Quindi qui è già tardi per settare la endianness corretta, e in generale a meno di sovrascrivere il metodo, non possiamo seguire
                // proprio questa strada :(
                // >> una cosa che si può pensare di fare è copiare da riga 220 a 231, modificando la condizione sulla endianness
                
                // per byte* ho dovuto fare ALLOW UNSAFE CONTENT IN THIS PROJECT, cosa significa? ci va bene?
                fixed(byte * pbyte = & bytes[0])
                {
                        if (endianness == -1)
                        {
                            x = (*pbyte) | (*(pbyte + 1) << 8) | (*(pbyte + 2) << 16) | (*(pbyte + 3) << 24);
                        }
                        else if (endianness == 1){
                            x = (*pbyte << 24) | (*(pbyte + 1) << 16)  | (*(pbyte + 2) << 8) | (*(pbyte + 3));                        
                        }
                }

                num = *(float*) & x; // cfr riga 332
                return num;
            }
        }*/
        
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
            return BitConverter.ToSingle(bytes, 0);
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
            
            /*
            int littleEnd = -1;
            int bigEnd = 1;
            Debug.Assert(endianness != 0); // il debug mi fa uscire il messaggio di errore solo se falsa.
            double End = endianness / Math.Abs(endianness); // endiannes = +/- 1
            if (End > 0)
            {
                return bigEnd;
            }
            else if (End < 0)
            {
                return littleEnd;
            }
            else return 0;
            */
            
            // più sinteticamente:
            Debug.Assert(endianness != 0); // il debug mi fa uscire il messaggio di errore solo se falsa.
            double norm_end = endianness / Math.Abs(endianness); // normalization
            return (int)norm_end;
        }
        // DUBBIO: HO TRASFORMATO DA PRIVATE A PUBLIC PER VEDERE DAL MAIN SE FUNZIONA LA MIA PARTE
        // LA TERREI COMUNQUE PUBLIC.
    }

} 

    


