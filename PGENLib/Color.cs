namespace PGENLib
{
    public struct Color
    {
        private float r;
        private float g;
        private float b;

        /// <summary>
        /// Costruttore vuoto
        /// </summary>
        public Color()
        {
            this.r = 0;
            this.g = 0;
            this.b = 0;
        }

        /// <summary>
        /// Costruttore non vuoto, chiede tre float per RGB
        /// </summary>
        public Color(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        //Metodi-------------------------------------

        /// <summary>
        /// Restituisce i valori di r,g,b in forma di stringa
        /// </summary>
        public override string ToString()
        {
            return $"({r}, {g}, {b})";
        }

        /// <summary>
        ///  Restituisce il colore somma (sulle varie componenti) 
        /// </summary>
        public static Color operator +(Color col1, Color col2)
        {
            Color col3 = new Color();
            col3.r = col1.r + col2.r;
            col3.g = col1.g + col2.g;
            col3.b = col1.b + col2.b;
            return col3;
        }

        /// <summary>
        ///  Restituisce il colore differenza (sulle varie componenti) 
        /// </summary>
        public static Color operator -(Color col1, Color col2)
        {
            Color col3 = new Color();
            col3.r = col1.r - col2.r;
            col3.g = col1.g - col2.g;
            col3.b = col1.b - col2.b;
            return col3;
        }

        /// <summary>
        ///  Restituisce il colore moltiplicato per uno scalare (sulle varie componenti) 
        /// </summary>
        public static Color operator *(Color col1, float s)
        {
            Color col2;
            col2.r = col1.r * s;
            col2.g = col1.g * s;
            col2.b = col1.b * s;
            return col2;
        }
        
        /// <summary>
        ///  Restituisce il prodotto tra due colori (sulle varie componenti) 
        /// </summary>
        public static Color operator *(Color col1, Color col2)
        {
            Color col3;
            col3.r = col1.r * col2.r;
            col3.g = col1.g * col2.g;
            col3.b = col1.b * col2.b;
            return col3;
        }

        public static bool are_close(Color p, Color q)
        {
            double epsilon = 1E-5;
            if (Math.Abs(p.r - q.r) < epsilon & Math.Abs(p.g - q.g) < epsilon & Math.Abs(p.b - q.b) < epsilon)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

