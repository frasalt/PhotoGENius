using System;
using SixLabors.ImageSharp;

namespace PGENLib
{
    
    //==============================================================================================================
    //Ray
    //==============================================================================================================
    public struct Ray
    {
        public Point Origin;
        public Vec Dir;
        public float Tmin;
        public float Tmax;
        public int Depth;

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Ray()
        {
            Origin = new Point(0, 0, 0);
            Dir = new Vec(0, 0, 0);
            Tmin = 0.0f;
            Tmax = 0.0f;
            Depth = 0;
        }

        /// <summary>
        /// Constructor to create a Ray.
        /// </summary>
        public Ray(Point origin, Vec dir)
        {
            this.Origin = origin;
            this.Dir = dir;
            this.Tmin = 1e-5f;
            this.Tmax = float.PositiveInfinity;
            this.Depth = 0;
        }
        
        /// <summary>
        /// Constructor to create a Ray.
        /// </summary>
        public Ray(Point origin, Vec dir, float tmin, float tmax = float.PositiveInfinity, int depth = 0)
        {
            this.Origin = origin;
            this.Dir = dir;
            this.Tmin = tmin;
            this.Tmax = tmax;
            this.Depth = depth;
        }

        public Point get_Origin()
        {
            return Origin;
        }

        public Vec get_Dir()
        {
            return Dir;
        }

        public static bool are_close(Ray a, Ray b)
        {
            if (Point.are_close(a.Origin, b.Origin) & Vec.are_close(a.Dir, b.Dir))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Point At(float t)
        {
            return this.Origin + this.Dir * t;
        }

        public Ray Transform(Transformation tr)
        {
            var newRay = new Ray(tr*Origin, tr*Dir)
            {
                Tmax = this.Tmax,
                Tmin = this.Tmin,
                Depth = this.Depth
            };
            return newRay; 
        }
        
    }
    
    //==============================================================================================================
    //Cameras
    //==============================================================================================================
    /// <summary>
    /// Interface for camera: the method FireRay will be implemented in two different ways by OrthogonalCamera and
    /// PerspectiveCamera.
    /// </summary>
    public interface ICamera
    {
        Ray FireRay(float u, float v);

    }
  
    public struct OrthogonalCamera : ICamera
    {
        private float AspectRatio;
        private Transformation Transf;

        public OrthogonalCamera(float aspectRatio = 1.0f)
        {
            AspectRatio = aspectRatio;
            Transf = new Transformation();
        }
        
        public OrthogonalCamera(float aspectRatio, Transformation tr)
        {
            AspectRatio = aspectRatio;
            Transf = tr;
        }
        
        public Ray FireRay(float u, float v)
        {
            var origin = new Point(-1.0f, (1.0f - 2.0f * u) * this.AspectRatio, 2.0f * v - 1.0f);
            var direction = new Vec(1.0f, 0.0f, 0.0f);
            var rayToFire = new Ray(origin, direction);
                
            return rayToFire.Transform(this.Transf);
        }
    }
    
    public struct PerspectiveCamera : ICamera
    {
        private float ScreenDistance;
        private float AspectRatio;
        private Transformation Transf;
        
        public PerspectiveCamera()
        {
            ScreenDistance = 1.0f;
            AspectRatio = 1.0f;
            Transf = new Transformation();
        }
        
        // Martin: ho aggiunto questo costruttore che mi era comodo per il test di ImageTracer
        public PerspectiveCamera(float aspectRatio) 
        {
            ScreenDistance = 1.0f;
            AspectRatio = aspectRatio;
            Transf = new Transformation();
        }
        public PerspectiveCamera(float distance, float aspectRatio)
        {
            ScreenDistance = distance;
            AspectRatio = aspectRatio;
            Transf = new Transformation();
        }
        public PerspectiveCamera(float distance, float aspectRatio, Transformation tr)
        {
            ScreenDistance = distance;
            AspectRatio = aspectRatio;
            Transf = tr;
        }

        public Ray FireRay(float u, float v)
        {
            var origin = new Point(-this.ScreenDistance, 0.0f, 0.0f);
            var direction = new Vec(this.ScreenDistance, (1.0f - 2.0f * u) * this.AspectRatio, 2.0f * v - 1.0f);
            var rayToFire = new Ray(origin, direction, 1.0f);
                
            return rayToFire.Transform(this.Transf);
        }
    }
    
