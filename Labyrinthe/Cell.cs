using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth
{
    /// <summary>
    /// Représente une cellule dans un labyrinthe.
    /// </summary>
    class Cell
    {
        public bool[] _walls;
        public bool _visited;
        // Pour représenter le statut on utilise un entier qui vaudra soit 0 pour une cellule normale, 1 pour l'entrée et -1 pour la sortie.
        // On choisit un entier car c'est la façon la moins gourmande en resources de représenter trois états possibles.
        public int _status;

        /// <summary>
        /// Création d'une nouvelle cellule.
        /// </summary>
        public Cell()
        {
            // Dans l'ordre, Haut Bas Gauche Droite.
            _walls = new bool[4] { true, true, true, true };
            _visited = false;
            _status = 0;
        }

        /// <summary>
        /// Accède à l'état d'une des parois de la cellule.
        /// </summary>
        /// <param name="i">Entier indicant la paroi sur laquelle opérer 0 pour haut, 1 pour bas, 2 pour gauche et 3 pour droite</param>
        /// <returns>True si un mur est présent et False sinon.</returns>
        public bool this[int i]
        {
            get => _walls[i];
            set => _walls[i] = value;
        }
    }
}
