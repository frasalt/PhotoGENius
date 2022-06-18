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