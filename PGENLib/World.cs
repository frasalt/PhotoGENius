namespace PGENLib
{
    public struct World
    {

        public List<Shape> Shapes;

        public World()
        {
            Shapes = new List<Shape>();
        }

        /// <summary>
        /// Add a shape in the list of shapes present in the world.
        /// </summary>
        public void AddShape(Shape sh)
        {
            Shapes.Add(sh);
        }

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
        
    }
}