// See https://aka.ms/new-console-template for more information

using System.IO.Compression;
using PGENLib;
using System.Runtime.CompilerServices;
using System.Text;
using System.Runtime;

class Program
{
    
    class Parameters
    {
        public string InputPfmFileName = "";
        public float Factor = 0.2f;
        public float Gamma = 1.0f;
        public string OutputPngFileName = "";
        public string Options = "";

        // costruttore vuoto
        public Parameters()
        {
            InputPfmFileName = "";
            Factor = 0.2f;
            Gamma = 1.0f;
            OutputPngFileName = "";
            Options = "";
        }

        public void parse_command_line(string[] argv)
        {
            if(argv.Length != 4 && argv.Length != 5)
            {
                throw new RuntimeError("Usage: ./ProgramName.exe INPUT_PFM_FILE.pfm FACTOR GAMMA OUTPUT_PNG_FILE OPTIONS");
            }// Option sta per???
            InputPfmFileName = argv[0];

            try { Factor =  Convert.ToSingle(argv[1]); }
            catch //(ValueError) // se la funzione .ToSingle lanciasse un value error in caso di problemi,
                                 // catch potrebbe catturarla e restituire in cambio un RunTimeError
            {
                throw new RuntimeError($"Invalid factor ('{argv[1]}'), it must be a floating-point number.");
            }

            try { Gamma = Convert.ToSingle(argv[2]); }
            catch //(ValueError) // idem come sopra
            {
                throw new RuntimeError($"Invalid gamma ('{argv[2]}'), it must be a floating-point number.");
            }

            OutputPngFileName = argv[3];
            if (argv.Length == 5) Options = argv[4]; //CHIEDERE SPIEGAZIONE
        }
    }

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
        
        // main definitivo, una volta che funzionano readpfm e writepfm
        Parameters parameters = new Parameters();
        try
        {
            parameters.parse_command_line(argv);
        }
        catch(RuntimeError err)
        {
            Console.WriteLine("Error: {0} ", err.Message); // funzionerà?
            return;
        }

        HdrImage img = new HdrImage(0,0);
        
        using (var inpf = new FileStream(parameters.InputPfmFileName, FileMode.Open, FileAccess.Read))
        {
            img = img.ReadPFMFile(inpf);
        }

        Console.WriteLine($" >> File {parameters.InputPfmFileName} has been read from disk.");

        img.NormalizeImage(parameters.Factor);
        img.ClampImage();

        if (parameters.Options == "")
        {
            try
            {
                string outf = "C:/Users/User/RiderProjects/PhotoGENius/prova";
                {
                    img.WriteLdrImage(outf, "PNG", parameters.Gamma);
                }

                Console.WriteLine($" >> File {parameters.OutputPngFileName} has been written to disk.");
            }
            catch (IOException)
            {
                Console.WriteLine(
                    "Error: file {0} already exists. Use another name, or add option \"overwrite\".",
                    parameters.OutputPngFileName);
            }
        }
        /*
        else if(parameters.Options == "overwrite")
        {
            using (var outf = new FileStream(parameters.OutputPngFileName, FileMode.Create))
            {
                img.WriteLdrImage(outf, "PNG", parameters.Gamma);
            }
            Console.WriteLine($" >> File {parameters.OutputPngFileName} has been written to disk.");
        }
        */

    }
}
