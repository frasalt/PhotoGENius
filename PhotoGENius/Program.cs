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
    using System.Security.Cryptography.X509Certificates;
    using System.Runtime;
    using System.Text;
    using PGENLib;
    using Color = PGENLib.Color;
    using System.Diagnostics;


    namespace PhotoGENius; 

    class Program
    {
        static Color WHITE = new Color(1.0f, 1.0f, 1.0f);
        static Color BLACK = new Color(0.0f, 0.0f, 0.0f);

        static async Task<int> Main(string[] args)
        {
            //==============================================================================================================
            //Demo 
            //==============================================================================================================

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
                description: "Type of renderer to be used: 'flat' for colorful image or 'onoff' for black and white one or 'pathtracing'  ",
                getDefaultValue: () => "pathtracing");
            
            var raysNum = new Option<int>(
                name: "--num-of-rays",
                description: "Number of rays departing from each surface point (only applicable with --algorithm=pathtracing).",
                getDefaultValue: () => 10);
            
            var maxDepth = new Option<int>(
                name: "--max-depth",
                description: "Maximum allowed ray depth (only applicable with --algorithm=pathtracing).",
                getDefaultValue: () => 3);
            
            var initState = new Option<ulong>(
                name: "--init-state",
                description: "Initial seed for the random number generator (positive number, only applicable " +
                             "with --algorithm=pathtracing).",
                getDefaultValue: () => 45);
            
            var initSeq = new Option<ulong>(
                name: "--init-seq",
                description: "Identifier of the sequence produced by the random number generator (positive number, " +
                             "only applicable with --algorithm=pathtracing).",
                getDefaultValue: () => 54);
            
            var luminosityFactor = new Option<float>(
                name: "--lum-fac",
                description: "Regulates luminosity of rendered image.",
                getDefaultValue: () => 1.0f);
            
            var gammaFactor = new Option<float>(
                name: "--gamma-fac",
                description: "Regulates gamma compression of rendered image.",
                getDefaultValue: () => 1.8f);
            
            var samplePerPixel = new Option<int>(
                name: "--sample-per-pixel",
                description: "Number of sample per pixel (must be a perfect square).",
                getDefaultValue: () => 1);

            var rootCommand = new RootCommand("Sample app for creating an image or converting PMF file to PNG.");
            var demo = new Command("demo", "Create an image.")
            {
                width,
                height,
                angleDeg,
                pfmOutput,
                pngOutput,
                cameraType,
                algorithm,
                raysNum,
                maxDepth,
                initState,
                initSeq,
                luminosityFactor,
                gammaFactor,
                samplePerPixel
            };
            rootCommand.AddCommand(demo);
            
            demo.SetHandler((int widthValue, int heightValue, float angleDegValue, string pfmOutputValue,
                    string pngOutputValue, string cameraTypeValue, string algorithmValue, int raysNumValue, int maxDepthValue, ulong initStateValue,
                    ulong initSeqValue, float luminosityFactorValue, float gammaFactorValue, int samplePerPixelValue) =>
                { 
                    
                    var samplePerSide = (int) Math.Sqrt(samplePerPixelValue);
                    
                    if (Math.Abs(Math.Pow(samplePerSide, 2.0f) - samplePerPixelValue) > 10E-5)
                    {
                        Console.WriteLine("Error: samplePerPixel must be a perfect square");
                        return;
                    }
                    // 1.World initialization

                    var world = new World();
                
                    // materials
                    Console.WriteLine("");
                    Console.WriteLine("=====================================");
                    Console.WriteLine("Initializing materials...");
                
                    var skyMaterial = new Material(
                        //new UniformPigment(new Color(1.0f, 0.9f, 0.5f)),
                        new UniformPigment(new Color(2*1f, 2*0.9f, 2*0.5f)),
                        new DiffuseBRDF(new UniformPigment(new Color(0f, 0f, 1f)))
                    );
                    var groundMaterial = new Material(
                        new DiffuseBRDF(new CheckeredPigment(new Color(0.3f, 0.5f, 0.1f), new Color(0.1f, 0.2f, 0.5f)))
                    );
                    var sphereMaterial = new Material(
                        new DiffuseBRDF(new UniformPigment(new Color(0.3f, 0.4f, 0.8f)))
                    );
                    var mirrorMaterial = new Material(
                        new SpecularBRDF(new UniformPigment(new Color(0.6f, 0.2f, 0.3f)))
                    );

                    // shapes
                    Console.WriteLine("Initializing shapes...");

                    // sky
                    world.AddShape(
                        new Sphere(Transformation.Scaling(new Vec(200f, 200f, 200f)) * Transformation.Translation(new Vec(0f, 0f, 0.4f)),skyMaterial)
                    );
                    // ground
                    world.AddShape(
                        new XyPlane(Transformation.Scaling(new Vec(1f, 1f, 1f)),groundMaterial)
                    );
                
                    //sphere in the middle
                    world.AddShape(
                        new Sphere(Transformation.Translation(new Vec(0f, 0f, 1f)),sphereMaterial)
                    );
                    // sphere aside
                    world.AddShape(
                        new Sphere(Transformation.Translation(new Vec(1f, 2.5f, 0f)),mirrorMaterial)
                    );
                    
                    /*
                    //---------------------------
                    //tree
                    //---------------------------
                    //cilinder in the middle
                    world.AddShape(
                        new Cilinder( Transformation.Traslation(new Vec(0f, 0f, 1f)),sphereMaterial,
                            0.0f, 2.0f, 0.3f)
                    );
                    //Sphere
                    world.AddShape(
                        new Sphere(Transformation.Scaling(new Vec(2f, 2f, 2f))*Transformation.Traslation(new Vec(0f, 0f, 2.0f)),sphereMaterial)
                        );
                    //----------------------------
                    */

                
                    /*
                    //---------------------------
                    //cube with spheres
                    //---------------------------
                    // sphere in the middle
                    world.AddShape(
                        new Sphere(Transformation.Traslation(new Vec(0f, -2.5f, 1f)),sphereMaterial)
                    );
                    
                    //   sphere in vertices
                    var scaling = Transformation.Scaling(new Vec(0.1f, 0.1f, 0.1f));
                    Transformation transformation;
                    
                    var col1 = new Color(0.5f, 0.5f, 0.5f);
                    var col2 = new Color(0.0f, 1.0f, 0.0f);
                    var emittedRad = new CheckeredPigment(col1, col2, 4);
                    var material = new Material(emittedRad, new DiffuseBRDF());
    
                    for (var x = -0.5f; x <= 0.5f; x++)
                    {
                        for (var y = -0.5f; y <= 0.5f; y++)
                        {
                            for (var z = -0.5f; z <= 0.5f; z++)
                            {
                                transformation = Transformation.Traslation(new Vec(x, y, z));
    
                                var sphere = new Sphere(transformation * scaling, material);
                                world.AddShape(sphere);
                            }
                        }
                    }
    
                    //   sphere in faces
                    var red = new Color(1.0f, 0.0f, 0.0f);
                    var blue = new Color(0.0f, 0.0f, 1.0f);
                    var redEmittedRad = new UniformPigment(red);
                    var blueEmittedRad = new UniformPigment(blue);
                    var redMaterial = new Material(redEmittedRad, new DiffuseBRDF());
                    var blueMaterial = new Material(blueEmittedRad, new DiffuseBRDF());
    
                    transformation = Transformation.Translation(new Vec(0.0f, 0.0f, -0.5f));
                    world.AddShape(new Sphere(transformation * scaling, blueMaterial));
    
                    transformation = Transformation.Translation(new Vec(0.0f, 0.5f, 0.0f));
                    world.AddShape(new Sphere(transformation * scaling, redMaterial));
                    //---------------------------
                    */
                
                    // 2.Camera initialization
                    Console.WriteLine("Initializing camera...");

                    Transformation transformation = Transformation.Translation(new Vec(-3.0f, 1.0f, 1.5f));
                    var rotation = Transformation.RotationZ(angleDegValue);
                    float aspectRatio = (float) widthValue / heightValue;

                    ICamera camera;
                    if (cameraTypeValue == "perspective")
                    {
                        camera = new PerspectiveCamera(1.0f, aspectRatio, rotation * transformation);
                    }
                    else if(cameraTypeValue == "orthogonal")
                    {
                        camera = new OrthogonalCamera(aspectRatio, rotation * transformation);
                    }
                    else
                    {
                        throw new Exception("Invalid camera option: use orthogonal, or perspective");
                    }

                    // 3.(ruotare l'osservatore)

                    // 4.Run raytracer
                    Console.WriteLine("Running raytracer...");

                    var image = new HdrImage(widthValue, heightValue);
                    Console.WriteLine($"    Generating a {widthValue}×{heightValue} image, with the camera tilted by {angleDegValue}°");

                    var tracer = new ImageTracer(image, camera, samplePerSide);
                
                    if (algorithmValue == "onoff")
                    {
                        Console.WriteLine("    Using on/off renderer");
                        var renderer = new OnOffRenderer(world);

                        tracer.FireAllRays(renderer.Call);
                    
                    }
                    else if(algorithmValue == "flat")
                    {
                        Console.WriteLine("    Using flat renderer");
                        var renderer = new FlatRenderer(world);
                        tracer.FireAllRays(renderer.Call);
                    }
                    else if (algorithmValue == "pathtracing")
                    {
                        Console.WriteLine("    Using pathtracing");
                        var renderer = new PathTracer(
                            world,
                            new PCG(initStateValue, initSeqValue), 
                            raysNumValue, 
                            maxDepthValue
                        );
                        tracer.FireAllRays(renderer.Call);
                    }
                

                    // 5.salvare PFM 
                    var stream = new MemoryStream();
                    using FileStream fstream = File.OpenWrite(pfmOutputValue);

                    image.WritePFMFile(stream, Endianness.BigEndian); // salvo le immagini in memoria...
                    try // ... e le scrivo su file
                    {
                        var outf = pfmOutputValue;
                        {
                            image.WritePFMFile(fstream, Endianness.BigEndian);
                        }

                        Console.WriteLine($" >> File {pfmOutputValue} has been written to disk.");
                    }
                    catch
                    {
                        Console.WriteLine(
                            $"Error: couldn't write file {pfmOutputValue}");
                    }
                    
                    // 6.convertire a PNG
                    image.NormalizeImage(luminosityFactorValue);
                    image.ClampImage();

                    // salvo in file PNG, a seconda delle opzioni
                    try
                    {
                        var outf = pngOutputValue;
                        {
                            image.WriteLdrImage(outf, "PNG", gammaFactorValue);
                        }

                        Console.WriteLine($" >> File {pngOutputValue} has been written to disk.");
                    }
                    catch
                    {
                        Console.WriteLine(
                            $"Error: couldn't write file {pngOutputValue}");
                    }
                    
                },
                width, height, angleDeg, pfmOutput, pngOutput, cameraType, algorithm, raysNum, maxDepth, 
                initState, initSeq, luminosityFactor, gammaFactor , samplePerPixel );
            
            //==============================================================================================================
            // Render 
            //==============================================================================================================

            var scenefile = new Option<string>(
                name: "--file-name",
                description: "Input file for scene description",
                getDefaultValue: () => "input_file.txt");

            var render = new Command("render", "Create an image.")
            {
                scenefile,
                width,
                height,
                angleDeg,
                pfmOutput,
                pngOutput,
                cameraType,
                algorithm,
                raysNum,
                maxDepth,
                initState,
                initSeq
            };
            rootCommand.AddCommand(render);
            
            render.SetHandler((string scenefileValue, int widthValue, int heightValue, float angleDegValue, string pfmOutputValue,
                    string pngOutputValue, string cameraTypeValue, string algorithmValue, int raysNumValue, int maxDepthValue, ulong initStateValue,
                    ulong initSeqValue, float luminosityFactorValue, float gammaFactorValue, int samplePerPixelValue) =>
                {
                    Stream scene_stream = new FileStream(scenefileValue, FileMode.Open);
                    Dictionary<string, float> dict = new Dictionary<string, float>();


                    Scene scene = new Scene();
                    
                    scene = ExpectParse.parse_scene(new InputStream(scene_stream), dict);

                    Console.WriteLine("Number of shapes in world: "+ scene.World.Shapes.Count);
                    Console.WriteLine(scene.World.Shapes);
                    
                    // 4.Run raytracer

                    var image = new HdrImage(widthValue, heightValue);
                    Console.WriteLine($"Generating a {widthValue}×{heightValue} image, with the camera tilted by {angleDegValue}°");

                    var tracer = new ImageTracer(image, scene.Camera);
                
                    if (algorithmValue == "onoff")
                    {
                        Console.WriteLine("Using on/off renderer");
                        var renderer = new OnOffRenderer(scene.World);
                        tracer.FireAllRays(renderer.Call);
                    }
                    else if(algorithmValue == "flat")
                    {
                        Console.WriteLine("Using flat renderer");
                        var renderer = new FlatRenderer(scene.World);
                        tracer.FireAllRays(renderer.Call);
                    }
                    else if (algorithmValue == "pathtracing")
                    {
                        Console.WriteLine("Using pathtracing");
                        var renderer = new PathTracer(
                            scene.World,
                            new PCG(initStateValue, initSeqValue), 
                            raysNumValue, 
                            maxDepthValue
                        );
                        tracer.FireAllRays(renderer.Call);
                    }
                

                    // 5.salvare PFM 
                    Console.WriteLine("Saving PFM image...");

                    var stream = new MemoryStream();
                    using FileStream fstream = File.OpenWrite(pfmOutputValue);

                    image.WritePFMFile(stream, Endianness.BigEndian); // salvo le immagini in memoria...
                    try // ... e le scrivo su file
                    {
                        var outf = pfmOutputValue;
                        {
                            image.WritePFMFile(fstream, Endianness.BigEndian);
                        }

                        Console.WriteLine($"    File {pfmOutputValue} has been written to disk.");
                    }
                    catch
                    {
                        Console.WriteLine(
                            $"Error: couldn't write file {pfmOutputValue}");
                    }
                    
                    // 6.convertire a PNG
                    Console.WriteLine("Converting and saving PNG image...");

                    image.NormalizeImage(luminosityFactorValue);
                    image.ClampImage();

                    // salvo in file PNG, a seconda delle opzioni
                    try
                    {
                        var outf = pngOutputValue;
                        {
                            image.WriteLdrImage(outf, "PNG", gammaFactorValue);
                        }

                        Console.WriteLine($"    File {pngOutputValue} has been written to disk.");
                    }
                    catch
                    {
                        Console.WriteLine(
                            $"Error: couldn't write file {pngOutputValue}");
                    }
                    
                },
                scenefile, width, height, angleDeg, pfmOutput, pngOutput, cameraType, algorithm, raysNum, maxDepth, 
                initState, initSeq, luminosityFactor, gammaFactor , samplePerPixel );
            
            
            //==============================================================================================================
            //Pfm2png con SystemCommandLine
            //==============================================================================================================

            var factor = new Option<float>(
                name: "--factor",
                description: "Multiplicative factor.",
                getDefaultValue: () => 0.2f);
            
            var gamma = new Option<float>(
                name: "--gamma",
                description: "Exponent for gamma correction.",
                getDefaultValue: () => 1.0f);
            
            var inputPfmFileName = new Option<string>(
                name: "--input-pfm",
                description: "PFM file to be converted.",
                getDefaultValue: () => "input.pfm");
            
            var outputPngFileName = new Option<string>(
                name: "--output-png",
                description: "PNG output file.",
                getDefaultValue: () => "output.png");

            // ----- << linee cambiate da martin
            var pfm2png = new Command("pfm2png", "Convert a PFM file to a PNG")
            {
                factor,
                gamma,
                inputPfmFileName,
                outputPngFileName
            };
            rootCommand.AddCommand(pfm2png);
            // ----- >>

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
            
            return await rootCommand.InvokeAsync(args);
        }
   
    }