namespace PGENLib
{
    

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
        public Ray(Point origin, Vec dir, float tmin)
        {
            this.Origin = origin;
            this.Dir = dir;
            this.Tmin = tmin;
            this.Tmax = float.PositiveInfinity;
            this.Depth = 0;
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
            var newRay = new Ray(tr*this.Origin, tr*this.Dir)
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

        public OrthogonalCamera(float aspectRatio)
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
    
}