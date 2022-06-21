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
    //==================================================================================================================
    //Pigments
    //==================================================================================================================
    
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
        /// <returns>the color of the pigment at the specified coordinates</returns>
        public virtual Color GetColor(Vec2d uv)
        {
            throw new NotImplementedException("Method Pigment.GetColor is abstract and cannot be called");
        }
    }
    
    //==================================================================================================================
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
        /// Associates a uniform color to the whole surface.
        /// </summary>
        /// <param name="uv"></param>
        /// <returns>Color</returns>
        public override Color GetColor(Vec2d uv)
        { 
            return Color;
        }
    }
    
    //==================================================================================================================
    /// <summary>
    /// A checkered pigment.
    /// The number of rows/columns in the checkered pattern is tunable, but you cannot have a different number of
    /// repetitions along the u/v directions.
    /// The parameters defined in this dataclass are the following:
    /// <list type="table">
    /// <item>
    ///     <term>Col1</term>
    ///     <description> first `Color`</description>
    /// </item>
    /// <item>
    ///     <term>Col2</term>
    ///     <description> second `Color`</description>
    /// </item>
    /// <item>
    ///     <term>NumStep</term>
    ///     <description> `int` number of repetition</description>
    /// </item>
    /// </list>
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
            if((intU % 2) == (intV % 2))
            {
                return Col1;
            }
            return  Col2;
        }
    }

    //==================================================================================================================
    /// <summary>
    /// The texture is given through a PFM image. The parameters defined in this dataclass are the following:
    /// <list type="table">
    /// <item>
    ///     <term>Image</term>
    ///     <description> `HdrImage`object</description>
    /// </item>
    /// </list>
    /// </summary>
    public class ImagePigment : Pigment
    {
        public HdrImage Image;
        public ImagePigment(HdrImage image)
        {
            Image = image;
        }

        public override Color GetColor(Vec2d uv)
        {
            int col = (int)(uv.u * Image.Width);
            int row = (int)(uv.v * Image.Height);
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

    //==================================================================================================================
    //BRDF
    //==================================================================================================================
    /// <summary>
    /// An abstract class representing a Bidirectional Reflectance Distribution Function (BRDF).
    /// </summary>
    public abstract class BRDF
    {
        public Pigment Pigment;

        /// <summary>
        /// Costructor without parameters, sets Pigment to a default UniformPigment(Black).
        /// </summary>
        public BRDF() 
        { 
            Pigment = new UniformPigment();
        }
        /// <summary>
        /// Constructor with parameter.
        /// </summary>
        /// <param name="pigment">: specify the chosen pigment.</param>
        public BRDF(Pigment pigment)
        {
            Pigment = pigment;
        }

        public virtual Color Eval(Normal normal, Vec inDir, Vec outDir, Vec2d uv)
        {
            return new Color(0.0f, 0.0f, 0.0f); //BLACK
        }

        /// <summary>
        /// Abstract method to be implemented in the BRDF derived classes.
        /// </summary>
        /// <param name="pcg"> Used to generate random numbers</param>
        /// <param name="incomingDir"> Direction of the incoming ray</param>
        /// <param name="interactionPoint"> Where the ray hit the surface</param>
        /// <param name="normal"> Normal on interactionPoint</param>
        /// <param name="depth"> Depth value for the new ray</param>
        /// <returns></returns>
        public virtual Ray ScatterRay(PCG pcg, Vec incomingDir, Point interactionPoint, Normal normal, int depth)
        {
            throw new NotImplementedException("Method BRDF.ScatterRay is abstract and cannot be called");
        }
    }

    //==================================================================================================================
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
            
        public override Ray ScatterRay(PCG pcg, Vec incomingDir, Point interactionPoint, Normal normal, int depth)
        {
            var onb = Vec.CreateOnbFromZ(normal);
            var cosThetaSq = pcg.RandomFloat();
            var cosTheta = (float)Math.Sqrt(cosThetaSq);
            var senTheta = (float)Math.Sqrt(1.0f - cosThetaSq);
            var phi = (float)(2.0f * Math.PI * pcg.RandomFloat());
            var direction = onb.e1 * (float)Math.Cos(phi) * cosTheta
                              + onb.e2 * (float)Math.Sin(phi) * cosTheta 
                              + onb.e3 * senTheta;
            var scatterRay = new Ray(interactionPoint, direction, 1.0E-3f, Single.PositiveInfinity, depth);
            return scatterRay;
        }
    }

    //==================================================================================================================
    /// <summary>
    /// A class representing an ideal mirror BRDF.
    /// </summary>
    public class SpecularBRDF : BRDF
    {
        public double TresholdAngleRad;
        
            
        public SpecularBRDF(Pigment pigment, double tresholdAngleRad = Math.PI/1800.0) : base(pigment)
        {
            TresholdAngleRad = tresholdAngleRad;
        }

        /// <summary>
        /// We provide this implementation for reference, but we are not going to use it (neither in the path tracer
        /// nor in the point-light tracer).
        /// </summary>
        /// <param name="normal">normal</param>
        /// <param name="inDir">Vec</param>
        /// <param name="outDir">Vec</param>
        /// <param name="uv">Vec2d</param>
        /// <returns></returns>
        public override Color Eval(Normal normal, Vec inDir, Vec outDir, Vec2d uv)
        {
            var thetaIn = Math.Acos(Vec.NormalizeDot(normal.ToVec(), inDir));
            var thetaOut = Math.Acos(Vec.DotProd(normal.ToVec(), outDir));

            if (Math.Abs(thetaIn - thetaOut) < TresholdAngleRad)
            {
                return Pigment.GetColor(uv);
            }

            return new Color(); //BLACK
        }
        
        public override Ray ScatterRay(PCG pcg, Vec incomingDir, Point interactionPoint, Normal normal, int depth)
        {
            //There is no need to use the PCG here, as the reflected direction is always completely deterministic
            // for a perfect mirror.
            var normalizedIncomingDir = incomingDir.NormalizeVec();
            var normalizedNormal = normal.ToVec().NormalizeVec();
            var dotProd = Vec.DotProd(normalizedIncomingDir, normalizedNormal);
            var direction = normalizedIncomingDir - normalizedNormal * 2.0f * dotProd;
            var scatterRay = new Ray(interactionPoint, direction, 1.0E-3f, float.PositiveInfinity, depth);
            return scatterRay;
        }
    }
    
    //==================================================================================================================
    //Material
    //==================================================================================================================
    /// <summary>
    /// A class representing a Material.
    /// The parameters defined in this dataclass are the following:
    /// <list type="table">
    /// <item>
    ///     <term>Brdf</term>
    /// </item>
    /// <item>
    ///     <term>EmittedRadiance</term>
    /// </item>
    /// </list>
    /// </summary>
    public class Material
    {
        public BRDF Brdf;
        public Pigment EmittedRadiance; 
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
            EmittedRadiance = emittedRadiance;
            Brdf = brdf;
        }
    }
}
