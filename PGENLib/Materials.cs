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
    /// <summary>
    /// This abstract class represents a pigment with a function that associates a color with
    /// each point on a parametric surface (u,v).
    /// </summary>
    public abstract class Pigment
    {
        protected Color Color;

        protected Pigment(Color color = default) //Default(Color)=BLACK
        {
            Color = color;
        }
        
        /// <summary>
        /// Calculate a color (type color) associated with a (u,v) coordinate.
        /// </summary>
        /// <param name="uv"> 2D vector </param>
        /// <returns></returns>
        public virtual Color GetColor(Vec2d uv)
        {
            //Color pigm = new Color();
            //return pigm;
            throw new NotImplementedException("Method Pigment.GetColor is abstract and cannot be called");

        }
    }
    /// <summary>
    /// A uniform pigment.
    /// </summary>
    public class UniformPigment : Pigment
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="color"></param>

        public UniformPigment(Color color = default) : base(color)
        {
        }
        
        /// <summary>
        /// Associates a uniform color to the hole surface.
        /// </summary>
        /// <param name="uv"></param>
        /// <returns></returns>
        public override Color GetColor(Vec2d uv)
        { 
            return Color;
        }
    }
    /// <summary>
    /// A checkered pigment.
    /// The number of rows/columns in the checkered pattern is tunable, but you cannot have a different number of
    /// repetitions along the u/v directions.
    /// </summary>
    public class CheckeredPigment : Pigment
    {
        public Color Col1; 
        public Color Col2; 
        public int NumStep;

        public CheckeredPigment(Color col1, Color col2, int numStep = 10)
        {
            Col1 = col1;
            Col2 = col2;
            NumStep = numStep;
        }
            
        public override Color GetColor(Vec2d uv)
        {
            var intU = (int)Math.Floor(uv.u * NumStep); 
            var intV = (int)Math.Floor(uv.v * NumStep);
            var r1 = (intU % 2);
            var r2 = (intV % 2);
            if(r1 == r2)
            {
                return Col1;
            }
            return  Col2;
        }
    }

        public class ImagePigment : Pigment
        {
            public HdrImage Image;
            public ImagePigment(HdrImage image)
            {
                Image = image;
            }

            public override Color GetColor(Vec2d uv)
            {
                int col = (int)uv.u * Image.Width;
                int row = (int)uv.v * Image.Height;
                if (col >= Image.Width)
                {
                    col = Image.Width - 1;
                }

                if (row >= Image.Height)
                {
                    row = Image.Height - 1;
                }

                return Image.GetPixel(col, row);
            }
        }

        /// <summary>
        /// An abstract class representing a Bidirectional Reflectance Distribution Function (BRDF.
        /// </summary>
        public abstract class BRDF
        {
            public Pigment Pigment;

            public BRDF()
            {
                Pigment = new UniformPigment();
            }
            public BRDF(Pigment pigment)
            {
                Pigment = pigment;
            }

            public virtual Color Eval(Normal normal, Vec inDir, Vec outDir, Vec2d uv)
            {
                return new Color(0.0f, 0.0f, 0.0f); //BLACK
            }
            
        }

        /// <summary>
        /// A class representing an ideal diffuse BRDF.
        /// </summary>
        public class DiffuseBRDF : BRDF
        {
            public DiffuseBRDF() {} //Uses the BRDF default constructor
            
            public DiffuseBRDF(Pigment pigment) : base(pigment){}

            public override Color Eval(Normal normal, Vec inDir, Vec outDir, Vec2d uv)
            {
                return Pigment.GetColor(uv) * (float) (1.0f / Math.PI);
            }
        }
    
        /// <summary>
        /// A class representing a Material.
        /// The parameters defined in this dataclass are the following:
        /// <list type="table">
        /// <item>
        ///     <term>_brdf</term>
        /// </item>
        /// <item>
        ///     <term>_emittedRadiance</term>
        /// </item>
        /// </list>
        /// </summary>
        public class Material
        {
            protected internal BRDF Brdf; 
            protected internal Pigment EmittedRadiance; 
            public Material()
            {
                Brdf = new DiffuseBRDF();
                EmittedRadiance = new UniformPigment(new Color(0.0f, 0.0f, 0.0f)); //BLACK
            }

            public Material(BRDF brdf) : this(new UniformPigment(), brdf)
            {
            }

            public Material(Pigment emittedRadiance, BRDF brdf)
            {
                Brdf = brdf;
                EmittedRadiance = emittedRadiance;
            }
            
        }
        
}
