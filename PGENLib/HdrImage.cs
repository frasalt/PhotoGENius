namespace PGENLib;
public class HdrImage
{
    public int width;
    public int height;
    public Color[] pixels;
    public HdrImage(int width_constr=0, int height_constr=0)
    {
        width = width_constr;
        height = height_constr;
        // create an empty image
        for (int i = 0; i < HdrImage.width * HdrImage.height; i++)
        {
            HdrImage.pixels[i] = Color();
        }
    }
}
