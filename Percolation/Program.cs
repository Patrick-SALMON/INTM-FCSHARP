using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Percolation
{
    class Program
    {
        static void Main()
        {
            int taille = 10;
            int essais = 100;

            PercolationSimulation percolationSimulation = new PercolationSimulation();

            PclData pclData = percolationSimulation.MeanPercolationValue(taille, essais);

            Console.WriteLine($"Pour une grille de taille {taille} avec {essais} essais, on a {pclData.Mean} de moyenne et {pclData.StandardDeviation} d'écart-type");

            // Keep the console window open
            Console.WriteLine("----------------------");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
