using System.Numerics;
using System.Runtime.CompilerServices;

namespace PGENLib
{
    //==================================================================================================================
    //Vec
    //==================================================================================================================
    /// <summary>
    /// A 3D vector.
    /// This class has three floating-point fields: `x`, `y`, and `z`.
    /// </summary>
    
    public struct Vec
    {
        public float x;
        public float y;
        public float z;

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Vec()
        {
            x = 0;
            y = 0;
            z = 0;
        }
        
        /// <summary>
        /// Constructor with float parameters.
        /// </summary>
        public Vec(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        //METODI========================================================================================================
        /// <summary>
        /// Returns x, y, z as a string.
        /// </summary>
        public override string ToString()
        {
            return $"Vec(x={x}, y={y}, z={z})";
        }
        
        /// <summary>
        /// Check weather two vectors Vec are equals. 
        /// </summary>
        public static bool are_close(Vec p, Vec q)
        {
            var epsilon = 1E-5;
            if (Math.Abs(p.x - q.x) < epsilon & 
                Math.Abs(p.y - q.y) < epsilon & 
                Math.Abs(p.z - q.z) < epsilon)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        /// <summary>
        ///  Returns the sum of two Vec.  
        /// </summary>
        public static Vec operator +(Vec v, Vec w)
        {
            var t = new Vec
            {
                x = v.x + w.x,
                y = v.y + w.y,
                z = v.z + w.z
            };
            return t;
        }

        /// <summary>
        ///  Returns the difference between two Vec. 
        /// </summary>
        public static Vec operator -(Vec v, Vec w)
        {
            var t = new Vec
            {
                x = v.x - w.x,
                y = v.y - w.y,
                z = v.z - w.z
            };
            return t;
        }

        /// <summary>
        ///  Returns the product between a vector and a scalar.
        /// </summary>
        public static Vec operator *(Vec v, float s)
        {
            var t = new Vec
            {
                x = v.x * s,
                y = v.y * s,
                z = v.z * s
            };
            return t;
        }
        
        /// <summary>
        ///  Returns the division between a Vec and a float, which is a Vec.
        /// </summary>
        public static Vec operator /(Vec v, float s)
        {
            var t = new Vec
            {
                x = v.x / s,
                y = v.y / s,
                z = v.z / s
            };
            return t;
        }

    
        /// <summary>
        ///  Return the reversed vector.
        /// </summary>
        public Vec Neg()
        {
            float s = -1;
            return this*s;
        }
        
        /// <summary>
        ///  Return the reversed vector.
        /// </summary>
        public static Vec operator-(Vec a)
        {
            float s = -1;
            return a*s;
        }
        
        /// <summary>
        ///  Returns the dot product between two vectors.
        /// </summary>
        public static float DotProd(Vec v, Vec w)
        {
            return v.x*w.x+v.y*w.y+v.z*w.z;
        }
        
        /// <summary>
        ///  Returns the cross product between two vectors.
        /// </summary>
        public static Vec CrossProduct(Vec v, Vec w)
        {
            var t = new Vec
            {
                x = v.y*w.z - v.z*w.y,
                y = v.z*w.x - v.x*w.z,
                z = v.x*w.y - v.y*w.x
            };
            return t;
        }
        
        /// <summary>
        ///  Return the squared norm (Euclidean length) of a vector. 
        /// </summary>
        public static float SquaredNorm(Vec v)
        {
            return DotProd(v,v);
        }
        
        /// <summary>
        ///  Return the norm (Euclidean length) of a vector. 
        /// </summary>
        public static float Norm(Vec v)
        {
            return (float) Math.Sqrt(DotProd(v,v));
        }
        
        /// <summary>
        ///  Normalize the vector, so that it's norm is equal to 1.
        /// </summary>
        public Vec NormalizeVec()
        {
            var t = new Vec
            {
                x = this.x/Norm(this),
                y = this.y/Norm(this),
                z = this.z/Norm(this)
            };
            return t;
            
        }
        
        /// <summary>
        ///  Turns a vector in a normal, and returns the normal.
        /// </summary>
        public Normal VecToNorm()
        {
            var t = new Normal
            {
                x = this.x,
                y = this.y,
                z = this.z
            };
            return t;
        }
        
           
        /// <summary>
        /// Create a orthonormal basis (Onb) from a vector representing the z axis (normalized)
        /// Return a tuple containing the three vectors (e1, e2, e3) of the basis. The result is such
        /// that e3 = normal.
        /// The `normal` vector must be normalized.
        /// </summary>
        public static Tuple CreateOnbFromZ(Normal e3)
        {
            float sign;
            if (e3.z > 0.0)
            {
                sign = 1.0f;
            }
            else sign = -1.0f;

            float a = (float) -1.0f / (sign + e3.z);
            float b = e3.x * e3.y * a;
            Vec e1 = new Vec( 1.0f + sign * e3.x * e3.x * a,sign * b,-sign * e3.x);
            Vec e2 = new Vec(b, sign + e3.y * e3.y * a, -e3.y);
            Vec vecN = new Vec(e3.x, e3.y, e3.z);
            Tuple T = new Tuple(e1, e2, vecN);
            
            return T;
        }
        
        /// <summary>
        /// Normalize two vectors or normals and apply the dot product.
        /// The result is the cosine of the angle between the two vectors/normals.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        
        //DEVO IMPLEMENTEARE UNION PER FORZA PERCHè COSì VALE SIA PER I VETTORI CHE PER LE NORMALI
        public float NormalizeDot(Vec v1, Vec v2)
        {
            Vec v1Vec = new Vec(v1.x, v1.y, v1.z).NormalizeVec();
            Vec v2Vec = new Vec(v2.x, v2.y, v2.z).NormalizeVec();
            float r = Vec.DotProd(v1Vec, v2Vec);
            return r;
        }
        
    }
    //==========================================================================================================
    //Tuple 
    //==========================================================================================================
    public struct Tuple
    {
        public Vec e1;
        public Vec e2;
        public Vec e3;

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Tuple()
        {
            e1.x = 0;
            e2.x = 0;
            e3.x = 0;
            e1.y = 0;
            e2.y = 0;
            e3.y = 0;
            e1.z = 0;
            e2.z = 0;
            e3.z = 0;
        }

        /// <summary>
        /// Constructor with float parameters.
        /// </summary>
        public Tuple(Vec e1, Vec e2, Vec e3)
        {
            this.e1 = e1;
            this.e2 = e2;
            this.e3 = e3;
        }

    }

    //==================================================================================================================
    //Point
    //==================================================================================================================

    public struct Point
    {
        public float x;
        public float y;
        public float z;

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Point()
        {
            x = 0;
            y = 0;
            z = 0;
        }
        
        /// <summary>
        /// Constructor with parameter. 
        /// </summary>
        public Point(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        //METODI========================================================================================================
        /// <summary>
        /// Returns x, y, z as a string.
        /// </summary>
        public override string ToString()
        {
            return $"Point(x={x}, y={y}, z={z})";
        }
        
        /// <summary>
        /// Check weather two vectors are equals.  
        /// </summary>
        public static bool are_close(Point p, Point q)
        {
            var epsilon = 1E-5;
            if (Math.Abs(p.x - q.x) < epsilon & 
                Math.Abs(p.y - q.y) < epsilon & 
                Math.Abs(p.z - q.z) < epsilon)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        /// <summary>
        ///  Return the sum between a point and a vector, which is a point.
        /// </summary>
        public static Point operator +(Point p, Vec v)
        {
            var q = new Point
            {
                x = v.x + p.x,
                y = v.y + p.y,
                z = v.z + p.z
            };
            return q;
        }

        /// <summary>
        ///  Returns the product between a point and a scalar.
        /// </summary>
        public static Point operator *(Point p, float a)
        {
            var q = new Point
            {
                x = p.x * a,
                y = p.y * a,
                z = p.z * a
            };
            return q;
        }
        /// <summary>
        ///  Returns the difference between two points, as a vector.
        /// </summary>
        public static Vec operator -(Point p, Point q)
        {
            var v = new Vec
            {
                x = p.x - q.x,
                y = p.y - q.y,
                z = p.z - q.z
            };
            return v;
        }
        
        /// <summary>
        ///  Return the difference between a point and a vector, as point.
        /// </summary>
        public static Point operator -(Point p, Vec v)
        {
            var q = new Point
            {
                x = p.x - v.x,
                y = p.y - v.y,
                z = p.z - v.z
            };
            return q;
        }

        /// <summary>
        ///  Turns a point into a vector and returns it.
        /// </summary>
        public Vec PointToVec()
        {
            var q = new Vec
            {
                x = this.x,
                y = this.y,
                z = this.z
            };
            return q;
        }
        /// <summary>
        ///  Returns the division between a point and a scalar. 
        /// </summary>
        public static Point operator /(Point v, float s)
        {
            var t = new Point
            {
                x = v.x / s,
                y = v.y / s,
                z = v.z / s
            };
            return t;
        }
    }
    //==================================================================================================================
    //Normal
    //==================================================================================================================
    /// <summary>
    /// A normal vector in 3D space.
    /// This struct has three floating-point fields: `x`, `y`, and `z`.
    /// </summary>
    public struct Normal
    {
        public float x;
        public float y;
        public float z;

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Normal()
        {
            x = 0;
            y = 0;
            z = 0;
        }
        
        /// <summary>
        /// Constructor with float parameters. 
        /// </summary>
        public Normal(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        //METODI========================================================================================================
        /// <summary>
        /// Returns x, y, z as astring.
        /// </summary>
        public override string ToString()
        {
            return $"Normal(x={x}, y={y}, z={z})";
        }
        
        /// <summary>
        /// Returns the Normal as a Vec.
        /// </summary>
        public Vec ToVec()
        {
            return new Vec(x, y, z);
        }
        
        
        
        /// <summary>
        /// Check wheater two normal are equals. 
        /// </summary>
        public static bool are_close(Normal p, Normal q)
        {
            var epsilon = 1E-5;
            if (Math.Abs(p.x - q.x) < epsilon & 
                Math.Abs(p.y - q.y) < epsilon & 
                Math.Abs(p.z - q.z) < epsilon)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        ///  Returns the product between a vector and a scalar. 
        /// </summary>
        public static Normal operator *(Normal n, float s)
        {
            var t = new Normal
            {
                x = n.x * s,
                y = n.y * s,
                z = n.z * s
            };
            return t;
        }
        /// <summary>
        ///  Invert the direction of a normal. 
        /// </summary>
        public Normal Neg()
        {
            const float s = -1;
            return this*s;
        }
        
        /// <summary>
        ///  Returns the dot product between a vector and a normal.
        /// </summary>
        public static float VecDotNormal(Vec v, Normal w)
        {
            return v.x*w.x+v.y*w.y+v.z*w.z;
        }
        
        /// <summary>
        ///  Returns the cross product between a vector and a normal.
        /// </summary>
        public static Vec VecCrossNormal(Vec v, Normal w)
        {
            var t = new Vec     
            {
                x = v.y*w.z - v.z*w.y,
                y = v.z*w.x - v.x*w.z,
                z = v.x*w.y - v.y*w.x
            };
            return t;
        }
        /// <summary>
        ///  Returns the cross product between two normals.
        /// </summary>
        public static Normal NormalCrossNormal(Normal v, Normal w)
        {
            var t = new Normal 
            {
                x = v.y*w.z - v.z*w.y,
                y = v.z*w.x - v.x*w.z,
                z = v.x*w.y - v.y*w.x
            };
            return t;
        }

        /// <summary>
        ///  Returns the squared norm of a normal.
        /// </summary>
        public static float SquaredNorm(Normal n)
        {
            return n.x * n.x + n.y * n.y + n.z * n.z;
        }
        /// <summary>
        ///  Returns the norm of a normal.
        /// </summary>
        
        public static float Norm(Normal n)
        {
            return (float) Math.Sqrt(SquaredNorm(n)) ;
        }
        
        /// <summary>
        ///  Normalize the vector, so that it's norm is equal to 1.
        /// </summary>
        public Normal NormalizeNormal()
        {
            var t = new Normal
            {
                x = this.x/Norm(this),
                y = this.y/Norm(this),
                z = this.z/Norm(this)
            };
            return t;
        }
    }
    
    //==================================================================================================================
    //Transformation
    //==================================================================================================================
    /// <summary>
    /// An affine transformation.
    ///This class encodes an affine transformation. It has been designed with the aim of making the calculation
    ///of the inverse transformation particularly efficient.
    /// </summary>
    public struct Transformation
    {
        public Matrix4x4 m;
        public Matrix4x4 invm;
        
        /// <summary>
        /// Constructor, without any parameter
        /// </summary>
        public Transformation()
        {
            m = Matrix4x4.Identity;
            bool consistency = Matrix4x4.Invert(m, out invm);
            
        }
        
        public Transformation(float a, float b, float c, float d,
                              float e, float f, float g, float h, 
                              float i, float l, float s, float n,
                              float o, float p, float q, float r)
        {
            m = new Matrix4x4(a,b,c,d, 
                              e,f,g,h,
                              i,l,s,n,
                              o,p,q,r);
            bool consistency = Matrix4x4.Invert(m, out invm);
        }
        
        public Transformation(Matrix4x4 a, Matrix4x4 inva)
        {
            m = a;
            invm = inva;
        }
        
        public Transformation(Matrix4x4 a)
        {
            m = a;
            bool consistency = Matrix4x4.Invert(a, out invm);
        }
        
        //METODI========================================================================================================
        /// <summary>
        /// Check wheather two matrices are equals.
        /// </summary>
        public static bool are_close(Matrix4x4 a, Matrix4x4 b)
        {
            var epsilon = 1E-5;
            if (Math.Abs(a.M11 - b.M11) < epsilon & Math.Abs(a.M12 - b.M12) < epsilon &
                Math.Abs(a.M13 - b.M13) < epsilon &
                Math.Abs(a.M21 - b.M21) < epsilon & Math.Abs(a.M22 - b.M22) < epsilon &
                Math.Abs(a.M23 - b.M23) < epsilon &
                Math.Abs(a.M31 - b.M31) < epsilon & Math.Abs(a.M32 - b.M32) < epsilon &
                Math.Abs(a.M33 - b.M33) < epsilon)
            {
                return true; 
            }
            else
            {
                return false;
            }
                                                          
        }
        
        /// <summary>
        /// Check whether two Transformations are equals. 
        /// </summary>
        public static bool are_close(Transformation a, Transformation b)
        {
            if (are_close(a.m, b.m) & are_close(a.invm, b.invm))
            {
                return true; 
            }
            else
            {
                return false;
            }
                                                          
        }
        
        /// <summary>
        /// Check the internal consistency of the transformation. 
        /// </summary>
        public bool IsConsistent()
        {
            return are_close(m*invm, Matrix4x4.Identity);
        }
        
        /// <summary>
        /// Returns a struct Transformation object representing the inverse affine transformation.
        /// </summary>
        public Transformation Inverse()
        {
            Transformation tr = new Transformation(this.invm, this.m);
            return tr;
        }

        /// <summary>
        /// Product Transformation-Transformation, returns a Transformation.
        /// </summary>
        public static Transformation operator *(Transformation a, Transformation b)
        {
            Transformation tr = new Transformation(a.m*b.m, b.invm*a.invm);
            return tr;
        }
        
        /// <summary>
        /// Product Transformation-Point, returns a Point. 
        /// </summary>
        public static Point operator *(Transformation a, Point p)
        {
            Point q = new Point(p.x*a.m.M11 + p.y*a.m.M12 + p.z*a.m.M13 + a.m.M14, 
                                p.x*a.m.M21 + p.y*a.m.M22 + p.z*a.m.M23 + a.m.M24, 
                                p.x*a.m.M31 + p.y*a.m.M32 + p.z*a.m.M33 + a.m.M34);
            var w = p.x * a.m.M41 + p.y * a.m.M42 + p.z * a.m.M43 + a.m.M44;
            if (Math.Abs(w - 1.0) < 1E-5)
            {
                return q;
            }
            else
            {
                return q/w;
            }
        }
        
        /// <summary>
        /// Product Transformation-Vec, returns a Vec. 
        /// </summary>
        public static Vec operator *(Transformation a, Vec v)
        {
            Vec q = new Vec(v.x*a.m.M11 + v.y*a.m.M12 + v.z*a.m.M13, 
                            v.x*a.m.M21 + v.y*a.m.M22 + v.z*a.m.M23, 
                            v.x*a.m.M31 + v.y*a.m.M32 + v.z*a.m.M33);
            return q;
        }
        
        /// <summary>
        /// Product Transformation-Normal, returns a Normal. 
        /// </summary>
        public static Normal operator *(Transformation a, Normal n)
        {
            Normal q = new Normal(n.x*a.invm.M11 + n.y*a.invm.M21 + n.z*a.invm.M31, 
                                  n.x*a.invm.M12 + n.y*a.invm.M22 + n.z*a.invm.M32, 
                                  n.x*a.invm.M13 + n.y*a.invm.M23 + n.z*a.invm.M33);
            return q;
        }
        /// <summary>
        /// Returns a Transformation object, encoding a rigid translation.
        /// </summary>
        public static Transformation Traslation(Vec v)
        {
            Transformation trasl = new Transformation();
            trasl.m.M14 = v.x;
            trasl.m.M24 = v.y;
            trasl.m.M34 = v.z;
            trasl.invm.M14 = -v.x;
            trasl.invm.M24 = -v.y;
            trasl.invm.M34 = -v.z;
            return trasl;
        }
        
        /// <summary>
        /// Return a Transformation object, encoding a rotation around the X axis.
        /// The parameter `ang` specifies the rotation angle (in degrees). The positive sign is
        /// given by the right-hand rule.
        /// </summary>
        public static Transformation RotationX(float ang)
        {
            Transformation xRot = new Transformation();
            float cos = (float) Math.Cos(Math.PI * ang / 180.0); 
            float sin = (float) Math.Sin(Math.PI * ang / 180.0);
            xRot.m.M22 = cos;
            xRot.m.M23 = -sin;
            xRot.m.M32 = sin;
            xRot.m.M33 = cos;
            xRot.invm.M22 = cos;
            xRot.invm.M23 = sin;
            xRot.invm.M32 = -sin;
            xRot.invm.M33 = cos;
            return xRot;
        }
        
        /// <summary>
        /// Return a Transformation object, encoding a rotation around the Y axis.
        /// The parameter `ang` specifies the rotation angle (in degrees). The positive sign is
        /// given by the right-hand rule.
        /// </summary>
        public static Transformation RotationY(float ang)
        {
            Transformation yRot = new Transformation();
            float cos = (float) Math.Cos(Math.PI * ang / 180.0);
            float sin = (float) Math.Sin(Math.PI * ang / 180.0);
            yRot.m.M11 = cos;
            yRot.m.M13 = sin;
            yRot.m.M31 = -sin;
            yRot.m.M33 = cos;
            yRot.invm.M11 = cos;
            yRot.invm.M13 = -sin;
            yRot.invm.M31 = sin;
            yRot.invm.M33 = cos;
            return yRot;
        }
        
        /// <summary>
        /// Return a Transformation object, encoding a rotation around the Z axis.
        /// The parameter `ang` specifies the rotation angle (in degrees). The positive sign is
        /// given by the right-hand rule.
        /// </summary>
        public static Transformation RotationZ(float ang)
        {
            Transformation zRot = new Transformation();
            float cos = (float) Math.Cos(Math.PI * ang / 180.0);
            float sin = (float) Math.Sin(Math.PI * ang / 180.0);
            zRot.m.M11 = cos;
            zRot.m.M12 = -sin;
            zRot.m.M21 = sin;
            zRot.m.M22 = cos;
            zRot.invm.M11 = cos;
            zRot.invm.M12 = sin;
            zRot.invm.M21 = -sin;
            zRot.invm.M22 = cos;
            return zRot;
        }

        /// <summary>
        /// Returns a Transformation object, encoding a scaling.
        /// </summary>
        public static Transformation Scaling(Vec v)
        {
            Transformation scl = new Transformation();
            scl.m.M11 = v.x;
            scl.m.M22 = v.y;
            scl.m.M33 = v.z;
            scl.invm.M11 = 1 / v.x;
            scl.invm.M22 = 1 / v.y;
            scl.invm.M33 = 1 / v.z;
            return scl;
        }
        
    }
    

        
        
    
    
    

    

}