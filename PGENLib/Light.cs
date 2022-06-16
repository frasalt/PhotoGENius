namespace PGENLib
{
    /// <summary>
    /// The class holds information about a pointlight (a Dirac's delta in the rendering equation) used by the
    /// PointLight Renderer. The fields are the following:
    /// <list type="table">
    /// <item>
    ///     <term>Position</term>
    ///     <description> a `Point` object holding the position of the point light in 3D space</description>
    /// </item>
    /// <item>
    ///     <term>Color</term>
    ///     <description> a `Color` object being the color of the point light</description>
    /// </item>
    /// <item>
    ///     <term>LinearRadius</term>
    ///     <description> a float number used to compute the solid angle subtended by the light at a
    ///     given distance d (r/d)^2</description>
    /// </item>
    /// </list>
    /// </summary>
    public struct PointLight
    {
        public Point Position;
        public Color Color;
        public float LinearRadius;

        public PointLight(Point position, Color color, float linearRadius = 0f)
        {
            Position = position;
            Color = color;
            LinearRadius = linearRadius;
        }
    }
}