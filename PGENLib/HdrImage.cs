namespace PGENLib
{
    public class HdrImage
    {
        // attributi dell'immagine
        public static int width;
        public static int height;
        public Color[] pixels; // un vettore di tipo Color che contiene tutti i pixel

        // Costruttore: 
        public HdrImage(int width_constr, int height_constr, Color[] pixels)
        {
            width = width_constr;
            height = height_constr;
            this.pixels = pixels;
            Color col = new Color();
            // create an empty image
            for (int i = 0; i < width * height; i++)
            {
                this.pixels[i] = col;
            }
        }

        /// <summary>
        /// Verifica che date coordinate abbiano valori sensati, ovvero compresi tra 0 e il numero di righe/colonne
        /// </summary>
        private bool ValidCoord(int x, int y)
        {
            return (x > 0 && y > 0 && x < width && y < width);
        }

        /// <summary>
        /// Data una coppia di coordinate, restituisce la posizione del pixel nel vettore di memorizzazione.
        /// </summary>
        private int PixelOffset(int x, int y)
        {
            //assert (this.ValidCoord(x, y)) ;
            return y * width + x;
        }
        
        /// <summary>
        /// Imposta il colore di un pixel di date coordinate
        /// </summary>
        private void SetPixel(int x, int y, Color new_col)
        {
            //assert (this.ValidCoord(x, y));
            this.pixels[this.PixelOffset(x, y)] = new_col;
        }
        /// <summary>
        /// Data una coppia di coordinate, restituisce il colore del pixel corrispondente
        /// </summary>
        private Color GetPixel(int x, int y)
        {
            // assert (this.ValidCoord(x, y)) ;
            return this.pixels[this.PixelOffset(x,y)];
        }
    }
} 


