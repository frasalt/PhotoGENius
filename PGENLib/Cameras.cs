namespace PGENLib
{
/*
    public interface ICamera
    {
        void FireRay(float u, float v);

    }

    public struct OrthogonalCamera : ICamera
    {
        private float AspectRatio;
        private Transformation tr;

        public void FireRay(float u, float v)
        {
            Point origin = new Point(-1.0, (1.0 - 2 * u) * this.AspectRatio, 2 * v - 1);
            direction = VEC_X;
            return Ray(origin = origin, dir = direction, tmin = 1.0).transform(self.transformation);
        }


    }
*/
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
            Tmin = 0;
            Tmax = 0;
            Depth = 0;
        }

        /// <summary>
        /// Constructor to create a Ray.
        /// </summary>
        public Ray(Point Origin, Vec Dir)
        {
            this.Origin = Origin;
            this.Dir = Dir;
            this.Tmin = 1e-5f;
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

        public static bool is_close(Ray a, Ray b)
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

        public static Point at(Ray a, float t)
        {
            return a.Origin + (a.Dir * t);

        }
    }
}