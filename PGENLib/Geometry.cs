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

using System.Numerics;

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
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vec(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        //METODI========================================================================================================
        
        /// <summary>
        /// Returns vector's components x, y, z as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Vec(x={x}, y={y}, z={z})";
        }
        
        /// <summary>
        /// Check if two vectors are similar enough to be considered equal. 
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
        ///  Returns the sum of two vectors.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static Vec operator +(Vec v, Vec w)
        {
            v.x += w.x;
            v.y += w.y;
            v.z += w.z;
            return v;
        }

        /// <summary>
        /// Returns the difference between two vectors.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static Vec operator -(Vec v, Vec w)
        {
            v.x -= w.x;
            v.y -= w.y;
            v.z -= w.z;
            return v;
        }

        /// <summary>
        /// Returns the product between a vector and a scalar.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Vec operator *(Vec v, float s)
        {
            return new Vec(v.x * s, v.y * s, v.z * s);
        }
        
        /// <summary>
        /// Returns the division between a vector and a float, which is a vector.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="s"></param>
        /// <returns>a vector</returns>
        public static Vec operator /(Vec v, float s)
        {
            return new Vec(v.x / s, v.y / s, v.z / s);
        }

        /// <summary>
        /// Return the reversed vector.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Vec operator-(Vec a)
        {
            float s = -1;
            return a*s;
        }
        
        /// <summary>
        /// Returns the dot product between two vectors.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static float DotProd(Vec v, Vec w)
        {
            return v.x*w.x+v.y*w.y+v.z*w.z;
        }
        
        /// <summary>
        /// Returns the cross product between two vectors.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static Vec CrossProduct(Vec v, Vec w)
        {
            return new Vec(v.y * w.z - v.z * w.y, v.z * w.x - v.x * w.z, v.x * w.y - v.y * w.x);
        }
        
        /// <summary>
        /// Return the squared norm (Euclidean length) of a vector.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float SquaredNorm(Vec v)
        {
            return DotProd(v,v);
        }
        
        /// <summary>
        /// Return the norm (Euclidean length) of a vector.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float Norm(Vec v)
        {
            return (float) Math.Sqrt(DotProd(v,v));
        }
        
        /// <summary>
        /// Normalize the vector, so that it's norm is equal to 1.
        /// </summary>
        /// <returns></returns>
        public Vec NormalizeVec()
        {
            return new Vec(x/Norm(this),y/Norm(this),z/Norm(this));
        }
        
        /// <summary>
        /// Turns a vector in a normal, and returns the normal.
        /// </summary>
        /// <returns></returns>
        public Normal VecToNorm()
        {
            return new Normal(x, y, z);
        }
        
        /// <summary>
        /// Create a orthonormal basis (Onb) from a vector representing the z axis (which must be normalized).
        /// </summary>
        /// <param name="e3"> normalized Normal vector, on the z axis</param>
        /// <returns> Tuple containing the three vectors of the basis, such that e3 = normal</returns>
        public static MyTuple CreateOnbFromZ(Normal e3)
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
            MyTuple T = new MyTuple(e1, e2, vecN);
            
            return T;
        }
        
        /// <summary>
        /// Normalize two vectors or normals and evaluate the dot product.
        /// The result is the cosine of the angle between the two vectors/normals.
        /// </summary>
        /// <param name="v1">Vector</param>
        /// <param name="v2">Vector</param>
        /// <returns> a float representing the cosine of the angle between the two vecors/normals.</returns>
        public static float NormalizeDot(Vec v1, Vec v2)
        {
            var v1Vec = new Vec(v1.x, v1.y, v1.z).NormalizeVec();
            var v2Vec = new Vec(v2.x, v2.y, v2.z).NormalizeVec();
            float r = DotProd(v1Vec, v2Vec);
            return r;
        }

        /// <summary>
        /// Normalize two vectors or normals and evaluate the dot product.
        /// The result is the cosine of the angle between the two vectors/normals.
        /// </summary>
        /// <param name="v1">Vec</param>
        /// <param name="v2">Normal</param>
        /// <returns> a float representing the cosine of the angle between the two vecors/normals.</returns>
        public static float NormalizeDot(Vec v1, Normal v2)
        {
            var v1Vec = new Vec(v1.x, v1.y, v1.z).NormalizeVec();
            var v2Vec = new Vec(v2.x, v2.y, v2.z).NormalizeVec();
            float r = DotProd(v1Vec, v2Vec);
            return r;
        }
    }
    
    //==========================================================================================================
    //Tuple 
    //==========================================================================================================
    
    
    public struct MyTuple
    {
        public Vec e1;
        public Vec e2;
        public Vec e3;

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public MyTuple()
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
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <param name="e3"></param>
        public MyTuple(Vec e1, Vec e2, Vec e3)
        {
            this.e1 = e1;
            this.e2 = e2;
            this.e3 = e3;
        }

    }

    //==================================================================================================================
    //Point
    //==================================================================================================================
    /// <summary>
    /// A point in 3D space.
    /// This struct has three floating-point fields: `x`, `y`, and `z`.
    /// </summary>
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
        /// Constructor with float parameters.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Point(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        //METODI========================================================================================================
        
        /// <summary>
        /// Returns point's components x, y, z as a string.
        /// </summary>
        public override string ToString()
        {
            return $"Point(x={x}, y={y}, z={z})";
        }
        
        /// <summary>
        /// Check if two points are similar enough to be considered equal.  
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
            return false;
        }
        
        /// <summary>
        /// Return the sum between a point and a vector, which is a point.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="v"></param>
        /// <returns>point</returns>
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
        /// Returns the product between a point and a scalar.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="a"></param>
        /// <returns></returns>
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
        /// Returns the difference between two points, as a vector.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns>vector</returns>
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
        /// Return the difference between a point and a vector, as point.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="v"></param>
        /// <returns></returns>
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
        /// Turns a point into a vector and returns it.
        /// </summary>
        /// <returns></returns>
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
        /// <param name="v"></param>
        /// <param name="s"></param>
        /// <returns></returns>
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
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Normal(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        //METODI========================================================================================================
        
        /// <summary>
        /// Returns normal's components x, y, z as a string.
        /// </summary>
        /// <returns></returns>
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
        /// Check if two normals are similar enough to be considered equal. 
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
            return false;
            
        }
        
        /// <summary>
        /// Returns the product between a vector and a scalar.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="s"></param>
        /// <returns></returns>
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
        /// Invert the direction of a normal.
        /// </summary>
        /// <returns></returns>
        public Normal Neg()
        {
            const float s = -1;
            return this*s;
        }
        
        /// <summary>
        /// Returns the dot product between a vector and a normal.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static float VecDotNormal(Vec v, Normal w)
        {
            return v.x*w.x+v.y*w.y+v.z*w.z;
        }
        
        /// <summary>
        /// Returns the cross product between a vector and a normal.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
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
        /// Returns the cross product between two normals.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
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
        /// Returns the squared norm of a normal.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static float SquaredNorm(Normal n)
        {
            return n.x * n.x + n.y * n.y + n.z * n.z;
        }
        
        /// <summary>
        /// Returns the norm of a normal.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>

        public static float Norm(Normal n)
        {
            return (float) Math.Sqrt(SquaredNorm(n)) ;
        }
        
        /// <summary>
        /// Normalize the normal, so that it's norm is equal to 1.
        /// </summary>
        /// <returns></returns>
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
        /// Constructor, without any parameter.
        /// </summary>
        public Transformation()
        {
            m = Matrix4x4.Identity;
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
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
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
            return false;
        }
        
        /// <summary>
        /// Check whether two Transformations are equals.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool are_close(Transformation a, Transformation b)
        {
            if (are_close(a.m, b.m) & are_close(a.invm, b.invm))
            {
                return true; 
            }
            return false;
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
            return new Transformation(this.invm, this.m);
        }

        /// <summary>
        /// Product Transformation-Transformation, returns a Transformation.dot
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Transformation operator *(Transformation a, Transformation b)
        {
            return new Transformation(a.m*b.m, b.invm*a.invm);
        }
        
        /// <summary>
        /// Product Transformation-Point, returns a Point.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="p"></param>
        /// <returns>a point</returns>
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
            return q/w;
        }
        
        /// <summary>
        /// Product Transformation-Vec, returns a Vec.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="v"></param>
        /// <returns>a vector</returns>
        public static Vec operator *(Transformation a, Vec v)
        {
            return new Vec(v.x*a.m.M11 + v.y*a.m.M12 + v.z*a.m.M13, 
                            v.x*a.m.M21 + v.y*a.m.M22 + v.z*a.m.M23, 
                            v.x*a.m.M31 + v.y*a.m.M32 + v.z*a.m.M33);
        }
        
        /// <summary>
        /// Product Transformation-Normal, returns a Normal.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="n"></param>
        /// <returns>a normal</returns>
        public static Normal operator *(Transformation a, Normal n)
        {
            return new Normal(n.x*a.invm.M11 + n.y*a.invm.M21 + n.z*a.invm.M31, 
                                  n.x*a.invm.M12 + n.y*a.invm.M22 + n.z*a.invm.M32, 
                                  n.x*a.invm.M13 + n.y*a.invm.M23 + n.z*a.invm.M33);
        }
        
        /// <summary>
        /// Returns a Transformation object, encoding a rigid translation.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Transformation Translation(Vec v)
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
    
    //==================================================================================================================
    //Vec2d
    //==================================================================================================================
    
    /// <summary>
    /// A 2D vector, with two floating point fields u and v.
    /// </summary>
    public struct Vec2d
    {
        public float u;
        public float v;

        /// <summary>
        /// Constructor with float parameters.
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        public Vec2d(float u, float v)
        {
            this.u = u;
            this.v = v;

        }

        //METODI========================================================================================================
        
        /// <summary>
        /// Check weather two Vec2d objects are equals.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static bool are_close(Vec2d p, Vec2d q)
        {
            var epsilon = 1E-5;
            if (Math.Abs(p.u - q.u) < epsilon && Math.Abs(p.v - q.v) < epsilon)
            {
                return true;
            }
            return false;
        }
    }
}