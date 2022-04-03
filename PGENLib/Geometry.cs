namespace PGENLib
{
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
}