using System.Diagnostics;
using System;
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
            byte[] bfloat = new byte[4];
            bfloat =  [ 0x00, 0x10, 0x20, 0x30 ];
            float ffloat = BitConverter.ToSingle(bfloat, 0);

            /*
            for (int i = 0; i < 4; i++)
            {
                float ffloat = ToSingle(byte[], 0);
            }
            */
        }

        // funzione di lettura di byte fino a \n - MARTINO
        /*private string ReadLine(Stream str)
        {
            byte[] result = Encoding.ASCII.GetBytes(str);
        }
        */

        // funzione lettura dim img - FRA
        // private ParseImgSize()

        // funzione decod endianness - TERESA
        private int ParseEndiannes(double endianness)
        {
            Debug.Assert(endianness == 0);
            endianness = endianness / Math.Abs(endianness); // endiannes = +/- 1
        }
    }
}

