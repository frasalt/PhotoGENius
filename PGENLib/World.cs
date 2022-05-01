namespace PGENLib
{
    public struct World
    {

        public List<Shape> Shapes;

        public World()
        {
            this.Shapes = new List<Shape>();
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
                if (closest == null || (float)closest?.t > (float)intersection?.t)
                {
                    closest = intersection;
                }
            }

            return closest;
        }
        
    }
}