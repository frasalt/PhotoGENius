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
        ///  Restituisce il vettore diviso per uno scalare 
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
        
        /// <summary>
        ///  Trasforma un vettore in una normale
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
        /// <summary>
        ///  Restituisce il punto diviso per uno scalare 
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
        /// Confronta due matrici e restituisce true se coincidono, false altrimenti; 
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
        /// Confronta due matrici e restituisce true se coincidono, false altrimenti; 
        /// </summary>
        public static bool are_close(Transformation a, Transformation b)
        {
            var epsilon = 1E-5;
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
        /// Verifica che i parametri di una trasformazione siano trasformazione e relativa inversa. 
        /// </summary>
        public bool IsConsistent()
        {
            return are_close(m*invm, Matrix4x4.Identity);
        }
        
        /// <summary>
        /// Restituisce l'inverso di una trasformazione. 
        /// </summary>
        public Transformation Inverse()
        {
            Transformation tr = new Transformation(this.invm, this.m);
            return tr;
        }

        /// <summary>
        /// Prodotto tra Transformation, restituisce Transformation
        /// </summary>
        public static Transformation operator *(Transformation a, Transformation b)
        {
            Transformation tr = new Transformation(a.m*b.m, b.invm*a.invm);
            return tr;
        }
        
        /// <summary>
        /// Prodotto con Point, restiruisce Point
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
        /// Prodotto con Vec, restiruisce Vec
        /// </summary>
        public static Vec operator *(Transformation a, Vec v)
        {
            Vec q = new Vec(v.x*a.m.M11 + v.y*a.m.M12 + v.z*a.m.M13, 
                            v.x*a.m.M21 + v.y*a.m.M22 + v.z*a.m.M23, 
                            v.x*a.m.M31 + v.y*a.m.M32 + v.z*a.m.M33);
            return q;
        }
        
        /// <summary>
        /// Prodotto con Normal, restiruisce Normal
        /// </summary>
        public static Normal operator *(Transformation a, Normal n)
        {
            Normal q = new Normal(n.x*a.invm.M11 + n.y*a.invm.M21 + n.z*a.invm.M31, 
                                  n.x*a.invm.M12 + n.y*a.invm.M22 + n.z*a.invm.M32, 
                                  n.x*a.invm.M13 + n.y*a.invm.M23 + n.z*a.invm.M33);
            return q;
        }
        /// <summary>
        /// Metodo che restituisce una matrice di traslazione
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
        /// Metodo che restituisce una matrice di rotazione
        /// </summary>
        public static Transformation Rotation(Vec v)
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
        
    }
    
    

    

}