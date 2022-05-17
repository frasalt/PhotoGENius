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
}