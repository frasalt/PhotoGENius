namespace PGENLib;
//using class Color;
{
    public class HdrImage
    {
        public static int width;
        public static int height;
        public static Color[] pixels;

        public HdrImage(int width_constr = 0, int height_constr = 0)
        {
            width = width_constr;
            height = height_constr;
            // create an empty image
            for (int i = 0; i < HdrImage.width * HdrImage.height; i++)
            {
                HdrImage.pixels[i] = Color(0, 0, 0);
            }
        }
    }
} 
/*    
}public class HdrImage
{
    public static int width;
    public static int height;
    public static Color[] pixels;
    public HdrImage(int width_constr=0, int height_constr=0)
    {
        width = width_constr;
        height = height_constr;
        // create an empty image
        for (int i = 0; i < HdrImage.width * HdrImage.height; i++)
        {
            HdrImage.pixels[i] = Color(0,0,0);
        }
    }
}
*/