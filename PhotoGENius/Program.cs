// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;
using PGENLib;
using static PGENLib.Color;
class Program
{
    static void Main()
    {
        Console.WriteLine("Hello, World!");
        // Prova colore
        Color red = new Color(255,0,0);
        Console.WriteLine(red);
        Color green = new Color(0, 255, 0);
        //prova somma
        Color somma = new Color();
        somma = somma.Add(red, green);
        Console.WriteLine(somma);
        //prova col*scal
        Color mul = new Color();
        float a = 5;
        mul = mul.mult_Cs(green, a);
        Console.WriteLine(mul);
        //
        Color mul2 = new Color();
        mul2 = mul2.mult_CC(green, red);
        Console.WriteLine(mul2);
        
        
        
    }
}
