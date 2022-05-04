    /*
    PhotoGENius : photorealistic images generation.
    Copyright (C) 2022  Lamorte Teresa, Salteri Francesca, Zanetti Martino

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
     */

using System.IO.Compression;
using System.Numerics;
using PGENLib;
using System.Runtime.CompilerServices;
using System.Text;
using System.Runtime;
using SixLabors.ImageSharp.Processing;

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

    static void Main()
    { 
        // 1.World initialization (10 spheres)
        World world = new World();

        //   sphere in vertices
        Transformation scaling = Transformation.Scaling(new Vec(0.1f, 0.1f, 0.1f));
        Transformation transformation;

        for (float x = -0.5f; x <= 0.5f; x++)
        {
            for (float y = -0.5f; y <= 0.5f; y++)
            {
                for (float z = -0.5f; z <= 0.5f; z++)
                {
                    transformation = Transformation.Traslation(new Vec(x,y,z));
                    
                    Sphere sphere = new Sphere(transformation*scaling);
                    world.AddShape(sphere); 
                }
            }
        }
        
        //   sphere in faces
        transformation = Transformation.Traslation(new Vec(0.0f,0.0f,-0.5f));
        world.AddShape(new Sphere(transformation*scaling));
        
        transformation = Transformation.Traslation(new Vec(0.0f,0.5f,0.0f));
        world.AddShape(new Sphere(transformation*scaling));
        

        // 2.Camera initialization
        transformation = Transformation.Traslation(new Vec(-1.0f,0.0f,0.0f));
        OrthogonalCamera camera = new OrthogonalCamera(4.0f/3.0f,transformation);
        

        // 3.(ruotare l'osservatore)

        // 4.Run raytracer
        Color WHITE = new Color(255, 255, 255);
        Color BLACK = new Color(0, 0, 0);

        HdrImage image = new HdrImage(400,300);
        ImageTracer tracer = new ImageTracer(image, camera);

        Color ComputeColor(Ray ray)
        {
            if (world.RayIntersection(ray)==null) return BLACK;
            else return WHITE;
        }
        
        tracer.FireAllRays(ComputeColor);

        // 5.salvare PFM
        Stream stream = new FileStream("image.pfm", FileMode.Open, FileAccess.Write);
        image.WritePFMFile(stream, Endianness.LittleEndian);

        // 6.convertire a PNG
        
    }
    
    //----------------------------------------------------------------------------------------------------------- 
    /*
    static void Pfm2Png(string[] argv)
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
    }*/
    
}

