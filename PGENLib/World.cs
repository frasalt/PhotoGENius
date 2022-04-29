namespace PGENLib
{
    public struct World
    {

        public List<IShapes> Shapes;

        public World()
        {
            this.Shapes = new List<IShapes>();
        }

        /// <summary>
        /// Add a shape in the list of shapes present in the world.
        /// </summary>
        public void AddShape(IShapes sh)
        {
            Shapes.Append(sh);
        }

        public HitRecord? rayIntersection(Ray intRay, List<IShapes> Shapes)
        {
            HitRecord? closest = null;
             
            
            for (int i = 0; i < Shapes.Count; i++)
            {
                HitRecord? intersection;
                intersection = IShapes.RayIntersection(intRay);
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