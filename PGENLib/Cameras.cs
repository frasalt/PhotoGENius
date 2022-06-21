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

namespace PGENLib
{
    
    //==============================================================================================================
    //Ray
    //==============================================================================================================
    /// The class contains the following members:
    /// <list type="table">
    /// <item>
    ///     <term>Origin</term>
    ///     <description> the 3D `Point` where the ray originated</description>
    /// </item>
    /// <item>
    ///     <term>Dir</term>
    ///     <description> the 3D direction (`Vec`) along which this ray propagates</description>
    /// </item>
    /// <item>
    ///     <term>Tmin</term>
    ///     <description> the minimum float distance travelled by the ray is this number times `dir`</description>
    /// </item>
    /// <item>
    ///     <term>Tmax</term>
    ///     <description> the maximum distance travelled by the ray is this number times `dir`</description>
    /// </item>
    /// <item>
    ///     <term>Depth</term>
    ///     <description> `int` number of times this ray was reflected/refracted</description>
    /// </item>
    /// </list>
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
            Origin = origin;
            Dir = dir;
            Tmin = 1e-5f;
            Tmax = float.PositiveInfinity;
            Depth = 0;
        }
        
        /// <summary>
        /// Constructor to create a Ray.
        /// </summary>
        public Ray(Point origin, Vec dir, float tmin, float tmax = float.PositiveInfinity, int depth = 0)
        {
            Origin = origin;
            Dir = dir;
            Tmin = tmin;
            Tmax = tmax;
            Depth = depth;
        }
        
        /// <summary>
        /// Check if two rays are similar enough to be considered equal.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool are_close(Ray a, Ray b)
        {
            if (Point.are_close(a.Origin, b.Origin) & Vec.are_close(a.Dir, b.Dir))
            {
                return true;
            }
            return false;
        }

        public Point At(float t)
        {
            return Origin + Dir * t;
        }

        /// <summary>
        /// This method returns a new ray whose origin and direction are the transformation of the original ray.
        /// </summary>
        /// <param name="tr"></param>
        /// <returns></returns>
        public Ray Transform(Transformation tr)
        {
            return new Ray(tr * Origin, tr * Dir, Tmin, Tmax, Depth);
        }
        
    }
    
    //==============================================================================================================
    //Cameras
    //==============================================================================================================
    /// <summary>
    /// An interface representing an observer.
    /// Interface for camera: the method FireRay will be implemented in two different ways by OrthogonalCamera and
    /// PerspectiveCamera.
    /// </summary>
    public interface ICamera
    {
        Ray FireRay(float u, float v);
        Transformation GetTransf();
        void SetTransf(Transformation tr);
    }
    
    /// <summary>
    /// This struct implements an observer seeing the world through an orthogonal projection.
    /// </summary>
    public struct OrthogonalCamera : ICamera
    {
        
        /// <summary>
        /// The parameter `aspect_ratio` defines how larger than the height is the image. For fullscreen
        /// images, you should probably set `aspect_ratio` to 16/9, as this is the most used aspect ratio
        /// used in modern monitors.
        /// </summary>
        public float AspectRatio;
        public Transformation Transf;

        public Transformation GetTransf()
        {
            return Transf;
        }
        
        public void SetTransf(Transformation tr)
        {
            Transf = tr;
        }


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
        
       
        /// <summary>
        /// Shoot a ray through the camera's screen
        /// </summary>
        /// <param name="u">x-axis</param>
        /// <param name="v">y-axis</param>
        /// <returns></returns>
        /// The coordinates (u, v) specify the point on the screen where the ray crosses it.
        /// The following diagram represents the coordinates (0,0); (1,0); (0,1); (1,1):
        ///   (0, 1)                          (1, 1)
        ///      +------------------------------+
        ///      |                              |
        ///      |                              |
        ///      |                              |
        ///      +------------------------------+
        ///   (0, 0)                          (1, 0)
        public Ray FireRay(float u, float v)
        {

            var origin = new Point(-1.0f, (1.0f - 2.0f * u) * this.AspectRatio, 2.0f * v - 1.0f);
            var direction = new Vec(1.0f, 0.0f, 0.0f);
            var rayToFire = new Ray(origin, direction);
                
            return rayToFire.Transform(this.Transf);
        }
    }
    
   /// <summary>
   /// This class implements an observer seeing the world through a perspective projection.
   /// </summary>
    public struct PerspectiveCamera : ICamera
    {
        /// <summary>
        /// The parameter `screen_distance` tells how far from the eye of the observer is the screen,
        /// and it influences the so-called «aperture» (the field-of-view angle along the horizontal direction).
        /// </summary>
        public float ScreenDistance;
        /// <summary>
        /// The parameter `aspect_ratio` defines how larger than the height is the image. For fullscreen
        /// images, you should probably set `aspect_ratio` to 16/9, as this is the most used aspect ratio
        /// used in modern monitors.
        /// </summary>
        public float AspectRatio;
        public Transformation Transf;
        
        public Transformation GetTransf()
        {
            return Transf;
        }
        public void SetTransf(Transformation tr)
        {
            Transf = tr;
        }

        
        public PerspectiveCamera()
        {
            ScreenDistance = 1.0f;
            AspectRatio = 1.0f;
            Transf = new Transformation();
        }
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
        /// <summary>
        /// Shoot a ray through the camera's screen
        /// </summary>
        /// <param name="u">x-axis</param>
        /// <param name="v">y-axis</param>
        /// <returns></returns>
        /// The coordinates (u, v) specify the point on the screen where the ray crosses it.
        /// The following diagram represents the coordinates (0,0); (1,0); (0,1); (1,1):
        ///   (0, 1)                          (1, 1)
        ///      +------------------------------+
        ///      |                              |
        ///      |                              |
        ///      |                              |
        ///      +------------------------------+
        ///   (0, 0)                          (1, 0)
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
    ///     <description> random number generator</description>
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
        public ImageTracer(HdrImage image, ICamera camera, PCG pcg, int samplePerSide = 1)
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
        public ImageTracer(HdrImage image, ICamera camera, int samplePerSide = 1)
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