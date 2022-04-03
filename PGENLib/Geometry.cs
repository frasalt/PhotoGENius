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
}