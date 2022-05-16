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

    using System.CommandLine;
    using PGENLib;
    using Color = PGENLib.Color;


    namespace PhotoGENius; 

    class Program
    {
        static Color WHITE = new Color(1.0f, 1.0f, 1.0f);
        static Color BLACK = new Color(0.0f, 0.0f, 0.0f);

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
                if (argv.Length != 4 && argv.Length != 5)
                {
                    throw new RuntimeError(
                        "Usage: ./PhotoGENius.exe INPUT_PFM_FILE.pfm FACTOR GAMMA OUTPUT_PNG_FILE OPTIONS");
                }

                // associo i comandi dell'utente ai parametri di funzionamento del programma
                InputPfmFileName = argv[0];

                try
                {
                    Factor = Convert.ToSingle(argv[1]);
                }
                catch
                {
                    throw new RuntimeError($"Invalid factor ('{argv[1]}'), it must be a floating-point number.");
                }

                try
                {
                    Gamma = Convert.ToSingle(argv[2]);
                }
                catch
                {
                    throw new RuntimeError($"Invalid gamma ('{argv[2]}'), it must be a floating-point number.");
                }

                OutputPngFileName = argv[3];
                if (argv.Length == 5) Options = argv[4];
            }
        }
        

        //==============================================================================================================
        //Prova demo con SystemCommandLine
        //==============================================================================================================
        static async Task<int> Main(string[] args)
        {
            var width = new Option<int>(
                name: "--width",
                description: "Width of the image to render.",
                getDefaultValue: () => 640);
            
            var height = new Option<int>(
                name: "--height",
                description: "Height of the image to render.",
                getDefaultValue: () => 480);
            
            var angleDeg = new Option<float>(
                name: "--angle-deg",
                description: "Angle of view in degrees.",
                getDefaultValue: () => 0.0f);
            
            var pfmOutput = new Option<string>(
                name: "--pfm-output",
                description: "Name of the PFM file to create.",
                getDefaultValue: () => "output.pfm");
            
            var pngOutput = new Option<string>(
                name: "--png-output",
                description: "Name of the PNG file to create.",
                getDefaultValue: () => "output.png");
            
            var cameraType = new Option<string>(
                name: "--camera-type",
                description: "Type of camera to be used: 'orthogonal' or 'perspective'.",
                getDefaultValue: () => "orthogonal");
            
            var algorithm = new Option<string>(
                name: "--algorithm",
                description: "Type of renderer to be used: 'flat' for colorful image or 'onoff' for black and white one.",
                getDefaultValue: () => "onoff");
            
            var demo = new RootCommand("Sample app for creating an image")
            {
                width,
                height,
                angleDeg,
                pfmOutput,
                pngOutput,
                cameraType,
                algorithm
            };

            demo.SetHandler((int widthValue, int heightValue, float angleDegValue, string pfmOutputValue,
                string pngOutputValue, string cameraType, string algorithmValue) =>
            {

                // 1.World initialization (10 spheres)
                var world = new World();

                //   sphere in vertices
                var scaling = Transformation.Scaling(new Vec(0.1f, 0.1f, 0.1f));
                Transformation transformation;

                for (var x = -0.5f; x <= 0.5f; x++)
                {
                    for (var y = -0.5f; y <= 0.5f; y++)
                    {
                        for (var z = -0.5f; z <= 0.5f; z++)
                        {
                            transformation = Transformation.Traslation(new Vec(x, y, z));

                            var sphere = new Sphere(transformation * scaling);
                            world.AddShape(sphere);
                        }
                    }
                }

                //   sphere in faces
                transformation = Transformation.Traslation(new Vec(0.0f, 0.0f, -0.5f));
                world.AddShape(new Sphere(transformation * scaling));

                transformation = Transformation.Traslation(new Vec(0.0f, 0.5f, 0.0f));
                world.AddShape(new Sphere(transformation * scaling));


                // 2.Camera initialization
                transformation = Transformation.Traslation(new Vec(-1.0f, 0.0f, 0.0f));
                var rotation = Transformation.RotationZ(angleDegValue);
                float aspectRatio = (float) widthValue / heightValue;
                //OrthogonalCamera camera = new OrthogonalCamera(4.0f / 3.0f, transformation);

                ICamera camera;
                if (cameraType == "perspective")
                {
                    camera = new PerspectiveCamera(1.0f, aspectRatio, rotation * transformation);
                }
                else
                {
                    camera = new OrthogonalCamera(aspectRatio, rotation * transformation);
                }

                // 3.(ruotare l'osservatore)

                // 4.Run raytracer
                var image = new HdrImage(800, 600);
                var tracer = new ImageTracer(image, camera);

                if (algorithmValue == "onoff")
                {
                    var renderer = new OnOffRenderer(world);
                    tracer.FireAllRays(renderer.Call);
                }
                else
                {
                    var renderer = new FlatRenderer(world);
                    tracer.FireAllRays(renderer.Call);
                }
            

            // 5.salvare PFM : <<<<<<<<<<<<<<<<<<< ATTENZIONE QUI: non funizona la scrittura SU FILE. pfm, per colpa dello stream.
                    var stream = new MemoryStream();
                    image.WritePFMFile(stream, Endianness.BigEndian);

                    //image.WritePFMFile(stream, Endianness.LittleEndian);

                    // 6.convertire a PNG
                    image.NormalizeImage(1.0f);
                    image.ClampImage();

                    // salvo in file PNG, a seconda delle opzioni
                    try
                    {
                        var outf = pngOutputValue;
                        {
                            image.WriteLdrImage(outf, "PNG", 0.2f);
                        }

                        Console.WriteLine($" >> File {pngOutputValue} has been written to disk.");
                    }
                    catch
                    {
                        Console.WriteLine(
                            $"Error: couldn't write file {pngOutputValue}");
                    }
                },
                width, height, angleDeg, pfmOutput, pngOutput, cameraType, algorithm );
            
            return await demo.InvokeAsync(args);
            
            //---------------------------------------------------------------------------------
            var factor = new Option<float>(
                name: "--factor",
                description: "Multiplicative factor.",
                getDefaultValue: () => 0.2f);
            
            var gamma = new Option<float>(
                name: "--gamma",
                description: "Value to be used for gamma correction.",
                getDefaultValue: () => 1.0f);
            
            var inputPfmFileName = new Option<string>(
                name: "--input-pfm",
                description: "PFM file to be converted.",
                getDefaultValue: () => "input.pfm");
            
            var outputPngFileName = new Option<string>(
                name: "--output-png",
                description: "PNG output file.",
                getDefaultValue: () => "output.png");

            var pfm2png = new RootCommand("Sample app for converting a PFM to a PNG.")
            {
                factor,
                gamma,
                inputPfmFileName,
                outputPngFileName
            };
            
            pfm2png.SetHandler((float factorValue, float gammaValue, string inputPfmFileNameValue, string outputPngFileNameValue) =>
                {
                    /*
            Parameters parameters = new Parameters();
        
            // riempio i parametri
            try { parameters.parse_command_line(argv); }
            catch (RuntimeError)
            {
                Console.WriteLine("Error: invalid number of parameters. Please, follow usage instructions.");
                return;
            }
            */
    
                    HdrImage img = new HdrImage(0,0);
            
                    // leggo l'immagine HDR in formato PFM
                    using (var inpf = new FileStream(inputPfmFileNameValue, FileMode.Open, FileAccess.Read))
                    { img = img.ReadPFMFile(inpf); }
    
                    Console.WriteLine($" >> File {inputPfmFileNameValue} has been read from disk.");
    
                    // converto i dati in formato LDR
                    img.NormalizeImage(factorValue);
                    img.ClampImage();
    
                    // salvo in file PNG, a seconda delle opzioni
                    try
                    {
                        string outf = outputPngFileNameValue;
                        {
                            img.WriteLdrImage(outf, "PNG", gammaValue);
                        }
    
                        Console.WriteLine($" >> File {outputPngFileNameValue} has been written to disk.");
                    }
                    catch
                    {
                        Console.WriteLine(
                            "Error: couldn't write file {0}.", outputPngFileNameValue);
                    }
                },
                factor, gamma, inputPfmFileName, outputPngFileName);
            
                
        }
        
        /*
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
                    transformation = Transformation.Traslation(new Vec(x, y, z));

                    Sphere sphere = new Sphere(transformation * scaling);
                    world.AddShape(sphere);
                }
            }
        }

        //   sphere in faces
        transformation = Transformation.Traslation(new Vec(0.0f, 0.0f, -0.5f));
        world.AddShape(new Sphere(transformation * scaling));

        transformation = Transformation.Traslation(new Vec(0.0f, 0.5f, 0.0f));
        world.AddShape(new Sphere(transformation * scaling));


        // 2.Camera initialization
        transformation = Transformation.Traslation(new Vec(-1.0f, 0.0f, 0.0f));
        Transformation rotation = Transformation.RotationZ(53.0f);
        //OrthogonalCamera camera = new OrthogonalCamera(4.0f / 3.0f, transformation);
        PerspectiveCamera camera = new PerspectiveCamera(1.0f, 4.0f / 3.0f, transformation*rotation);


        // 3.(ruotare l'osservatore)

        // 4.Run raytracer
        HdrImage image = new HdrImage(800, 600);
        ImageTracer tracer = new ImageTracer(image, camera);

        Color ComputeColor(Ray ray)
        {
            if (world.RayIntersection(ray) == null) return BLACK;
            else return WHITE;
        }

        tracer.FireAllRays(ComputeColor);

        // 5.salvare PFM : <<<<<<<<<<<<<<<<<<< ATTENZIONE QUI: non funizona la scrittura su file pfm, per colpa dello stream.
        MemoryStream stream = new MemoryStream();
        image.WritePFMFile(stream, Endianness.BigEndian);

        //image.WritePFMFile(stream, Endianness.LittleEndian);

        // 6.convertire a PNG
        image.NormalizeImage(1.0f);
        image.ClampImage();

        // salvo in file PNG, a seconda delle opzioni
        try
        {
            string outf = "image.png";
            {
                image.WriteLdrImage(outf, "PNG", 0.2f);
            }

            Console.WriteLine($" >> File image.png has been written to disk.");
        }
        catch
        {
            Console.WriteLine(
                "Error: couldn't write file image.png.");
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
    */

    }