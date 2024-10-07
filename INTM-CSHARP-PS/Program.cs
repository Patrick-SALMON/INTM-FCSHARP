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

            // Keep the console window open
            Console.WriteLine("\n----------------------");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }


    }
}
