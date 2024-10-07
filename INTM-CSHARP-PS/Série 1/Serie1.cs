using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTM.Serie1
{
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
            char oddsymbol = '+';
            char toWrite;
            char evensymbol;
            string pyramidType;

            if (isSmooth)
            {
                evensymbol = '+';
                pyramidType = "lisse";
            }
            else
            {
                evensymbol = '-';
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
                    toWrite = evensymbol;
                }
                else
                {
                    toWrite = oddsymbol;
                }
                
                for (int bord = gauche; bord <= droite; bord++)
                {
                    Console.Write(toWrite);
                }

                Console.Write('\n');
            }
            Console.WriteLine();
        }

    }
}
