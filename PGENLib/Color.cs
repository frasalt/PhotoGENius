namespace PGENLib
{
    public struct Color
    { 

        public Color(double r, double g, double b) //Costruttore
        {
            this.r = 0;
            this.g = 0;
            this.b = 0;
        }
        public double r { get; }
        public double g { get; }
        public double b { get; }
        public override string ToString() => $"({r}, {g}, {b})";
    }
}