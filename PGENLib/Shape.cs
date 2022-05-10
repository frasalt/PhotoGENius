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

        public Sphere()
        {
            _transf = new Transformation();
        }
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
        /// Computes the normal in the intersection point of a ray and the surface of a unit sphere.
        /// The normal has always the opposite direction with respect to a given Ray. 
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
        /// Convert the 3D intersection point into a 2D point, of coordinates (u,v). 
        /// </summary>
        public Vec2d SpherePointToUv(Point point)
        {
            float u = 0.0f;
            // <<<<<< ho aggiunto questo controllo per evitare divisioni per zero (martin)
            if (point.x != 0)
            {
                double arg = point.y / (double)point.x;
                u = (float)Math.Atan(arg) / (2.0f * (float)Math.PI);
            }
            else if (point.x == 0)
            {
                if (point.y > 0) u = 1.0f;
                else if (point.y < 0) u = -1.0f;
            }
            // <<<<<<<<<<
            float v = (float)Math.Acos(point.z) / (float)Math.PI;
            
            if (u >= 0.0)
            {
                Vec2d tot = new Vec2d(u,v);
                return tot;
            }
            else
            {
                u += 1.0f;
                Vec2d tot = new Vec2d(u, v);
                return tot;
            }
        }
        
 
    }

    public class XyPlane : Shape
    {
        private Transformation _transf;
        
        public XyPlane(Transformation transf)
        {
            _transf = transf;
        }
        
        /// <summary>
        /// Compute the intersection between a ray and the shape.
        /// Returns a HitRecord object, or Null if the ray doesn't hit the plane.
        /// </summary>
        public override HitRecord? RayIntersection(Ray ray)
        {
            Ray invRay = ray.Transform(this._transf.Inverse());
            var originVec = invRay.Origin.PointToVec();

            if (Math.Abs(invRay.Dir.z) < 1E-5)
            {
                return null;
            }
            var t = - originVec.z/invRay.Dir.z;
            if (t <= invRay.Tmin || t >= invRay.Tmax)
            {
                return null;
            }
            var hitPoint = invRay.At(t);
            var planeNormal = new Normal(0.0f, 0.0f, 1.0f);
            if (invRay.Dir.z > 0)
            {
                planeNormal = planeNormal * -1.0f;
            } 
            
            var surfacePoint = new Vec2d(hitPoint.x - (float)Math.Floor(hitPoint.x),  
                hitPoint.y - (float)Math.Floor(hitPoint.y));
            var hit = new HitRecord(_transf*hitPoint, _transf*planeNormal,
                surfacePoint, t, ray);
            return hit;
        }
        
    }

}