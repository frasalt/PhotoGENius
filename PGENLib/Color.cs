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

namespace PGENLib
{
    // RGB Color
    // The class has three floating-point members: 'r' = red, `g`= green, and `b`= blue.
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
        /// Returns the color sum (on the various components).
        /// </summary>
        /// <param name="col1"></param>
        /// <param name="col2"></param>
        /// <returns>The color sum</returns>
        public static Color operator +(Color col1, Color col2)
        {
            col1.r += col2.r;
            col1.g += col2.g;
            col1.b += col2.b;
            return col1;
        }

        /// <summary>
        ///  Returns the color difference (on the various components).
        /// </summary>
        /// <param name="col1"></param>
        /// <param name="col2"></param>
        /// <returns>The color difference</returns>
        public static Color operator -(Color col1, Color col2)
        {
            col1.r -= col2.r;
            col1.g -= col2.g;
            col1.b -= col2.b;
            return col1;
        }

        /// <summary>
        /// Returns the color multiplied by a scalar (on the various components).
        /// </summary>
        /// <param name="col1"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Color operator *(Color col1, float s)
        {
            Color col2;
            col2.r = col1.r * s;
            col2.g = col1.g * s;
            col2.b = col1.b * s;
            return col2;
        }

        /// <summary>
        /// Returns the product between two colors (on the various components).
        /// </summary>
        /// <param name="col1"></param>
        /// <param name="col2"></param>
        /// <returns></returns>
        public static Color operator *(Color col1, Color col2)
        {
            Color col3;
            col3.r = col1.r * col2.r;
            col3.g = col1.g * col2.g;
            col3.b = col1.b * col2.b;
            return col3;
        }

        /// <summary>
        /// Check if two colors are similar enough to be considered equal.
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
        /// Calculate the luminosity of a color (Shirley and Morley formula).
        /// </summary>
        /// <returns></returns>
        public float Lum()
        {
            float lum = Math.Max((Math.Max(r, b)), g) + Math.Min((Math.Min(r, b)),g);
            return lum/2;
        }

        public static Color Black()
        {
            return new Color();
        }

        public static Color White()
        {
            return new Color(1.0f, 1.0f, 1.0f);
        }
    }
}