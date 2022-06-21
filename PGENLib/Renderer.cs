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
    //Renderers
    //==================================================================================================================
    /// <summary>
    /// A class implementing a solver of the rendering equation.
    /// This is an abstract class; you should use a derived concrete class.
    /// This class has 2 members:
    /// <list type="table">
    /// <item>
    ///     <term> World</term>
    ///     <description> type `World`</description>
    /// </item>
    /// <item>
    ///     <term>BackGroundColor</term>
    ///     <description> type `Color`</description>
    /// </item>
    /// </list>
    /// </summary>
    public abstract class Renderer
    {
        public World World;
        public Color BackgroundColor;

        protected Renderer(World world, Color backgroundColor = default) //Default color is BLACK
        {
            World = world;
            BackgroundColor = backgroundColor;
        }

        /// <summary>
        /// Virtual method to Estimate the radiance along a ray.
        /// Find it overwritten in class OnOffRenderer and FlatRenderer
        /// </summary>
        /// <param name="ray"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual Color Call(Ray ray)
        {
            throw new NotImplementedException("Method Renderer.Call is abstract and cannot be called");
        }
    }

    //==================================================================================================================
    //Onoff renderer
    //==================================================================================================================
    /// <summary>
    /// A on/off renderer, produces black and white images and is useful for debugging purposes.
    /// Other than Renderer's member it has the third member
    /// <list type="table">
    /// <item>
    ///     <term> Color</term>
    ///     <description> type `Color`</description>
    /// </item>
    /// </list>
    /// </summary>
    public class OnOffRenderer : Renderer
    {
        public Color Color;

        public OnOffRenderer(World world, Color backGroundColor = default) : this(new Color(1.0f, 1.0f, 1.0f), world, backGroundColor)
        {
        }

        public OnOffRenderer(Color color, World world, Color backGroundColor = default) : base(world, backGroundColor)
        {
            Color = color;
        }
        
        public override Color Call(Ray ray)
        {
            return World.RayIntersection(ray) == null ? BackgroundColor : Color;
        }
    }

    //==================================================================================================================
    //Flat renderer
    //==================================================================================================================
    /// <summary>
    /// This renderer estimates the solution of the rendering equation by neglecting any contribution of the light.
    /// It just uses the pigment of each surface to determine how to compute the final radiance.
    /// </summary>
    public class FlatRenderer : Renderer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="world">World</param>
        /// <param name="backGroundColor">Color</param>
        public FlatRenderer(World world, Color backGroundColor = default) : base(world, backGroundColor){}

        /// <summary>
        /// Estimate the radiance along a ray solving the rendering equation.
        /// </summary>
        /// <param name="ray">Ray</param>
        /// <returns>Color</returns>
        public override Color Call(Ray ray)
        {
            var hit = World.RayIntersection(ray);
            if (hit == null)
            {
                return BackgroundColor;
            }
            var material = hit.Value.Material;
            var tot = material.Brdf.Pigment.GetColor(hit.Value.SurfacePoint);
            var tot2 = material.EmittedRadiance.GetColor(hit.Value.SurfacePoint);
            return tot + tot2;
        }
    }

    //==================================================================================================================
    //PathTracer renderer
    //==================================================================================================================
    /// <summary>
    /// The algorithm implemented here allows the caller to tune number of rays thrown at each iteration, as well as the
    /// maximum depth. It implements Russian roulette to reduce the number of recursive calls.
    /// <list type="table">
    /// <item>
    ///     <term>Pcg</term>
    ///     <description> Random number generator</description>
    /// </item>
    /// <item>
    ///     <term>NumbOfRays</term>
    ///     <description> Number of rays to be fired at each iteration</description>
    /// </item>
    /// <item>
    ///     <term>MaxDepth</term>
    ///     <description> Maximum number of reflections for any ray </description>
    /// </item>
    ///  <item>
    ///     <term>RussianRouletteLimit</term>
    ///     <description> Minimum number of reflections for the Russian Roulette algorith to start </description>
    /// </item>
    /// </list>
    /// </summary>
    public class PathTracer : Renderer
    {
        public PCG Pcg;
        public int NumbOfRays;
        public int MaxDepth; 
        public int RussianRouletteLimit;
        
        public PathTracer(World world, PCG pcg, int numbOfRays = 10, int maxDepth = 2, int russianRouletteLimit = 3,
            Color backgroundColor = default) : base(world, backgroundColor)
        {
            Pcg = pcg;
            NumbOfRays = numbOfRays;
            MaxDepth = maxDepth;
            RussianRouletteLimit = russianRouletteLimit;
        }

        /// <summary>
        /// Estimate the radiance along a ray solving the rendering equation.
        /// </summary>
        /// <param name="ray">Ray</param>
        /// <returns>Color</returns>
        public override Color Call(Ray ray)
        {
            if (ray.Depth > MaxDepth)
            {
                return new Color(0.0f, 0.0f, 0.0f);
            }

            var hitRecord = World.RayIntersection(ray);
            if (hitRecord == null)
            {
                return BackgroundColor;
            }

            var hitMaterial = hitRecord.Value.Material;
            var hitColor = hitMaterial.Brdf.Pigment.GetColor(hitRecord.Value.SurfacePoint);
            var emittedRadiance = hitMaterial.EmittedRadiance.GetColor(hitRecord.Value.SurfacePoint);
            var m1 = Math.Max(hitColor.r, hitColor.g);
            var hitColorLum = Math.Max(m1, hitColor.b);
            
            //Russian roulette
            if (ray.Depth >= RussianRouletteLimit)
            {
                var q = (float)Math.Max(0.05, 1 - hitColorLum);
                if (Pcg.RandomFloat() > q)
                {
                    //Keep the recursion going, but compensate for other potentially discarded rays
                    hitColor *= 1.0f / (1.0f - q);
                }
                else
                {
                    //Terminate prematurely
                    return emittedRadiance;
                }
                
            }
            
            //Monte Carlo integration
            var cumRadiance = new Color(); //Black
            if (hitColorLum > 0.0f)
            {
                for (var rayIndex = 0; rayIndex<NumbOfRays; rayIndex++)
                {
                    var newRay = hitMaterial.Brdf.ScatterRay(Pcg, hitRecord.Value.Ray.Dir, hitRecord.Value.WorldPoint,
                        hitRecord.Value.Normal, ray.Depth+1);
                    var newRadiance = Call(newRay); //Recursive call
                    cumRadiance += hitColor * newRadiance;
                }
            }

            return emittedRadiance + cumRadiance * (1.0f / NumbOfRays);
        }
    }

    //==================================================================================================================
    //Pointlight renderer
    //==================================================================================================================
    /// <summary>
    /// Class that implements a Point Light renderer. The solid angle integral of the rendering equations
    /// can be simplified, because the integrand contains some localized Dirac deltas.
    /// </summary>
    public class PointLightRenderer : Renderer
    {
        public Color AmbientColor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="world">World</param>
        /// <param name="backGroundColor">Color</param>
        public PointLightRenderer(World world, Color backGroundColor = default) : this(new Color(0f, 0f, 0f), world, backGroundColor)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ambientColor">Color</param>
        /// <param name="world">World</param>
        /// <param name="backGroundColor">Color</param>
        public PointLightRenderer(Color ambientColor, World world, Color backGroundColor = default) : base(world, backGroundColor)
        {
            AmbientColor = ambientColor;
        }
        
        /// <summary>
        /// Estimate the radiance along a ray solving the rendering equation.
        /// </summary>
        /// <param name="ray">Ray</param>
        /// <returns>Color</returns>
        public override Color Call(Ray ray)
        {
            var hitRecord = World.RayIntersection(ray);
            if (hitRecord == null)
            {
                //The ray didn't hit
                return BackgroundColor;
            }
            
            var hitMaterial = hitRecord.Value.Material;
            var resultColor = AmbientColor;
            //Check the contribution of each light 
            foreach (var curLight in World.PointLights)
            {
                if (World.IsPointVisible(curLight.Position, hitRecord.Value.WorldPoint))
                {
                    var distanceVec = hitRecord.Value.WorldPoint - curLight.Position;
                    var distance = Vec.Norm(distanceVec);
                    var inDir = distanceVec * (1.0f / distance);
                    var cosTheta = Math.Max(0.0f, Vec.NormalizeDot(-ray.Dir, hitRecord.Value.Normal));
                    float distanceFactor;
                    //Compute distance factor
                    if (curLight.LinearRadius > 0.0f)
                    {
                        distanceFactor = (float) Math.Pow((curLight.LinearRadius / distance), 2.0f);
                    }
                    else
                    {
                        distanceFactor = 1.0f;
                    }
                    //Compute emitted radiance and brdf and combine them 
                    var emittedColor = hitMaterial.EmittedRadiance.GetColor(hitRecord.Value.SurfacePoint);
                    var brdfColor = hitMaterial.Brdf.Eval(
                        hitRecord.Value.Normal,
                        inDir,
                        -ray.Dir,
                        hitRecord.Value.SurfacePoint
                    );
                    resultColor += (emittedColor + brdfColor) * curLight.Color * cosTheta * distanceFactor;
                }
            }
            
            return resultColor;
        }
    }
}