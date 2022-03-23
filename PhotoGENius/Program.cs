// See https://aka.ms/new-console-template for more information
using PGENLib;
using System.Runtime.CompilerServices;
using System.Text;

class Program
{
    static void Main()
    {
        // prova lettura da stream
        var img = new HdrImage(3, 2);
        
        using (Stream fileStream = File.OpenRead(@"..\..\..\..\PGENLib.tests\reference_le.pfm"))
        {
            HdrImage prova = img.ReadPFMFile(fileStream);
        }
        
        using (Stream outFileStream = File.OpenWrite("file.pfm"))
        {
            img.WritePFMFile(outFileStream, Endianness.BigEndian);
        }
        
        using (Stream outFileStream2 = File.OpenWrite("file2.pfm"))
        {
            img.WritePFMFile(outFileStream2, Endianness.LittleEndian);
        }
        
    }
}
