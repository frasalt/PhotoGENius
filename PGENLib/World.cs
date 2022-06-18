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
    public struct World
    { 
        /// <summary>
        /// Class that represents a world. It is the variable to which we add the shapes.
        /// </summary>

        public List<Shape> Shapes;
        public List<PointLight> PointLights;

        /// <summary>
        /// Empty constructor
        /// </summary>
        public World()
        {
            PointLights = new List<PointLight>();
            Shapes = new List<Shape>();
        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="shapes"></param>
        /// <param name="pointLights"></param>
        public World(List<Shape> shapes, List<PointLight> pointLights)
        {
            PointLights = pointLights;
            Shapes = shapes;
        }
        
        /// <summary>
        /// Add a shape in the list of shapes present in the world.
        /// </summary>
        /// <param name="sh">  `Shape` object to be added in the world</param>
        public void AddShape(Shape sh)
        {
            Shapes.Add(sh);
        }

        /// <summary>
        /// Add a ligth in the list of shapes present in the world.
        /// </summary>
        /// <param name="light">`PointLight` object to be added in the world</param>
        public void AddLight(PointLight light)
        {
            PointLights.Add(light);
        }

        /// <summary>
        /// Determine whether a ray intersects any of the objects in this world.
        /// </summary>
        /// <param name="intRay"></param>
        /// <returns></returns>
        public HitRecord? RayIntersection(Ray intRay)
        {
            HitRecord? closest = null;
            
            for (int i = 0; i < Shapes.Count; i++)
            {
                HitRecord? intersection;
                intersection = Shapes[i].RayIntersection(intRay);
                if (intersection == null) continue;
                if (closest == null || closest.Value.t > intersection.Value.t)
                {
                    closest = intersection;
                }
            }
            return closest;
        }
        
        /// <summary>
        /// Checks wheater a point is visible.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="observerPosition"></param>
        /// <returns></returns>
        public bool IsPointVisible(Point point, Point observerPosition)
        {
            var direction = point - observerPosition;
            var directionNorm = Vec.Norm(direction);
            var tmin = (float) 1e-2 / directionNorm;

            var ray = new Ray(origin: observerPosition, dir: direction, tmin: tmin, tmax: 1.0f);
            foreach (var t in Shapes)
            {
                if (t.QuickRayIntersection(ray))
                {
                    return false;
                }
            }

            return true;
        }
        
    }
}