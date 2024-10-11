using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTM.Serie1
{
    // J'ai pas à dire grand chose, le code est bien présenté, indenté correctement. 
    // Il remplit de ce que j'ai contrôlé aux consignes des exercices.
    public static class Serie1
    {
        // Exercice I - Opérations élémentaires
        public static void BasicOperation(int a, int b, char ope)
        {
            // res contiendra soit le résultat de l'opération soit "Opération Invalide" si elle ne peut pas être faite
            string res;

            if (ope == '+')
            {
                res = (a + b).ToString();
            }
            else if (ope == '-')
            {
                res = (a - b).ToString();
            }
            else if (ope == '*')
            {
                res = (a * b).ToString();
            }
            else if (ope == '/')
            {
                // On ne peut pas diviser par 0
                if (b == 0)
                {
                    res = "Opération invalide.";
                }
                else
                {
                    res = (a / b).ToString();
                }
            }
            // Le code ope n'est pas +,-,* ou /
            else
            {
                res = "Opération invalide.";
            }

            // On écrit l'opération et son résultat
            Console.WriteLine($"{a} {ope} {b} = {res}");
        }

        public static void IntegerDivision(int a, int b)
        {
            // On remonte immédiatement si b est 0
            if (b == 0)
            {
                Console.WriteLine($"{a} : {b} = Opération invalide.");
                return;
            }
            // q étant un entier, cette division renvoie la partie entière c'est à dire le quotient
            int q = a / b;
            // % est l'opérateur qui donne le reste
            int r = a % b;
            if (r == 0)
            {
                Console.WriteLine($"{a} = {q} * {b}");
            }
            else
            {
                Console.WriteLine($"{a} = {q} * {b} + {r}");
            }
        }

        public static void Pow(int a, int b)
        {
            if (b < 0)
            {
                Console.WriteLine($"{a} ^ {b} = Opération invalide.");
            }
            else
            {
                Console.WriteLine($"{a} ^ {b} = {Math.Pow(a, b)}");
            }
        }

        // Exercice II - Horloge Parlante

        public static string GoodDay(int heure)
        {
            string mess;

            if (heure < 0 || heure > 23)
            {
                return $"Il est {heure}H. Ce n'est pas une heure valide";
            }

            if (heure >= 0 && heure < 6)
            {
                mess = "Merveilleuse nuit!";
            }
            else if (heure >= 6 && heure < 12)
            {
                mess = "Bonne matinée!";
            }
            else if (heure == 12)
            {
                mess = "Bon appétit!";
            }
            else if (heure > 12 && heure <= 18)
            {
                mess = "Profitez de votre après-midi!";
            }
            else
            {
                mess = "Passez une bonne soirée!";
            }

            return $"Il est {heure}H. {mess}";
        }

        // Exercice III - Construction d'une pyramide
        // Pour un niveau j entre 1 et N, le nombre de blocs sera 2*j - 1
        // Le nombre total de blocs au niveau N est N²
        // Le sommet se trouve à la position N
        // gauche(j) = N - j + 1 et droite(j) = N + j - 1

        public static void PyramidConstruction(int n, bool isSmooth)
        {
            int gauche;
            int droite;
            char symboleImpair = '+';
            char aEcrire;
            char symbolePair;
            string pyramidType;

            if (isSmooth)
            {
                symbolePair = '+';
                pyramidType = "lisse";
            }
            else
            {
                symbolePair = '-';
                pyramidType = "strillée";
            }

            Console.WriteLine($"Pyramide de taille {n} {pyramidType} ci-dessous: \n");

            for (int niv = 0; niv < n; niv++)
            {
                gauche = n - niv;
                droite = n + niv;
                Console.SetCursorPosition(gauche - 1, Console.CursorTop);

                if ((niv + 1) % 2 == 0)
                {
                    aEcrire = symbolePair;
                }
                else
                {
                    aEcrire = symboleImpair;
                }
                
                for (int bord = gauche; bord <= droite; bord++)
                {
                    Console.Write(aEcrire);
                }

                Console.Write('\n');
            }
            Console.WriteLine();
        }
        // Exercice IV - Factorielle

        public static int IterFactorielle(int n)
        {
            if (n == 0)
            {
                return 1;
            }

            int res = 1;

            for (int i = 1; i < n + 1; i++) 
            {
                res *= i;
            }

            return res;
        }

        public static int RecurFactorielle(int n)
        {
            if (n == 0)
            {
                return 1;
            }

            return n * RecurFactorielle(n - 1);
        }

        // Exercice V - Les nombres premiers

        public static bool IsPrime(int valeur)
        {
            if (valeur == 2)
            {
                return true;
            }

            if (valeur % 2 == 0)
            {
                return false;
            }

            int limite = (int)Math.Sqrt(valeur) + 1;

            for (int i = 3; i < limite; i += 2)
            {
                if (valeur % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static void DisplayPrimes()
        {
            Console.WriteLine("Liste des 100 premiers nombres premiers: \n");
            Console.Write("[ ");

            for (int i = 2; i < 101; i++)
            {
                if (IsPrime(i))
                {
                    Console.Write(i + " ");
                }
            }
            Console.Write("]\n");
        }

        // Exercice VI - Algorithme d'Euclide

        public static int Gcd(int a, int b)
        {
            if (b == 0)
            {
                return a;
            }

            int r = a % b;

            return Gcd(b, r);
        }

    }
}
