// See https://aka.ms/new-console-template for more information

using System.IO.Compression;
using PGENLib;
using System.Runtime.CompilerServices;
using System.Text;
using System.Runtime;

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
                throw new RuntimeEr("Usage: ./Program.exe INPUT_PFM_FILE FACTOR GAMMA OUTPUT_PNG_FILE");
                // come si fa l'equivalente in c#?
            }

            InputPfmFileName = argv[1];

            try { Factor =  Convert.ToSingle(argv[2]); }
            catch(ValueError)
            {
                throw RuntimeError($"Invalid factor ('{argv[2]}'), it must be a floating-point number.");
            }

            try { Gamma = Convert.ToSingle(argv[3]); }
            catch
            {
                ValueError:
                throw RuntimeError($"Invalid gamma ('{argv[3]}'), it must be a floating-point number.");
            }

            OutputPngFileName = argv[4];
        }
    }
    //*/
    
   //----------------------------------------------------------------------------------------------------------- 
    static void Main(string[] argv)
    {
        /*
        var img = new HdrImage(3, 2);
        
        // prova con Endianness
        HdrImage prova;
        
        using (Stream fileStream = File.OpenRead(@"..\..\..\..\PGENLib.tests\reference_be.pfm"))
        { prova = img.ReadPFMFile(fileStream); }

        using (Stream outFileStream = File.OpenWrite("file_BB.pfm"))
        { prova.WritePFMFile(outFileStream, Endianness.BigEndian); }
        
        using (Stream outFileStream2 = File.OpenWrite("file_BL.pfm"))
        { prova.WritePFMFile(outFileStream2, Endianness.LittleEndian); }
        
        using (Stream fileStream = File.OpenRead(@"..\..\..\..\PGENLib.tests\reference_le.pfm"))
        { prova = img.ReadPFMFile(fileStream); }

        using (Stream outFileStream = File.OpenWrite("file_LB.pfm"))
        { prova.WritePFMFile(outFileStream, Endianness.BigEndian); }
        
        using (Stream outFileStream2 = File.OpenWrite("file_LL.pfm"))
        { prova.WritePFMFile(outFileStream2, Endianness.LittleEndian); }
        //*/

        /*
        // main definitivo, una volta che funzionano readpfm e writepfm
        Parameters parameters = null;
        try
        {
            parameters.parse_command_line(argv);
        }
        catch
        {
            RuntimeError as err;
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
        //*/
    }
}
