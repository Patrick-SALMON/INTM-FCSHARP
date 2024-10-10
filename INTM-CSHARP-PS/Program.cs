using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTM.Serie1;
using INTM.Serie2;
using INTM.Serie4;

namespace INTM_CSHARP_PS
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Console.WriteLine("|--------------------------------------|");
            Console.WriteLine("|              Série 1                 |");
            Console.WriteLine("|--------------------------------------|\n");

            Console.WriteLine("\n|--------------------------------------|");
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
            */
            /*
            Console.WriteLine("|--------------------------------------|");
            Console.WriteLine("|              Série 2                 |");
            Console.WriteLine("|--------------------------------------|\n");

            Console.WriteLine("\n|--------------------------------------|");
            Console.WriteLine("| Exercice I - Recherche d'un élément  |");
            Console.WriteLine("|--------------------------------------|\n");

            int[] tab = { 1, -5, 10, 3, 0, 4, 2, -7 };
            int[] tabtrie = { -7, -5, 0, 1, 2, 3, 4, 10 };

            Console.WriteLine("Recherche linéaire: ");
            Console.WriteLine($"L'élément 2 du tableau est à l'indice {Serie2.LinearSearch(tab, 2)}");
            Console.WriteLine($"L'élément -8 du tableau est à l'indice {Serie2.LinearSearch(tab, -8)}");

            Console.WriteLine("\nRecherche dichotomique: ");
            Console.WriteLine($"L'élément 2 du tableau est à l'indice {Serie2.BinarySearch(tabtrie, 2)}");
            Console.WriteLine($"L'élément -8 du tableau est à l'indice {Serie2.BinarySearch(tabtrie, -8)}");
            
            Console.WriteLine("\n|------------------------------------------|");
            Console.WriteLine("| Exercice II - Bases du calcul matriciel  |");
            Console.WriteLine("|------------------------------------------|\n");

            int[] gauche = { 1, 2, 3 };
            int[] droite = { -1, -4, 0 };
            int[][] matrice = Serie2.BuildingMatrix(gauche, droite);

            Console.WriteLine("Matrice construite à partir de deux vecteurs: ");
            Serie2.DisplayMatrix(matrice);

            int[][] mGaucheAdd = new int[3][] { new int[2] { 1, 2 }, new int[2] { 4, 6 }, new int[2] { -1, 8 } };
            int[][] mDroiteAdd = new int[3][] { new int[2] { -1, 5 }, new int[2] { -4, 0 }, new int[2] { 0, 2 } };

            matrice = Serie2.Addition(mGaucheAdd, mDroiteAdd);

            Console.WriteLine("\nAddition de deux matrices: ");
            Serie2.DisplayMatrix(matrice);

            int[][] mGaucheMult = new int[3][] { new int[2] { 1, 2 }, new int[2] { 4, 6 }, new int[2] { -1, 8 } };
            int[][] mDroiteMult = new int[2][] { new int[3] { -1, 5, 0 }, new int[3] { -4, 0, 1 } };

            matrice = Serie2.Multiplication(mGaucheMult, mDroiteMult);

            Console.WriteLine("\nMultiplication de deux matrices: ");
            Serie2.DisplayMatrix(matrice);

            Console.WriteLine("\n|------------------------------------------|");
            Console.WriteLine("| Exercice III - Crible d'Erastothène      |");
            Console.WriteLine("|------------------------------------------|\n");

            int[] tabPremier = Serie2.EratosthenesSieve(100);

            Console.WriteLine("100 premiers nombres premiers d'après le crible d'Eratosthène:\n ");

            Console.Write("[ ");
            for (int i = 0; i < tabPremier.Length; i++)
            {
                Console.Write(tabPremier[i] + " ");
            }
            Console.Write("]\n");

            Console.WriteLine("\n|----------------------------------------------|");
            Console.WriteLine("| Exercice IV - Questionnaire à choix multiple |");
            Console.WriteLine("|----------------------------------------------|\n");

            Serie2.Qcm q1 = new Serie2.Qcm("Quand est l'année du sacre de Charlemagne ?", 
                                           new string[] { "476", "800", "1066", "1789" }, 1, 1);
            int score = -1;

            try
            {
                score = Serie2.AskQuestion(q1);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine(ae.ToString());
            }

            if (score == 0)
            {
                Console.WriteLine("Réponse fausse\n");
            }
            else if (score > 0)
            {
                Console.WriteLine("Bonne réponse !\n");
            }

            Serie2.Qcm q2 = new Serie2.Qcm("Quel est le nom du président de la République en 2021 ?",
                               new string[] { "Chirac", "De Gaulle", "Macron" }, 2, 1);
            Serie2.Qcm q3 = new Serie2.Qcm("Quelle est la durée de vie moyenne d'une naine jaune (en milliards d'années) ?",
                               new string[] { "1", "10", "1 000", "1 000 000" }, 1, 1);

            Serie2.Qcm[] qTab = new Serie2.Qcm[] { q1, q2, q3 };

            try
            {
                Serie2.AskQuestions(qTab);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine(ae.ToString());
            }
            */

            Console.WriteLine("|--------------------------------------|");
            Console.WriteLine("|              Série 4                 |");
            Console.WriteLine("|--------------------------------------|\n");

            Console.WriteLine("\n|--------------------------------------|");
            Console.WriteLine("| Exercice I - Code Morse              |");
            Console.WriteLine("|--------------------------------------|\n");

            string morse = "===.=.===.=...===.===.===...===.=.=...=.....===.===...===.===.===...=.===.=...=.=.=...=";
            string morseErr = "=...=.===.=...==...===.===.===...=.===.=";

            Console.WriteLine($"Le code morse suivant a {Serie4.LettersCount(morse)} lettres");
            Console.WriteLine(morse);

            Console.WriteLine($"\nLe code morse suivant a {Serie4.WordsCount(morse)} mots");
            Console.WriteLine(morse);

            Console.WriteLine("\nTraduction du code morse :\n");
            try
            {
                Console.WriteLine($"{morse} : {Serie4.MorseTranslation(morse)}\n");
                Console.WriteLine($"{morseErr} : {Serie4.MorseTranslation(morseErr)}");
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine(ae.ToString());
            }

            // Keep the console window open
            Console.WriteLine("\n----------------------");
            Console.WriteLine("Appuyer sur une touche pour quitter.");
            Console.ReadKey();
        }


    }
}