    //==============================================================================================================
    //ImageTracer
    //==============================================================================================================
    /// <summary>
    /// Send rays from the Camera (observer) to corresponding pixels of an HdrImage (screen),
    /// converting "u-v" Camera coordinates to "column-raw" index of the HdrImage.
    /// <list type="table">
    /// <item>
    ///     <term>Image</term>
    ///     <description> must be a :class:`.HdrImage` object that has already been initialized</description>
    /// </item>
    /// <item>
    ///     <term>Camera</term>
    ///     <description> must be a descendeant of the :class:`.Camera` object</description>
    /// </item>
    /// <item>
    ///     <term>Pcg</term>
    ///     <description> </description>
    /// </item>
    /// <item>
    ///     <term>SamplePerSize</term>
    ///     <description> If `SamplesPerSide` is larger than zero, stratified sampling will be applied to each pixel in the
    ///     image, using the random number generator `pcg`</description>
    /// </item>
    /// </list>
    /// </summary>
    public struct ImageTracer
    {
        public HdrImage Image;
        public ICamera Camera;
        public PCG Pcg;
        public int SamplePerSide;
        
        /// <summary>
        /// Constructor with parameters. If SamplePerSide is usefull to implement the antialiasing: if it's not zero,
        /// stratified sampling will be applied to each pixel in the image, using the random number generator `pcg`. 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="camera"></param>
        /// <param name="pcg"></param>
        /// <param name="samplePerSide"></param>
        public ImageTracer(HdrImage image, ICamera camera, PCG pcg, int samplePerSide = 0)
        {
            Image = image;
            Camera = camera;
            Pcg = pcg;
            SamplePerSide = samplePerSide;
            
        }
        
        /// <summary>
        /// Constructor with parameters. If SamplePerSide is usefull to implement the antialiasing: if it's not zero,
        /// stratified sampling will be applied to each pixel in the image, using the random number generator `pcg`. 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="camera"></param>
        /// <param name="samplePerSide"></param>
        public ImageTracer(HdrImage image, ICamera camera, int samplePerSide = 0)
        {
            Image = image;
            Camera = camera;
            Pcg = new PCG();
            SamplePerSide = samplePerSide;
            
        }

        /// <summary>
        /// Shoot one light ray through image pixel of coordinates (col, row), which are measured in the
        /// same way as in HdrImage: the bottom left corner is placed at (0, 0). The parameters (uPixel, vPixel) specify
        /// where the ray should cross the pixel: (0.5f, 0.5f) represents the pixel's center.
        /// </summary>
        /// <param name="col"> Type int</param> 
        /// <param name="row"> Type int</param>
        /// <param name="uPixel"> Type float in the range [0, 1], default = 0.5 </param>
        /// <param name="vPixel"> Type float in the range [0, 1], default = 0.5</param>
        /// <returns></returns>
        public Ray FireRay(int col, int row, float uPixel = 0.5f, float vPixel = 0.5f)
        {
            float u = (col + uPixel) / Image.Width;
            float v = 1.0f - (row + vPixel) / Image.Height;
            return Camera.FireRay(u, v);
        }

        /// <summary>
        /// Shoot several light rays crossing each of the pixels in the image. For each pixel of the `HdrImage`
        /// object, fire one ray and pass it to the function `func`, which must accept a `Ray` as its only
        /// parameter and must return a `Color` object, representing the color to assign to that pixel in the image.
        /// </summary>
        /// <param name="func"></param>
        
        public void FireAllRays (Func<Ray,Color> func)
        {
            for(int row = 0; row< Image.Height; row ++)
            {
                if(row%20 == 0) Console.WriteLine($"        Fill row {row}/{Image.Height}");
                for(int col = 0; col< Image.Width; col ++)
                {
                    var cumColor = new Color(); //Black
                    if (SamplePerSide > 0)
                    {
                        // Run stratified sampling over the pixel's surface.
                        for (int interPixelRow = 0; interPixelRow < SamplePerSide; interPixelRow++)
                        {
                            for (int interPixelCol = 0; interPixelCol < SamplePerSide; interPixelCol++)
                            {
                                var uPixel = (interPixelCol + Pcg.RandomFloat()) / SamplePerSide;
                                var vPixel = (interPixelRow + Pcg.RandomFloat()) / SamplePerSide;
                                var ray = FireRay(col, row, uPixel, vPixel);
                                cumColor += func(ray);
                                
                                Image.SetPixel(col, row, cumColor * (float)(1 / Math.Pow(SamplePerSide, 2.0f)));
                            }
                        }
                    }
                    
                }
            }
        }
        
    }
}