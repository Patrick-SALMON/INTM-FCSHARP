using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Percolation
{
    /// <summary>
    /// Décrit une grille de percolation avec des méthodes pour la faire évoluer, la grille est initialisée avec toutes les cases bloquantes.
    /// </summary>
    public class Percolation
    {
        private readonly bool[,] _open;
        private readonly bool[,] _full;
        private readonly int _size;
        private bool _percolate;

        /// <summary>
        /// Définit une grille de percolation selon la taille en entrée.
        /// </summary>
        /// <param name="size">Taille de la grille de percolation.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Percolation(int size)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), size, "Taille de la grille négative ou nulle.");
            }

            _open = new bool[size, size];
            _full = new bool[size, size];
            _size = size;
            _percolate = false;
        }

        /// <summary>
        /// Détermine si une case de la grille est ouverte ou non
        /// </summary>
        /// <param name="i">Indice de ligne dans la grille.</param>
        /// <param name="j">Indice de colonne dans la grille.</param>
        /// <returns>True si la case est ouverte et False sinon.</returns>
        public bool IsOpen(int i, int j)
        {
            return _open[i,j];
        }

        /// <summary>
        /// Détermine si une case de la grille est pleine ou non
        /// </summary>
        /// <param name="i">Indice de ligne dans la grille.</param>
        /// <param name="j">Indice de colonne dans la grille.</param>
        /// <returns>True si la case est pleine et False sinon.</returns>
        private bool IsFull(int i, int j)
        {
            return _full[i,j];
        }

        /// <summary>
        /// Détermine si la grille est arrivée à percolation, c'est à dire qu'une case pleine se trouve à la dernière ligne de la grille.
        /// </summary>
        /// <returns>True si la grille percole, False sinon.</returns>
        public bool Percolate()
        {
            // Ancienne version à temps linéaire
            /*for (int j = 0; j < _size; j++)
            {
                if (_full[_size - 1, j]) 
                {
                    return true;
                }
            }*/
            return _percolate;
        }

        /// <summary>
        /// Trouve tous les voisins orthogonaux d'une case de la grille.
        /// </summary>
        /// <param name="i">Indice de ligne de la grille.</param>
        /// <param name="j">Indice de colonne de la grille.</param>
        /// <returns>Une liste de paires d'indices correspondant aux cases qui sont voisines de la case d'entrée.</returns>
        private List<KeyValuePair<int, int>> CloseNeighbors(int i, int j)
        {
            List<KeyValuePair<int, int>> voisins = new List<KeyValuePair<int, int>>();

            for (int iVoisin = i - 1; iVoisin <= i + 1; iVoisin++) 
            {
                if (iVoisin != i && iVoisin >= 0 && iVoisin < _size)
                {
                    voisins.Add(new KeyValuePair<int, int>(iVoisin, j));
                }
            }

            for (int jVoisin = j - 1; jVoisin <= j + 1; jVoisin++)
            {
                if (jVoisin != j && jVoisin >= 0 && jVoisin < _size)
                {
                    voisins.Add(new KeyValuePair<int, int>(i, jVoisin));
                }
            }

            return voisins;
        }

        // 3(b) Dans le pire des cas, la case qu'on veut ouvrir est la dernière avant que toute la grille soit inondée par des cases pleines et on doit traiter
        // tous les voisins jusqu'à ce que tout soit rempli par des cases pleines. Dans ce cas on aura une performance en O(N²) où N est la taille de la grille.
        // 3(c) Ce cas est très peu probable car on ouvre les cases de manière aléatoire, la chance d'avoir exactement le pire des cas est donc très basse.
        
        /// <summary>
        /// Ouvre une case de la grille et gère récursivement l'impact de cette ouverture sur le reste de la grille.
        /// </summary>
        /// <param name="i">Indice de ligne de la grille.</param>
        /// <param name="j">Indice de colonne de la grille.</param>
        public void Open(int i, int j)
        {
            // Si la case est déjà pleine il n'y a pas d'intérêt à la traiter (c'est aussi la condition d'arrêt de la récursivité).
            if (IsFull(i, j)) 
            {
               return;
            }

            _open[i, j] = true;

            List<KeyValuePair<int, int>> voisins = CloseNeighbors(i, j);

            // Les cases ouvertes de la première ligne deviennent automatiquement pleine.
            if (i == 0)
            {
                _full[i, j] = true;
            }
            else
            {
                foreach (KeyValuePair<int, int> voisin in voisins)
                {
                    int iVoisin = voisin.Key;
                    int jVoisin = voisin.Value;

                    // Si un voisin (sauf celui du bas) est plein alors la case est pleine.
                    if (IsFull(iVoisin, jVoisin) && iVoisin <= i)
                    {
                        _full[i, j] = true;
                    }
                }
            }

            // On a percolation quand une case de la dernière ligne est pleine.
            if (i == _size - 1 && IsFull(i, j)) 
            {
                _percolate = true;
            }

            if (IsFull(i, j))
            {
                foreach (KeyValuePair<int, int> voisin in voisins)
                {
                    int iVoisin = voisin.Key;
                    int jVoisin = voisin.Value;

                    // On répercute le changement seulement pour les voisins qui sont ouvert et qui ne sont pas au-dessus de nous.
                    if (IsOpen(iVoisin, jVoisin) && iVoisin >= i)
                    {
                        Open(iVoisin, jVoisin);
                    }
                }
            }
        }

        /// <summary>
        /// Affiche la grille de percolation avec '1' une case pleine, '0' une case ouverte non pleine, '#' une case bloquante et '?' une case dans
        /// un état qui devrait être impossible (une case pleine mais pas ouverte par exemple).
        /// </summary>
        public void DisplayPerco()
        {
            Console.WriteLine();
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (IsOpen(i,j) && IsFull(i,j))
                    {
                        Console.Write('1');
                    }
                    else if (IsOpen(i,j) && !IsFull(i,j))
                    {
                        Console.Write('0');
                    }
                    else if (!IsOpen(i,j) && !IsFull(i,j))
                    {
                        Console.Write('#');
                    }
                    else
                    {
                        Console.Write('?');
                    }
                }
                Console.Write('\n');
            }
        }
    }
}
