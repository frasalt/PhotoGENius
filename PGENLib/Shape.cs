using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml.Schema;

namespace PGENLib
{

    /// <summary>
    /// Interface for a generic 3D shape: the method RayIntersection is implemented in different concrete shapes.
    /// </summary>
    public abstract class Shape
    {
        private Transformation _transf;
        public virtual HitRecord? RayIntersection(Ray ray)
        {
            throw new NotImplementedException();
        }
    }
    
    
    /// <summary>
    /// A 3D unit sphere centered on the axes origin.
    /// </summary>
    public class Sphere : Shape
    {
        private Transformation _transf;
        
        public Sphere(Transformation transf)
        {
            _transf = transf;
        }
        
        /// <summary>
        /// Compute the intersection between a ray and the shape.
        /// Returns a HitRecord, or Null if the ray doesn't hit the sphere.
        /// </summary>
        public override HitRecord? RayIntersection(Ray ray)
        {
            Ray invRay = ray.Transform(this._transf.Inverse());
            var originVec = invRay.Origin.PointToVec();
            var a = Vec.SquaredNorm(invRay.Dir);
            var b = 2.0f * Vec.DotProd(originVec, invRay.Dir);
            var c = Vec.SquaredNorm(originVec) - 1;
            var delta = b * b - 4.0f * a * c;
            
            if (delta <= 0)
            {
                return null;
            }
            float sqrtDelta = (float) Math.Sqrt(delta);
            float tmin = (-b - sqrtDelta) / (2.0f * a);
            float tmax = (-b + sqrtDelta) / (2.0f * a);
            float tFirstHit;
            if (tmin > invRay.Tmin & tmin < invRay.Tmax)
            {
                tFirstHit = tmin;
            }
            else if (tmax > invRay.Tmin & tmax < invRay.Tmax)
            {
                tFirstHit = tmax;
            }
            else
            {
                return null;
            }

            var hitPoint = invRay.At(tFirstHit);
            var hit = new HitRecord(_transf*hitPoint, _transf*SphereNormal(hitPoint, ray.Dir),
                SpherePointToUv(hitPoint), tFirstHit, ray);
            return hit;
        }
        /// <summary>
        /// Normal in intersection point. 
        /// </summary>
        public Normal SphereNormal(Point point,  Vec dir)
        { 
            Normal result = new Normal(point.x, point.y, point.z);
            Normal n;
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