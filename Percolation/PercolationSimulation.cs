using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Percolation
{
    public struct PclData
    {
        /// <summary>
        /// Moyenne 
        /// </summary>
        public double Mean { get; set; }
        /// <summary>
        /// Ecart-type
        /// </summary>
        public double StandardDeviation { get; set; }
        /// <summary>
        /// Fraction
        /// </summary>
        public double Fraction { get; set; }
    }

    /// <summary>
    /// Simulateur de percolation avec des méthodes permettant l'estimation du nombre moyen de cases à ouvrir pour obtenir percolation.
    /// </summary>
    public class PercolationSimulation
    {
        /// <summary>
        /// Réalise une série d'ouverture jusqu'à percolation sur des grilles de même taille puis calcule la moyenne et l'écart-type
        /// des proportions de cases ouvertes lors de la percolation.
        /// </summary>
        /// <param name="size">Taille de la grille de percolation.</param>
        /// <param name="t">Nombre d'ouvertures jusqu'à percolation à réaliser.</param>
        /// <returns>Une structure de donnée contenant la moyenne et l'écart-type obtenu.</returns>
        public PclData MeanPercolationValue(int size, int t)
        {
            PclData pclData = new PclData();
            double moyenne = 0;
            double ecartType = 0;

            for (int i = 0; i < t; i++)
            {
                double percoVal = PercolationValue(size);
                moyenne += percoVal;
                ecartType += percoVal * percoVal;
            }

            moyenne /= t;
            ecartType = Math.Sqrt((ecartType / t) - moyenne * moyenne);

            pclData.Mean = moyenne;
            pclData.StandardDeviation = ecartType;

            return pclData;
        }

        /// <summary>
        /// Ouvre aléatoirement les cases d'une grille de percolation jusqu'à arriver à une grille percolée puis calcule la proportion de cases ouvertes.
        /// </summary>
        /// <param name="size">Taile de la grille de percolation.</param>
        /// <returns>Un nombre à virgule flottante correspondant à la proportion de cases ouvertes dans la grille.</returns>
        public double PercolationValue(int size)
        {
            Percolation percolation = new Percolation(size);
            Random random = new Random();
            int casesOuvertes = 0;
            List<KeyValuePair<int, int>> casesDejaOuvertes = new List<KeyValuePair<int, int>>();

            while (percolation.Percolate() == false)
            {
                // Entier aléatoire entre 0 et size - 1.
                int iRandom = random.Next(0, size);
                int jRandom = random.Next(0, size);

                // On s'assure qu'on ouvre une case qui n'est pas déjà ouverte.
                if (casesDejaOuvertes.Contains(new KeyValuePair<int, int>(iRandom, jRandom)))  
                {
                    continue;
                }
                else
                {
                    casesDejaOuvertes.Add(new KeyValuePair<int, int>(iRandom, jRandom));
                }

                percolation.Open(iRandom, jRandom);
                casesOuvertes++;
            }
            // On cast en double car tous les éléments de cette équation sont des int.
            return (double)casesOuvertes / (size * size);
        }
    }
}
