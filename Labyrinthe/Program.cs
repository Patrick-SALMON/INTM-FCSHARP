using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth
{
    class Program
    {
        static void Main(string[] args)
        {
            Maze maze = new Maze(30,150);
            maze.Generate();

            // Keep the console window open
            Console.WriteLine("----------------------");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
