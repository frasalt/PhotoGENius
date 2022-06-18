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
        
        var rootCommand = new RootCommand("Sample app for creating an image or converting PMF file to PNG.");

        var width = new Option<int>(
            name: "--width",
            description: "Width of the image to render.",
            getDefaultValue: () => 640);
            
        var height = new Option<int>(
            name: "--height",
            description: "Height of the image to render.",
            getDefaultValue: () => 360);
            
        var angleDeg = new Option<float>(
            name: "--angle-deg",
            description: "Angle of view in degrees.",
            getDefaultValue: () => 0.0f);
            
        var pfmOutput = new Option<string>(
            name: "--pfm-output",
            description: "Name of the PFM file to create.",
            getDefaultValue: () => "../Media/imgs_pfm/output.pfm");
            
        var pngOutput = new Option<string>(
            name: "--png-output",
            description: "Name of the PNG file to create.",
            getDefaultValue: () => "../Media/imgs_png/output.png");
            
        var cameraType = new Option<string>(
            name: "--camera-type",
            description: "Type of camera to be used: 'orthogonal' or 'perspective'.",
            getDefaultValue: () => "perspective");
            
        var algorithm = new Option<string>(
            name: "--algorithm",
            description: "Type of renderer to be used: 'flat' for colorful image or 'onoff' for black and white one " +
                         "or 'pathtracing' or 'pointlight'  ",
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
            getDefaultValue: () => 4);
        
        //==============================================================================================================
        // Demo 
        //==============================================================================================================

        var demo = new Command("demo", "Create a demo image.")
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
                    
                // 1. World initialization

                var world = new World();
                
                // materials
                Console.WriteLine("\n=====================================");
                Console.WriteLine("Initializing materials...");
                
                var skyMaterial = new Material(
                    new UniformPigment(new Color(2f, 1.8f, 1f)),
                    new DiffuseBRDF(new UniformPigment(new Color(0f, 0f, 0f)))
                );
                var groundMaterial = new Material(
                    new DiffuseBRDF(new CheckeredPigment(new Color(0.3f, 0.5f, 0.1f), new Color(0.1f, 0.2f, 0.5f)))
                );
                var sphereMaterial = new Material(
                    new DiffuseBRDF(new UniformPigment(new Color(0.3f, 0.4f, 0.8f)))
                );
                var cylinderMaterial = new Material(
                    new DiffuseBRDF(new UniformPigment(new Color(0.8f, 0.4f, 0.3f)))
                );
                var mirrorMaterial = new Material(
                    new SpecularBRDF(new UniformPigment(new Color(0.6f, 0.2f, 0.3f)))
                );
                
                //---------------------------
                // example scene
                //---------------------------

                Console.WriteLine("\nInitializing shapes...");

                // sky
                world.AddShape(
                    new Sphere(Transformation.Scaling(new Vec(200f, 200f, 200f)) * Transformation.Translation(new Vec(0f, 0f, 0.4f)),skyMaterial)
                );
                // ground
                world.AddShape(
                    new XyPlane(Transformation.Scaling(new Vec(1f, 1f, 1f)), groundMaterial)
                );
                // sphere in the middle
                world.AddShape(
                    new Sphere(Transformation.Translation(new Vec(0f, -0.5f, 1.5f)),sphereMaterial)
                );
                // sphere aside
                world.AddShape(
                    new Sphere(Transformation.Translation(new Vec(1f, 2.5f, 0f)),mirrorMaterial)
                );
                // cylinder aside
                world.AddShape(
                    new Cylinder(Transformation.Translation(new Vec(0f, -0.5f, 0f)),cylinderMaterial,0f,0.5f, 6.29f, 1f)
                );
                // light source
                world.AddLight(new PointLight(new Point(-30f, 0f, 20f), new Color(1.0f, 1.0f, 1.0f)));
                    
                // 2. Camera initialization
                Console.WriteLine("\nInitializing camera...");

                Transformation transformation = Transformation.Translation(new Vec(-1.0f, 0f, 1f));
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

                // 3. Run raytracer
                Console.WriteLine("\nRunning raytracer...");

                var image = new HdrImage(widthValue, heightValue);
                Console.WriteLine(
                    $"    Generating a {widthValue}×{heightValue} image, with the camera tilted by {angleDegValue}°");

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
                else if (algorithmValue == "pointlight")
                {
                    Console.WriteLine("    Using pointlight tracer");
                    var renderer = new PointLightRenderer(world: world, backGroundColor: BLACK);
                    tracer.FireAllRays(renderer.Call);
                }
                else
                {
                    Console.WriteLine($"    [{algorithm}] is not valid. " +
                                      $"Available tracer are 'flat', 'onoff', 'pathtracing', 'pointlight'.");
                    return;
                }
                    
                // 4. Save PFM 
                Console.WriteLine("\nSaving PFM image...");
                    
                using FileStream fstream = File.OpenWrite(pfmOutputValue);

                try
                {
                    {
                        image.WritePFMFile(fstream, Endianness.BigEndian);
                        fstream.Close();
                    }

                    Console.WriteLine($"    File {pfmOutputValue} has been written to disk.");
                }
                catch
                {
                    Console.WriteLine(
                        $"Error: couldn't write file {pfmOutputValue}");
                }
                    
                // 6. Convert to PNG
                Console.WriteLine("\nConverting and saving PNG image...");
                    
                image.NormalizeImage(luminosityFactorValue);
                image.ClampImage();

                // save PNG file, depending on options
                try
                {
                    {
                        image.WriteLdrImage(pngOutputValue, "PNG", gammaFactorValue);
                    }

                    Console.WriteLine($"    File {pngOutputValue} has been written to disk.");
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
            getDefaultValue: () => "../InputSceneFiles/DEFAULT_INPUT.txt");

        var render = new Command("render", "Create an image.")
        {
            scenefile,
            width,
            height,
            angleDeg,
            pfmOutput,
            pngOutput,
            algorithm,
            raysNum,
            maxDepth,
            initState,
            initSeq,
            luminosityFactor,
            gammaFactor,
            samplePerPixel
        };
        rootCommand.AddCommand(render);
            
        render.SetHandler((string scenefileValue, int widthValue, int heightValue, float angleDegValue,
                string pfmOutputValue, string pngOutputValue, string algorithmValue, int raysNumValue,
                int maxDepthValue, ulong initStateValue, ulong initSeqValue, float luminosityFactorValue, float gammaFactorValue, int samplePerPixelValue) =>
            {
                // Compute number of sample per side, which will be used for antialiasing
                var samplePerSide = (int) Math.Sqrt(samplePerPixelValue);
                    
                if (Math.Abs(Math.Pow(samplePerSide, 2.0f) - samplePerPixelValue) > 10E-5)
                {
                    Console.WriteLine("Error: samplePerPixel must be a perfect square");
                    return;
                }
                Console.WriteLine("");
                Console.WriteLine("=====================================");
                Console.WriteLine("Initializing scene...");

                // 1. Read the scene from input file
                Stream sceneStream = new FileStream(scenefileValue, FileMode.Open);
                Dictionary<string, float> dict = new Dictionary<string, float>();
            
                Scene scene = new Scene();    
                scene = ExpectParse.parse_scene(new InputStream(sceneStream), dict);
                
                // Camera rotation
                if (angleDegValue != 0)
                    scene.Camera.SetTransf( Transformation.RotationZ(angleDegValue)*scene.Camera.GetTransf());

                // 2. Run raytracer
                Console.WriteLine("\nRunning raytracer...");

                var image = new HdrImage(widthValue, heightValue);
                Console.WriteLine(
                    $"    Generating a {widthValue}×{heightValue} image, with the camera tilted by {angleDegValue}°");
                
                var tracer = new ImageTracer(image, scene.Camera, samplePerSide);
                
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
                else if (algorithmValue == "pointlight")
                {
                    Console.WriteLine("    Using pointlight tracer");
                    var renderer = new PointLightRenderer(world: scene.World, backGroundColor: BLACK);
                    tracer.FireAllRays(renderer.Call);

                }
                else
                {
                    Console.WriteLine($"    [{algorithm}] is not valid. " +
                                      $"Available tracer are 'flat', 'onoff', 'pathtracing', 'pointlight'.");
                    return;
                }
              
                sceneStream.Close();

                // 3. Save PFM 
                Console.WriteLine("\nSaving PFM image...");
                
                using FileStream fstream = File.OpenWrite(pfmOutputValue);

                try
                {
                    {
                        image.WritePFMFile(fstream, Endianness.BigEndian);
                        fstream.Close();
                    }

                    Console.WriteLine($"    File {pfmOutputValue} has been written to disk.");
                }
                catch
                {
                    Console.WriteLine(
                        $"Error: couldn't write file {pfmOutputValue}");
                }
                
                // 4. Convert to PNG
                Console.WriteLine("\nConverting and saving PNG image...");
                
                image.NormalizeImage(luminosityFactorValue);
                image.ClampImage();

                // Save PNG file, depending on options
                try
                {
                    {
                        image.WriteLdrImage(pngOutputValue, "PNG", gammaFactorValue);
                    }

                    Console.WriteLine($"    File {pngOutputValue} has been written to disk.");
                }
                catch
                {
                    Console.WriteLine(
                        $"Error: couldn't write file {pngOutputValue}");
                }
            },

            scenefile, width, height, angleDeg, pfmOutput, pngOutput, algorithm, raysNum, maxDepth, 
            initState, initSeq, luminosityFactor, gammaFactor , samplePerPixel );
            
            
        //==============================================================================================================
        // Pfm2png 
        //==============================================================================================================
            
        var pfmInput = new Option<string>(
            name: "--pfm-input",
            description: "PFM file to be converted.",
            getDefaultValue: () => "../Media/Readme_imgs/memorial.pfm");
            
        var pfm2png = new Command("pfm2png", "Convert a PFM file to a PNG")
        {
            luminosityFactor,
            gammaFactor,
            pfmInput,
            pngOutput
        };
            
        rootCommand.AddCommand(pfm2png);
        pfm2png.SetHandler((float luminosityFactorValue, float gammaFactorValue, string pfmInputValue, string pngOutputValue) =>
            {

                HdrImage img = new HdrImage(0,0);
            
                // 1. Read HDR image and write to PFM file 
                
                Console.WriteLine($"Reading {pfmInputValue} ...");

                using (var inpf = new FileStream(pfmInputValue, FileMode.Open, FileAccess.Read))
                { img = img.ReadPFMFile(inpf); }
    
                Console.WriteLine($"    File {pfmInputValue} has been read from disk.");
    
                // 2. Convert to LDR
                
                Console.WriteLine("\nConverting to LDR format ...");

                img.NormalizeImage(luminosityFactorValue);
                img.ClampImage();
    
                // 3. Save to PNG file, depending on options
                
                Console.WriteLine("\nSaving as PNG ...");

                try
                {
                    string outf = pngOutputValue;
                    {
                        img.WriteLdrImage(outf, "PNG", gammaFactorValue);
                    }
    
                    Console.WriteLine($"    File {pngOutputValue} has been written to disk.");
                }
                catch
                {
                    Console.WriteLine(
                        "Error: couldn't write file {0}.", pngOutputValue);
                }
            },
            luminosityFactor, gammaFactor, pfmInput, pngOutput);
            
        return await rootCommand.InvokeAsync(args);
    }
   
}