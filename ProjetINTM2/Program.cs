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
                Banque banque = new Banque("../../Test/Gestionnaires_3.txt");
                banque.TraiterOperations("../../Test/Comptes_3.txt", "../../Test/Transactions_3.txt", "../../StatutComptes.csv", "../../StatutTransactions.csv");
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
