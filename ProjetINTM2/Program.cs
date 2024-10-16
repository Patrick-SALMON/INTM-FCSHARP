using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProjetINTM2
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Banque banque = new Banque("../../Gestionnaires.csv");
                banque.TraiterOperations("../../Comptes.csv", "../../Transactions.csv", "../../StatutComptes.csv", "../../StatutTransactions.csv");
                banque.ImprimerCompteurs("../../Statistiques.txt");
            }
            catch (FileNotFoundException fe)
            {
                Console.WriteLine(fe.ToString());
            }

            // Garder la console ouverte
            Console.WriteLine("\n----------------------");
            Console.WriteLine("Appuyer sur une touche pour quitter.");
            Console.ReadKey();
        }
    }
}
