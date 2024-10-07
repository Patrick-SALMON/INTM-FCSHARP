using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTM.Serie1;

namespace INTM_CSHARP_PS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("|--------------------------------------|");
            Console.WriteLine("| Exercice I - Opérations élémentaires |");
            Console.WriteLine("|--------------------------------------|\n");

            Serie1.BasicOperation(3, 4, '+');
            Serie1.BasicOperation(6, 2, '/');
            Serie1.BasicOperation(3, 0, '/');
            Serie1.BasicOperation(6, 9, 'L');

            Console.WriteLine();

            Serie1.IntegerDivision(12, -4);
            Serie1.IntegerDivision(13, -4);
            Serie1.IntegerDivision(12, 0);

            Console.WriteLine();

            Serie1.Pow(3, 4);
            Serie1.Pow(2, 0);
            Serie1.Pow(6, -2);

            Console.WriteLine("\n|--------------------------------------|");
            Console.WriteLine("| Exercice II - Horloge parlante       |");
            Console.WriteLine("|--------------------------------------|\n");

            Console.WriteLine(Serie1.GoodDay(DateTime.Now.Hour));

            Console.WriteLine("\n|--------------------------------------------|");
            Console.WriteLine("| Exercice III - Construction d'une pyramide |");
            Console.WriteLine("|--------------------------------------------|\n");

            Serie1.PyramidConstruction(10, true);
            Serie1.PyramidConstruction(10, false);
            Serie1.PyramidConstruction(0, true);

            Console.WriteLine("\n|-------------------------------------|");
            Console.WriteLine("| Exercice IV - Factorielle           |");
            Console.WriteLine("|-------------------------------------|\n");

            Console.WriteLine("5! = " + Serie1.IterFactorielle(5));
            Console.WriteLine("5! = " + Serie1.RecurFactorielle(5));

            Console.WriteLine("\n|-------------------------------------|");
            Console.WriteLine("| Exercice V - Les nombres premiers   |");
            Console.WriteLine("|-------------------------------------|\n");

            Serie1.DisplayPrimes();

            Console.WriteLine("\n|-------------------------------------|");
            Console.WriteLine("| Exercice VI - Algorithme d'Euclide  |");
            Console.WriteLine("|-------------------------------------|\n");

            string input;
            int a, b;
            do
            {
                Console.WriteLine("Saisir le premier nombre :");
                input = Console.ReadLine();

            } while (!int.TryParse(input, out a));
            do
            {
                Console.WriteLine("Saisir le second nombre :");
                input = Console.ReadLine();

            } while (!int.TryParse(input, out b));

            Console.WriteLine($"\nPGCD de {a} et {b} : {Serie1.Gcd(a, b)}");

            // Keep the console window open
            Console.WriteLine("\n----------------------");
            Console.WriteLine("Appuyer sur une touche pour quitter.");
            Console.ReadKey();
        }


    }
}
