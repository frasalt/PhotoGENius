using System.Numerics;
using System.Runtime.CompilerServices;

namespace PGENLib
{
    //==================================================================================================================
    //Vec
    //==================================================================================================================
    /// <summary>
    /// A 2D vector, with two floating point fields u and v.
    /// </summary>

    public struct Vec2d
    {
        internal float u;
        internal float v;

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Vec2d()
        {
            u = 0;
            v = 0;

        }

        /// <summary>
        /// Constructor with float parameters.
        /// </summary>
        public Vec2d(float u, float v)
        {
            this.u = u;
            this.v = v;

        }

        //METODI========================================================================================================
        /// <summary>
        /// Check weather two Vec2d objects are equals. 
        /// </summary>
        public static bool are_close(Vec2d p, Vec2d q)
        {
            var epsilon = 1E-5;
            if (Math.Abs(p.u - q.u) < epsilon && Math.Abs(p.v - q.v) < epsilon)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
    
    //==================================================================================================================
    //HitRecords
    //==================================================================================================================
    /// <summary>
    /// A class holding information about a ray-shape intersection.
    /// The parameters defined in this dataclass are the following:
    /// <list type="table">
    /// <item>
    ///     <term>WorldPoint</term>
    ///     <description> a `Point` object holding the world coordinates of the hit point</description>
    /// </item>
    /// <item>
    ///     <term>Normal</term>
    ///     <description> a `Normal` object holding the orientation of the normal to the surface where the hit happened</description>
    /// </item>
    /// <item>
    ///     <term>SurfacePoint</term>
    ///     <description> a `Vec2d` object holding the position of the hit point on the surface of the object</description>
    /// </item>
    /// <item>
    ///     <term>t</term>
    ///     <description> a floating-point value specifying the distance from the origin of the ray where the hit happened</description>
    /// </item>
    /// <item>
    ///     <term>Ray</term>
    ///     <description> a `Ray` object that hit the surface</description>
    /// </item>
    /// <item>
    ///     <term>Material</term>
    ///     <description> the parameter `Material` of the `Shape` object hit by the ray </description>
    /// </item>
    /// </list>
    /// </summary>
    
    public struct HitRecord
    {
        public Point WorldPoint;
        public Normal Normal;
        public Vec2d SurfacePoint;
        public float t;
        public Ray Ray;
        public Material Material;
        
        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        public HitRecord(Point worldPoint, Normal normal, Vec2d surfacePoint, float t, Ray ray, Material material)
        {
            WorldPoint = worldPoint;
            Normal = normal;
            SurfacePoint = surfacePoint;
            this.t = t;
            Ray = ray;
            Material = material;
        }
        
        //METODI========================================================================================================
        /// <summary>
        /// Check weather two HitRecord objects are equals. 
        /// </summary>
        public static bool are_close(HitRecord a, HitRecord b)
        {
            var epsilon = 1E-5;
            if (Point.are_close(a.WorldPoint, b.WorldPoint) &&
                Normal.are_close(a.Normal, b.Normal) &&
                Vec2d.are_close(a.SurfacePoint, b.SurfacePoint) &&
                Math.Abs(a.t-b.t)<epsilon &&
                Ray.are_close(a.Ray, b.Ray))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        
    }
}
