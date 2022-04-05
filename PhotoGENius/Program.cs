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

        /// <summary>
        /// Constructor of parameters
        /// </summary>
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
                throw new RuntimeError("Usage: ./PhotoGENius.exe INPUT_PFM_FILE.pfm FACTOR GAMMA OUTPUT_PNG_FILE OPTIONS");
            }
            
            // associo i comandi dell'utente ai parametri di funzionamento del programma
            InputPfmFileName = argv[0];

            try { Factor =  Convert.ToSingle(argv[1]); }
            catch { throw new RuntimeError($"Invalid factor ('{argv[1]}'), it must be a floating-point number."); }

            try { Gamma = Convert.ToSingle(argv[2]); }
            catch { throw new RuntimeError($"Invalid gamma ('{argv[2]}'), it must be a floating-point number."); }

            OutputPngFileName = argv[3];
            if (argv.Length == 5) Options = argv[4];
        }
    }

    //----------------------------------------------------------------------------------------------------------- 
    static void Main(string[] argv)
    {
        Parameters parameters = new Parameters();
        
        // riempio i parametri
        try { parameters.parse_command_line(argv); }
        catch (RuntimeError)
        {
            Console.WriteLine("Error: invalid number of parameters. Please, follow usage instructions.");
            return;
        }

        HdrImage img = new HdrImage(0,0);
        
        // leggo l'immagine HDR in formato PFM
        using (var inpf = new FileStream(parameters.InputPfmFileName, FileMode.Open, FileAccess.Read))
        { img = img.ReadPFMFile(inpf); }

        Console.WriteLine($" >> File {parameters.InputPfmFileName} has been read from disk.");

        // converto i dati in formato LDR
        img.NormalizeImage(parameters.Factor);
        img.ClampImage();

        // salvo in file PNG, a seconda delle opzioni
        if (parameters.Options == "")
        {
            try
            {
                string outf = parameters.OutputPngFileName;
                {
                    img.WriteLdrImage(outf, "PNG", parameters.Gamma);
                }

                Console.WriteLine($" >> File {parameters.OutputPngFileName} has been written to disk.");
            }
            catch
            {
                Console.WriteLine(
                    "Error: couldn't write file {0}.", parameters.OutputPngFileName);
            }
        }
        else if (parameters.Options != "")
        {
            Console.WriteLine("Advanced options not yet implemented: please, do not specify.");
        }

    }
}
