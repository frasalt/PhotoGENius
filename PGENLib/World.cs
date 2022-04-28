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
            this.Shapes.Append(sh);
        }
           
        /*


            def ray_intersection(self, ray: Ray) -> Optional[HitRecord]:
        closest = None # "closest" should be a nullable type!
        for shape in self.shapes:
        intersection = shape.ray_intersection(ray)

        if not intersection:
        continue

        if (not closest) or(intersection.t<closest.t):
        closest = intersection

        return closest
        */
    }
}