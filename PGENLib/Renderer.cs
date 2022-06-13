namespace PGENLib
{
    /// <summary>
    /// A class implementing a solver of the rendering equation.
    /// This is an abstract class; you should use a derived concrete class.
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

    /// <summary>
    /// A on/off renderer, produces black and white images and is useful for debugging purposes.
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

    public class FlatRenderer : Renderer
    {
        public FlatRenderer(World world, Color backGroundColor = default) : base(world, backGroundColor){}

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

    /// <summary>
    /// A simple point light Renderer, similar to the one that POV-Ray provides by default.
    /// </summary>
    public class PointLightRenderer : Renderer
    {
        public Color AmbientColor;

        public PointLightRenderer(World world, Color backGroundColor = default) : this(new Color(1.0f, 1.0f, 1.0f), world, backGroundColor)
        {
        }

        public PointLightRenderer(Color ambientColor, World world, Color backGroundColor = default) : base(world, backGroundColor)
        {
            AmbientColor = ambientColor;
        }
        
        public override Color Call(Ray ray)
        {
            var hitRecord = World.RayIntersection(ray);
            if (hitRecord == null)
            {
                return BackgroundColor;
            }

            var hitMaterial = hitRecord.Value.Material;
            var resultColor = AmbientColor;
            foreach (var curLight in World.PointLights)
            {
                if (World.IsPointVisible(curLight.Position, hitRecord.Value.WorldPoint))
                {
                    var distanceVec = hitRecord.Value.WorldPoint - curLight.Position;
                    var distance = Vec.Norm(distanceVec);
                    var inDir = distanceVec * (1.0f / distance);
                    var cosTheta = Math.Max(0.0f, Vec.NormalizeDot(-ray.Dir, hitRecord.Value.Normal));
                    float distanceFactor;
                    if (curLight.LinearRadius > 0.0f)
                    {
                        distanceFactor = (float) Math.Pow((curLight.LinearRadius / distance), 2.0f);
                    }
                    else
                    {
                        distanceFactor = 1.0f;
                    }

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