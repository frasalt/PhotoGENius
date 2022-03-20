using System.Diagnostics;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PGENLib
{
    public class PfmImage
    {
        // funzione grossa che legge i file ed è composta da 4 funzioncine
        // public void ReadPFMFile();

        // funzione lettura di sequenza di 4 byte - CHI FINISCE PER PRIMO
        private float ReadFloat(Stream instr, double endianness)
        {
            byte[] bfloat = new byte[4] {0x00, 0x10, 0x20, 0x30};
            //bfloat = [ 0x00, 0x10, 0x20, 0x30];
            float ffloat = BitConverter.ToSingle(bfloat, 0);

            /*
            for (int i = 0; i < 4; i++)
            {
                float ffloat = ToSingle(byte[], 0);
            }
            */
            return ffloat;
        }

        // funzione di lettura di byte fino a \n - MARTINO
        /*private string ReadLine(Stream str)
        {
            byte[] result = Encoding.ASCII.GetBytes(str);
        }
        */

        // funzione lettura dim img - FRA
        // private ParseImgSize()
        
        /// <summary>
        ///  Funzione che legge l'endianness e restituisce se è little o big.
        /// </summary>
        
        
        // DUBBIO HO TRASFORMATO DA PRIVATE A PUBLIC PER VEDERE DAL MAIN SE FUNZIONA LA MIA PARTE
        // LA TERREI COMUNQUE PUBLIC.
        public static int ParseEndianness(double endianness)
        {
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
        }
    }
}

