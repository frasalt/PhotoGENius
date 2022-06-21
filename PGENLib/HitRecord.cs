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
    //HitRecords
    //==================================================================================================================
    /// <summary>
    /// A class holding information about a ray-shape intersection.
    /// The parameters defined in this dataclass are the following:
    /// <list type="table">
    /// <item>
    ///     <term>WorldPoint</term>
    ///     <description> a `Point` object holding the world coordinates of the hit point</description>
    /// </item>
    /// <item>
    ///     <term>Normal</term>
    ///     <description> a `Normal` object holding the orientation of the normal to the surface where the hit happened</description>
    /// </item>
    /// <item>
    ///     <term>SurfacePoint</term>
    ///     <description> a `Vec2d` object holding the position of the hit point on the surface of the object</description>
    /// </item>
    /// <item>
    ///     <term>t</term>
    ///     <description> a floating-point value specifying the distance from the origin of the ray where the hit happened</description>
    /// </item>
    /// <item>
    ///     <term>Ray</term>
    ///     <description> a `Ray` object that hit the surface</description>
    /// </item>
    /// <item>
    ///     <term>Material</term>
    ///     <description> the parameter `Material` of the `Shape` object hit by the ray </description>
    /// </item>
    /// </list>
    /// </summary>
    
    public struct HitRecord
    {
        public Point WorldPoint;
        public Normal Normal;
        public Vec2d SurfacePoint;
        public float t;
        public Ray Ray;
        public Material Material;
        
        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="worldPoint"></param>
        /// <param name="normal"></param>
        /// <param name="surfacePoint"></param>
        /// <param name="t"></param>
        /// <param name="ray"></param>
        /// <param name="material"></param>
        public HitRecord(Point worldPoint, Normal normal, Vec2d surfacePoint, float t, Ray ray, Material material)
        {
            WorldPoint = worldPoint;
            Normal = normal;
            SurfacePoint = surfacePoint;
            this.t = t;
            Ray = ray;
            Material = material;
        }
        
        //METODI========================================================================================================
        
        /// <summary>
        /// Check weather two HitRecord objects are equals.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool are_close(HitRecord a, HitRecord b)
        {
            var epsilon = 1E-5;
            if (Point.are_close(a.WorldPoint, b.WorldPoint) &&
                Normal.are_close(a.Normal, b.Normal) &&
                Vec2d.are_close(a.SurfacePoint, b.SurfacePoint) &&
                Math.Abs(a.t-b.t)<epsilon &&
                Ray.are_close(a.Ray, b.Ray))
            {
                return true;
            }
            return false;
        }
    }
}
