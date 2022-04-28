using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml.Schema;

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
            HitRecord hit = new HitRecord(this.Transf*hitPoint, this.Transf*SphereNormal(hitPoint, ray.Dir),
                SpherePointToUv(hitPoint), tFirstHit, ray);
            return hit;
        }
        /// <summary>
        /// Normal in intersection point. 
        /// </summary>
        public Normal SphereNormal(Point point,  Vec dir)
        { 
            Normal result = new Normal(point.x, point.y, point.z);
            Normal n = new Normal();
            if (Vec.DotProd(point.PointToVec(),dir) < 0.0)
            {
                n = result;
            }
            else
            {
                n = result*(-1);
            }
            return n;
        }

        /// <summary>
        /// Convert intersection point in u,v coordinates. 
        /// </summary>
        public Vec2d SpherePointToUv(Point point)
        {
            var arg = point.y / point.x;
            var u = Math.Atan(arg) / (2.0 * Math.PI);
            var v = Math.Acos(point.z) / Math.PI;
            
            if (u >= 0.0)
            {
                Vec2d tot = new Vec2d((float)u,(float)v);
                return tot;
            }
            else
            {
                u = u + 1.0f;
                Vec2d tot = new Vec2d((float) u, (float) v);
                return tot;
            }
        }
        
 
    }
}