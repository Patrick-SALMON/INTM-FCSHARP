using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProjetINTM3
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Banque banque = new Banque("../../Test/Interets/Gestionnaires_1_Livret.txt");
                banque.TraiterOperations("../../Test/Interets/Comptes_1_Livret.txt", "../../Test/Interets/Transactions_1_Livret.txt", "../../StatutComptes.csv", "../../StatutTransactions.csv");
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
