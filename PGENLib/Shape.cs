using System.Numerics;
using System.Runtime.CompilerServices;

namespace PGENLib
{
    
    /// <summary>
    /// Interface for a generic 3D shape: the method RayIntersection is implemented in different concrete shapes.
    /// </summary>
    public interface IShapes
    {
        HitRecord? RayIntersection(Ray ray);

    }

    /// <summary>
    /// A 3D unit sphere centered on the axes origin.
    /// </summary>
    public struct Sphere : IShapes
    {
        private Transformation Transf;

        public Sphere(Transformation transf)
        {
            Transf = transf;
        }
        
        
        public HitRecord? RayIntersection(Ray ray)
        {
            Ray InvRay = ray.Transform(this.Transf.Inverse());
            var OriginVec = InvRay.Origin.PointToVec();
            var a = Vec.SquaredNorm(InvRay.Dir);
            var b = 2.0f * Vec.DotProd(OriginVec, InvRay.Dir);
            var c = Vec.SquaredNorm(OriginVec) - 1;
            var delta = b * b - 4.0f * a * c;
            
            if (delta <= 0)
            {
                return null;
            }
            float sqrtDelta = (float) Math.Sqrt(delta);
            float tmin = (float) ((-b - sqrtDelta) / (2.0f * a));
            float tmax = (float) ((-b + sqrtDelta) / (2.0f * a));
            float tFirstHit = 0;
            if (tmin > InvRay.Tmin & tmin < InvRay.Tmax)
            {
                tFirstHit = tmin;
            }
            else if (tmax > InvRay.Tmin & tmax < InvRay.Tmax)
            {
                tFirstHit = tmax;
            }
            else
            {
                return null;
            }

            var hitPoint = InvRay.At(tFirstHit);
            HitRecord hit = new HitRecord(this.Transf*hitPoint, this.Transf*sphere_normal(hit_point, ray.dir),
                sphere_point_to_uv(hit_point), tFirstHit, ray);
            return hit;
        }
        
    }
}