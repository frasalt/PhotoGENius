namespace PGENLib
{
    public struct Color
    {
        public float r;
        public float g;
        public float b;

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Color()
        {
            r = 0;
            g = 0;
            b = 0;
        }

        /// <summary>
        /// Constructor asking for three floats per RGB.
        /// </summary>
        public Color(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }



        //========================= METHODS =====================================================================


        public void SetR(float x)
        {
            r = x;
        }
        public void SetG(float x)
        {
            g = x;
        }public void SetB(float x)
        {
            b = x;
        }
        
        public float GetR()
        {
            return r;
        }

        public float GetG()
        {
            return g;
        }

        public float GetB()
        {
            return b;
        }

        /// <summary>
        /// Returns the values of r, g, b as a string.
        /// </summary>
        public override string ToString()
        {
            return $"({r}, {g}, {b})";
        }

        /// <summary>
        ///  Returns the color sum (on the various components). 
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
        ///  Returns the color difference (on the various components). 
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
        ///  Returns the color multiplied by a scalar (on the various components).
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
        ///  Returns the product between two colors (on the various components).
        /// </summary>
        public static Color operator *(Color col1, Color col2)
        {
            Color col3;
            col3.r = col1.r * col2.r;
            col3.g = col1.g * col2.g;
            col3.b = col1.b * col2.b;
            return col3;
        }

        /// <summary>
        /// Check if two colors are the same.
        /// </summary>
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
        
        /// <summary>
        ///  Calculate the luminosity of a color (Shirley & Morley formula).
        /// </summary>
        public float Lum()
        {
            float lum = Math.Max((Math.Max(this.r, this.b)), this.g) + Math.Min((Math.Min(this.r, this.b)), this.g);
            return lum/2;
        }
    }
}

