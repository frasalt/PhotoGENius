using System.Numerics;
using System.Security.Cryptography;

namespace PGENLib
{
    public struct Color
    {
        public float r;
        public float g;
        public float b;

        public Color(float r, float g, float b) //Costruttore
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public override string ToString()
        {
            return $"({r}, {g}, {b})";
        }
    };

    //Somma 
    //public static add operator +(add c1, add c2)
    //{
    //return new add(c1.r + c2.r, c1.g + c2.g, c1.b + c2.b);
    //};

}
