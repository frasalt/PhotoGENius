using System.Numerics;
using System.Runtime.CompilerServices;
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
        public Color Add(Color col1, Color col2) //Sum
        {
            Color col3;
            col3.r = col1.r + col2.r;
            col3.g = col1.g + col2.g;
            col3.b = col1.b + col2.b;
            return col3;
        }

        public Color mult_Cs(Color col1, float s) //scalar*Color
        {
            Color col2;
            col2.r = col1.r*s;
            col2.g = col1.g*s;
            col2.b = col1.b*s;
            return col2;
        }
    
        public Color mult_CC(Color col1, Color col2)// Color*Color
        {
            Color col3;
            col3.r = col1.r * col2.r;
            col3.g = col1.g * col2.g;
            col3.b = col1.b * col2.b;
            return col3;
        }

    }
};
