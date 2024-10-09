using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTM.Serie2
{
    class Serie2
    {
        // Exercice I - Recherche d'un élément

        // Dans le pire des cas, la méthode linéaire doit lire tous les éléments du tableau
        public static int LinearSearch(int[] tableau, int valeur)
        {
            if (tableau.Length == 0)
            {
                return -1;
            }

            for (int i = 0; i < tableau.Length; i++) 
            {
                if (tableau[i] == valeur)
                {
                    return i;
                }
            }
            return -1;
        }

        // Dans le pire des cas, la méthode dichotomique doit lire environ log(N) éléments avec N la taille du tableau
        public static int BinarySearch(int[] tableau, int valeur)
        {
            if (tableau.Length == 0) 
            {
                return -1;
            }

            int milieu;
            int gauche = 0;
            int droite = tableau.Length - 1;

            while (gauche <= droite)
            {
                milieu = (gauche + droite) / 2;

                if (valeur == tableau[milieu])
                {
                    return milieu;
                }
                else if (valeur > tableau[milieu] )
                {
                    // + 1 car on sait que milieu n'est pas la bonne valeur et pour la condtion d'arrêt
                    gauche = milieu + 1;
                }
                else
                {
                    // - 1 car on sait que milieu n'est pas la bonne valeur et pour la condition d'arrêt
                    droite = milieu - 1;
                }
            }
            return -1;
        }

        // Exercice II - Bases du calcul matriciel

        /// <summary>
        /// Construit une matrice d'après deux vecteurs de la même taille en prenant la transposée du premier et en la multipliant par le deuxième.
        /// </summary>
        /// <param name="leftVector"> Vecteur de gauche</param>
        /// <param name="rightVector"> Vecteur de droite</param>
        /// <returns>Une matrice vide si les vecteurs ne sont pas de la même taille ou une matrice carrée si ils le sont.</returns>
        public static int[][] BuildingMatrix(int[] leftVector, int[] rightVector)
        {
            if (leftVector.Length != rightVector.Length)
            {
                return new int[0][];
            }

            int mLineLength = leftVector.Length;
            int mColLength = rightVector.Length;
            int[][] mRes = new int[mLineLength][];

            for (int i = 0; i < mLineLength; i++)
            {
                mRes[i] = new int[mColLength];
                for (int j = 0; j < mColLength; j++)
                {
                    mRes[i][j] = leftVector[i] * rightVector[j];
                }
            }
            return mRes;
        }

        /// <summary>
        /// Affiche une matrice reçu en entrée
        /// </summary>
        /// <param name="matrix">Matrice à afficher.</param>
        public static void DisplayMatrix(int[][] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                Console.Write("( ");
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    Console.Write(matrix[i][j] + " ");
                }
                Console.Write(")\n");
            }
        }

        /// <summary>
        /// Réalise l'addition de deux matrices de même taille.
        /// </summary>
        /// <param name="leftMatrix">Matrice de gauche.</param>
        /// <param name="rightMatrix">Matrice de droite.</param>
        /// <returns>Une matrice résultat correspondant à l'addition des deux matrices d'entrée ou une matrice vide si les dimensions sont incorrectes.</returns>
        public static int[][] Addition(int[][] leftMatrix, int[][] rightMatrix)
        {
            if (leftMatrix.Length != rightMatrix.Length || leftMatrix[0].Length != rightMatrix[0].Length)
            {
                return new int[0][];
            }

            int mLineLength = rightMatrix.Length;
            int mColLength = rightMatrix[0].Length;
            int[][] mRes = new int[mLineLength][];

            for (int i = 0; i < mLineLength; i++)
            {
                mRes[i] = new int[mColLength];
                for (int j = 0; j < mColLength; j++)
                {
                    mRes[i][j] = leftMatrix[i][j] + rightMatrix[i][j];
                }
            }

            return mRes;
        }

        /// <summary>
        /// Réalise la soustraction de deux matrices de même taille.
        /// </summary>
        /// <param name="leftMatrix">Matrice de gauche.</param>
        /// <param name="rightMatrix">Matrice de droite.</param>
        /// <returns>Une matrice résultat correspondant à la soustraction de la matrice de gauche moins celle de droite ou une matrice vide si les dimensions sont incorrectes.</returns>
        public static int[][] Substraction(int[][] leftMatrix, int[][] rightMatrix)
        {
            if (leftMatrix.Length != rightMatrix.Length || leftMatrix[0].Length != rightMatrix[0].Length)
            {
                return new int[0][];
            }

            int mLineLength = rightMatrix.Length;
            int mColLength = rightMatrix[0].Length;
            int[][] mRes = new int[mLineLength][];

            for (int i = 0; i < mLineLength; i++)
            {
                mRes[i] = new int[mColLength];
                for (int j = 0; j < mColLength; j++)
                {
                    mRes[i][j] = leftMatrix[i][j] - rightMatrix[i][j];
                }
            }

            return mRes;
        }

        /// <summary>
        /// Réalise la multiplication de deux matrices.
        /// </summary>
        /// <param name="leftMatrix">Matrice de gauche.</param>
        /// <param name="rightMatrix">Matrice de droite</param>
        /// <returns>Une matrice résultat carré correspondant à la multiplication des deux matrices d'entrée ou une matrice vide si les dimensions ne sont pas exactes.</returns>
        public static int[][] Multiplication(int[][] leftMatrix, int[][] rightMatrix)
        {
            if (leftMatrix[0].Length != rightMatrix.Length)
            {
                return new int[0][];
            }

            int longueurCommune = rightMatrix.Length;
            int mLineLength = leftMatrix.Length;
            int mColLength = rightMatrix[0].Length;
            int[][] mRes = new int[mLineLength][];

            for (int i = 0; i < mLineLength; i++)
            {
                mRes[i] = new int[mColLength];
                for (int j = 0; j < mColLength; j++)
                {
                    int resSum = 0;
                    for (int k = 0; k < longueurCommune; k++)
                    {
                        resSum += leftMatrix[i][k] * rightMatrix[k][j];
                    }
                    mRes[i][j] = resSum;
                }

            }
            return mRes;
        }

        // Exercice III - Crible d'Eratosthène

        /// <summary>
        /// Détermine la liste des n premiers nombres premiers d'après le crible d'Eratosthène.
        /// </summary>
        /// <param name="n"> Entier maximum.</param>
        /// <returns>Une table d'entiers contenant tous les nombres premiers jusqu'à n.</returns>
        public static int[] EratosthenesSieve(int n)
        {
            // Initialisation
            bool[] premier = new bool[n + 1];
            int compteurNombrePremiers = 0;

            for (int i = 2; i < premier.Length; i++)
            {
                premier[i] = true;
            }

            int entier = 2;

            // Traitement du crible d'Eratosthène
            while (entier <= n) 
            {
                if (premier[entier]) 
                {
                    compteurNombrePremiers++;
                    for (int j = entier * entier; j < n + 1; j += entier)  
                    {
                        premier[j] = false;
                    }
                }
                entier++;
            }

            // Retranscription en tableau d'entiers
            int[] tabRes = new int[compteurNombrePremiers];
            compteurNombrePremiers = 0;

            for (int i = 0; i < premier.Length; i++)
            {
                if (premier[i])
                {
                    tabRes[compteurNombrePremiers] = i;
                    compteurNombrePremiers++;
                }
            }

            return tabRes;

        }

        // Exercice IV - Questionnaire à choix multiple

        // J'ai choisi cette structure car elle correspond aux données de l'énoncé.
        /// <summary>
        /// Contient une question d'un QCM avec les différentes réponses possibles, la solution et le poids de la question.
        /// </summary>
        public struct Qcm
        {
            public string Question;
            public string[] Answers;
            public int Solution;
            public int Weight;

            /// <summary>
            /// Constructeur de Qcm
            /// </summary>
            /// <param name="question">Question à poser.</param>
            /// <param name="answers">Réponses possibles.</param>
            /// <param name="solution">Indice de la bonne réponse.</param>
            /// <param name="weight">Poids de la question.</param>
            public Qcm(string question, string[] answers, int solution, int weight)
            {
                Question = question;
                Answers = answers;
                Solution = solution;
                Weight = weight;
            }
        }

        /// <summary>
        /// Vérifie qu'une structure Qcm est valide, c'est-à-dire que la solution pointe bien sur une réponse et le poids est strictement positif.
        /// </summary>
        /// <param name="qcm">Structure Qcm à vérifier</param>
        /// <returns>False si la structure n'est pas valide et True sinon.</returns>
        public static bool QcmValidity(Qcm qcm)
        {
            if (qcm.Question == null || qcm.Answers == null)
            {
                return false;
            }

            if (qcm.Solution < 0 || qcm.Solution >= qcm.Answers.Length)
            {
                return false;
            }

            if (qcm.Weight <= 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Pose une question donnée dans la structure qcm à l'utilisateur.
        /// </summary>
        /// <param name="qcm">Structure contenant la question à poser, la réponse et le poids de la question.</param>
        /// <returns>0 si la réponse est fausse et le poids associé à la question si elle est juste.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static int AskQuestion(Qcm qcm)
        {
            if (!QcmValidity(qcm))
            {
                throw new ArgumentException("Le qcm en entrée est invalide", "qcm");
            }

            Console.WriteLine(qcm.Question);

            for (int i = 0; i < qcm.Answers.Length; i++)
            {
                Console.Write($"{i + 1}. {qcm.Answers[i]}   ");
            }
            Console.WriteLine();

            string input = "";
            int reponse = -1;

            while (!int.TryParse(input, out reponse) || reponse <= 0 || reponse > qcm.Answers.Length) 
            {
                Console.Write("Réponse : ");
                input = Console.ReadLine();

                if (!int.TryParse(input, out reponse) || reponse <= 0 || reponse > qcm.Answers.Length )

                {
                    Console.WriteLine("Réponse invalide !");
                }
            }

            if (reponse == qcm.Solution + 1)
            {
                return qcm.Weight;
            }

            return 0;
        }

        public static void AskQuestions(Qcm[] qcms)
        {
            int scoreTotal = 0;
            int scoreCandidat = 0;
            Console.WriteLine("Questionnaire :");

            foreach (Qcm qcm in qcms)
            {
                scoreCandidat += AskQuestion(qcm);
                scoreTotal += qcm.Weight;
            }
            Console.WriteLine($"Résultats du questionnaire : {scoreCandidat} / {scoreTotal}");
        }

    }
}
