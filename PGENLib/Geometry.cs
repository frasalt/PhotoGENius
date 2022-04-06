using System.Numerics;
using System.Runtime.CompilerServices;

namespace PGENLib
{
    //==================================================================================================================
    //Vec
    //==================================================================================================================
    public struct Vec
    {
        public float x;
        public float y;
        public float z;

        /// <summary>
        /// Costruttore vuoto
        /// </summary>
        public Vec()
        {
            x = 0;
            y = 0;
            z = 0;
        }
        
        /// <summary>
        /// Costruttore con parametri float 
        /// </summary>
        public Vec(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        //METODI========================================================================================================
        /// <summary>
        /// Restituisce i valori di x,y,z in forma di stringa
        /// </summary>
        public override string ToString()
        {
            return $"Vec(x={x}, y={y}, z={z})";
        }
        
        /// <summary>
        /// Confronta due vettori e restituisce true se coincidono, false altrimenti; 
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
        ///  Restituisce il vettore somma  
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
        ///  Restituisce il vettore differenza 
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
        ///  Restituisce il vettore moltiplicato per uno scalare 
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
        ///  Restituisce -v, dato un vettore v
        /// </summary>
        public Vec Neg()
        {
            float s = -1;
            return this*s;
        }
        
        /// <summary>
        ///  Restituisce il prodotto scalare 
        /// </summary>
        public static float DotProd(Vec v, Vec w)
        {
            return v.x*w.x+v.y*w.y+v.z*w.z;
        }
        
        /// <summary>
        ///  Restituisce il prodotto vettoriale 
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
        ///  Restituisce la norma al quadrato di un vettore 
        /// </summary>
        public static float SquaredNorm(Vec v)
        {
            return DotProd(v,v);
        }
        
        /// <summary>
        ///  Restituisce la norma di un vettore 
        /// </summary>
        public static float Norm(Vec v)
        {
            return (float) Math.Sqrt(DotProd(v,v));
        }
        
        /// <summary>
        ///  Restituisce il vettore normalizzato 
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
        /// Costruttore vuoto
        /// </summary>
        public Point()
        {
            x = 0;
            y = 0;
            z = 0;
        }
        
        /// <summary>
        /// Costruttore con parametri float 
        /// </summary>
        public Point(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        //METODI========================================================================================================
        /// <summary>
        /// Restituisce i valori di x,y,z in forma di stringa
        /// </summary>
        public override string ToString()
        {
            return $"Point(x={x}, y={y}, z={z})";
        }
        
        /// <summary>
        /// Confronta due vettori e restituisce true se coincidono, false altrimenti; 
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
        ///  Somma tra punto e vettore, restituisce punto
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
        ///  Prodotto punto*scalare, restituisce punto
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
        ///  Restituisce il vettore differenza tra due punti 
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
        ///  Differenza tra punto e vettore, restituisce un punto 
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
        ///  Restituisce il vettore moltiplicato per uno scalare 
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
    }
    //==================================================================================================================
    //Normal
    //==================================================================================================================
    public struct Normal
    {
        public float x;
        public float y;
        public float z;

        /// <summary>
        /// Costruttore vuoto
        /// </summary>
        public Normal()
        {
            x = 0;
            y = 0;
            z = 0;
        }
        
        /// <summary>
        /// Costruttore con parametri float 
        /// </summary>
        public Normal(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        //METODI========================================================================================================
        /// <summary>
        /// Restituisce i valori di x,y,z in forma di stringa
        /// </summary>
        public override string ToString()
        {
            return $"Normal(x={x}, y={y}, z={z})";
        }
        
        /// <summary>
        /// Confronta due normali e restituisce true se coincidono, false altrimenti; 
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
        ///  Restituisce la normale moltiplicata per uno scalare 
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
        ///  Restituisce -n, data una normale n
        /// </summary>
        public Normal Neg()
        {
            const float s = -1;
            return this*s;
        }
        
        /// <summary>
        ///  Restituisce il prodotto scalare  tra un vettore e una normale
        /// </summary>
        public static float VecDotNormal(Vec v, Normal w)
        {
            return v.x*w.x+v.y*w.y+v.z*w.z;
        }
        
        /// <summary>
        ///  Restituisce il prodotto vettoriale Vec x Normal
        /// </summary>
        public static Normal VecCrossNormal(Vec v, Normal w)
        {
            var t = new Normal     //Restituisce un vettore??? Si arrabbia se lo metto vec
            {
                x = v.y*w.z - v.z*w.y,
                y = v.z*w.x - v.x*w.z,
                z = v.x*w.y - v.y*w.x
            };
            return t;
        }
        /// <summary>
        ///  Restituisce il prodotto vettoriale Normal x Normal
        /// </summary>
        public static Normal NormalCrossNormal(Normal v, Normal w)
        {
            var t = new Normal //Restituisce un vettore ???
            {
                x = v.y*w.z - v.z*w.y,
                y = v.z*w.x - v.x*w.z,
                z = v.x*w.y - v.y*w.x
            };
            return t;
        }

        /// <summary>
        ///  Restituisce la norma al quadrato
        /// </summary>
        public static float SquaredNorm(Normal n)
        {
            return n.x * n.x + n.y * n.y + n.z * n.z;
        }
        /// <summary>
        ///  Restituisce la norma
        /// </summary>
        
        public static float Norm(Normal n)
        {
            return (float) Math.Sqrt(SquaredNorm(n)) ;
        }
        
        /// <summary>
        ///  Funzione che normalizza la normale
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
    public struct Transformation
    {
        public Matrix4x4 m;
        public Matrix4x4 invm;
        
        /// <summary>
        /// Costruttore senza parametri, trasformazione identità lascia il vettore su cui agisce invariato
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
            invm = new Matrix4x4(1,0,0,0, 
                                 0,1,0,0,
                                 0,0,1,0,
                                 0,0,0,1);
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
        public static bool are_matrix_close(Matrix4x4 a, Matrix4x4 b)
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
        public bool IsConsistent()
        {
            return are_matrix_close(m*invm, Matrix4x4.Identity);
        }
        



    }
}