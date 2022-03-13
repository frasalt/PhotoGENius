using System.Numerics;
using System.Security.Cryptography;

namespace PGENLib
{
    public struct Color
    {
        private float r;
        private float g;
        private float b;

        //Costruttori---------------------------------
        public Color()
        {
            this.r = 0;
            this.g = 0;
            this.b = 0;
        }

        public Color(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        //Metodi-------------------------------------
        public override string ToString()
        {
            return $"({r}, {g}, {b})";
        }

        public static Color operator +(Color c1, Color c2)
        {
            Color c = new Color();
            c.r = c1.r + c2.r;
            c.g = c1.g + c2.g;
            c.b = c1.b + c2.b;
            return c;
        }
        

    };

    
