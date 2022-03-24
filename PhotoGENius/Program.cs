// See https://aka.ms/new-console-template for more information

using System.IO.Compression;
using PGENLib;
using System.Runtime.CompilerServices;
using System.Text;

class Program
{
    static void Main()
    {
        // prova lettura da stream
        /*HdrImage img2 = new HdrImage(3, 2);

        var col = new Color((float)1.0e1, (float)2.0e1, (float)3.0e1);
        img2.SetPixel(0, 0, col);
        col = new Color((float)4.0e1, (float)5.0e1, (float)6.0e1);
        img2.SetPixel(1, 0, col);
        col = new Color((float) 7.0e1, (float) 8.0e1, (float) 9.0e1);
        img2.SetPixel(2, 0, col);
        col = new Color((float)1.0e2, (float)2.0e2, (float)3.0e2);
        img2.SetPixel(0, 1, col);
        col = new Color((float)4.0e2, (float)5.0e2, (float)6.0e2);
        img2.SetPixel(1, 1, col);
        col = new Color((float)7.0e2, (float)8.0e2, (float)9.0e2);
        img2.SetPixel(2, 1, col);

        Stream buf = new MemoryStream();
        img2.WritePFMFile(buf, Endianness.BigEndian);
        */
        
        
        var img = new HdrImage(3, 2);
        
        // prova con BigEndianness
        HdrImage prova_be;
        
        using (Stream fileStream = File.OpenRead(@"..\..\..\..\PGENLib.tests\reference_be.pfm"))
        { prova_be = img.ReadPFMFile(fileStream); }

        using (Stream outFileStream = File.OpenWrite("file_BB.pfm"))
        { prova_be.WritePFMFile(outFileStream, Endianness.BigEndian); }
        
        using (Stream outFileStream2 = File.OpenWrite("file_BL.pfm"))
        { prova_be.WritePFMFile(outFileStream2, Endianness.LittleEndian); }
        
        // prova con BigEndianness
        HdrImage prova_le;
        
        using (Stream fileStream = File.OpenRead(@"..\..\..\..\PGENLib.tests\reference_le.pfm"))
        { prova_be = img.ReadPFMFile(fileStream); }

        using (Stream outFileStream = File.OpenWrite("file_LB.pfm"))
        { prova_be.WritePFMFile(outFileStream, Endianness.BigEndian); }
        
        using (Stream outFileStream2 = File.OpenWrite("file_LL.pfm"))
        { prova_be.WritePFMFile(outFileStream2, Endianness.LittleEndian); }
        
    }
}
