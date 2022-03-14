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
        Console.WriteLine("Rosso:" + red);
        Color green = new Color(0, 255, 0);
        Console.WriteLine("Verde:" + green);
        
        //prova somma
        Color somma = red + green;
        Console.WriteLine("Somma:" + somma);
        
        //prova col*scal
        Color mul = new Color();
        float a = 5;
        mul = mul.mult_Cs(green, a);
        Color mulb = green * a;
        Console.WriteLine("Moltiplicazione con scalare:" + mul);
        Console.WriteLine("Moltiplicazione con scalare (overloading*):" + mulb);
        
        //prova col*col
        Color mul2 = new Color();
        mul2 = mul2.mult_CC(green, red);
        Color mul2B = green * red;
        Console.WriteLine("Moltiplicazione con colore:" + mul2);
        Console.WriteLine("Moltiplicazione con colore (overloading*):" + mul2B);
        
        //prova are_close
        Color redB = new Color(254,0,0);
        if (Color.are_close(red, redB))
        {
            Console.WriteLine("I colori sono uguali");
        }
        else
        {
            Console.WriteLine("I colori sono diversi");
        }
        
        
        
        
    }
}
