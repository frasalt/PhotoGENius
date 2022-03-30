// See https://aka.ms/new-console-template for more information

using System.IO.Compression;
using PGENLib;
using System.Runtime.CompilerServices;
using System.Text;

class Program
{
    /*
    class Parameters
    {
        public string InputPfmFileName = "";
        public float Factor = 0.2f;
        public float Gamma = 1.0f;
        public string OutputPngFileName = "";

        public void parse_command_line(string[] argv)
        {
            if(argv.Length != 5)
            {
                raise RuntimeError("Usage: ./Program.exe INPUT_PFM_FILE FACTOR GAMMA OUTPUT_PNG_FILE");
                // come si fa l'equivalente in c#?
            }

            InputPfmFileName = argv[1];

            try { Factor =  Convert.ToSingle(argv[2]); }
            catch
            {
                ValueError:
                raise RuntimeError($"Invalid factor ('{argv[2]}'), it must be a floating-point number.");
                // come si fa l'equivalente in c#?
            }

            try { Gamma = Convert.ToSingle(argv[3]); }
            catch
            {
                ValueError:
                raise RuntimeError($"Invalid gamma ('{argv[3]}'), it must be a floating-point number.");
                // come si fa l'equivalente in c#?
            }

            OutputPngFileName = argv[4];
        }
    }
    */
    
   //----------------------------------------------------------------------------------------------------------- 
    static void Main(string[] argv)
    {
        /*
        // prova lettura da stream
        HdrImage img2 = new HdrImage(3, 2);

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
        
        /*
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
        */

        /* // main definitivo, una volta che funzionano readpfm e writepfm
        Parameters parameters = null;
        try
        {
            parameters.parse_command_line(argv);
        }
        catch
        {
            RuntimeError as err:
            Console.WriteLine("Error: ", err);
            return;
        }

        HdrImage img = null;
        
        with open(parameters.InputPfmFileName, "rb") as inpf
        {
            img = img.ReadPFMFile(inpf);
        }

        Console.WriteLine($"File {parameters.InputPfmFileName} has been read from disk.");

        img.NormalizeImage(parameters.Factor);
        img.ClampImage();

        with open(parameters.OutputPngFileName, "wb") as outf:
        {
            img.write_ldr_image(stream = outf, "PNG", parameters.Gamma);
        }

        Console.WriteLine($"File {parameters.OutputPngFileName} has been written to disk.");
        */
    }
}
