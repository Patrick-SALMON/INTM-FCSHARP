using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Labyrinth
{
    /// <summary>
    /// Représente un labyrinthe sous la forme d'un tableau en escalier de cellules.
    /// </summary>
    class Maze
    {
        /// <summary>
        /// Nombre de lignes de cellules
        /// </summary>
        public int Height { get; private set; }
        /// <summary>
        /// Nombre de colonnes de cellules
        /// </summary>
        public int Width { get; private set; }
        private readonly Cell[,] _maze;
        
        /// <summary>
        /// Initialise un labyrinthe dans les dimensions spécifiées. Toutes les parois de toutes les cellules sont initialement fermées.
        /// </summary>
        /// <param name="height">Hauteur du labyrinthe, son nombre de lignes.</param>
        /// <param name="width">Largeur du labyrinthe, son nombre de colonnes.</param>
        public Maze(int height, int width)
        {
            Height = height;
            Width = width;
            _maze = new Cell[Height, Width];
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    _maze[i, j] = new Cell();
                }
            }
        }

        /// <summary>
        /// Vérifie si une paroi d'une cellule existe ou non.
        /// </summary>
        /// <param name="i">Ligne où se trouve la cellule.</param>
        /// <param name="j">Colonne où se trouve la cellule.</param>
        /// <param name="w">Paroi de la cellule à vérifier.</param>
        /// <returns>True si la paroi existe et False sinon.</returns>
        private bool IsOpen(int i, int j, int w)
        {
            return !_maze[i, j][w];
        }

        /// <summary>
        /// Vérifie si une cellule est l'entrée du labyrinthe.
        /// </summary>
        /// <param name="i">Ligne où se trouve la cellule.</param>
        /// <param name="j">Colonne où se trouve la cellule.</param>
        /// <returns>True si la cellule est l'entrée et False sinon.</returns>
        private bool IsMazeStart(int i, int j)
        {
            if (_maze[i,j]._status == 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Vérifie si une cellule est la sortie du labyrinthe.
        /// </summary>
        /// <param name="i">Ligne où se trouve la cellule.</param>
        /// <param name="j">Colonne où se trouve la cellule.</param>
        /// <returns>True si la cellule est la sortie et False sinon.</returns>
        private bool IsMazeEnd(int i, int j)
        {
            if (_maze[i, j]._status == -1) 
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Ouvre une paroi d'une cellule et ouvre la paroi correspondante du voisin de la cellule.
        /// </summary>
        /// <param name="i">Ligne où se trouve la cellule.</param>
        /// <param name="j">Colonne où se trouve la cellule.</param>
        /// <param name="w">Paroi à ouvrir.</param>
        private void Open(int i, int j, int w)
        {
            List<KeyValuePair<int, int>> neighbors = CloseNeighbors(i, j);

            // Test pour la paroi du haut
            if (w == 0 && neighbors.Contains(new KeyValuePair<int, int>(i - 1, j)))
            {
                _maze[i, j][0] = false;
                _maze[i - 1, j][1] = false;
            }
            // Test pour la paroi du bas
            else if (w == 1 && neighbors.Contains(new KeyValuePair<int, int>(i + 1, j)))
            {
                _maze[i, j][1] = false;
                _maze[i + 1, j][0] = false;
            }
            // Test pour la paroi de gauche
            else if (w == 2 && neighbors.Contains(new KeyValuePair<int, int>(i, j - 1)))
            {
                _maze[i, j][2] = false;
                _maze[i, j - 1][3] = false;
            }
            // Test pour la paroi de droite
            else if (w == 3 && neighbors.Contains(new KeyValuePair<int, int>(i, j + 1)))
            {
                _maze[i, j][3] = false;
                _maze[i, j + 1][2] = false;
            }
            UpdateDisplay(i, j, w);
        }

        /// <summary>
        /// Trouve le côté où le voisin se trouve par rapport à la cellule en entrée et ouvre la paroi entre elle et le voisin.
        /// </summary>
        /// <param name="i">Ligne où se trouve la cellule.</param>
        /// <param name="j">Colonne où se trouve la cellule.</param>
        /// <param name="iNeighbor">Ligne où se trouve la cellule voisine.</param>
        /// <param name="jNeighbor">Colonne où se trouve la cellule voisine.</param>
        private void FindNeighborSideAndOpen(int i, int j, int iNeighbor, int jNeighbor)
        {
            if (iNeighbor == i - 1 && jNeighbor == j)
            {
                Open(i, j, 0);
            }
            else if (iNeighbor == i + 1 && jNeighbor == j)
            {
                Open(i, j, 1);
            }
            else if (iNeighbor == i && jNeighbor == j - 1)
            {
                Open(i, j, 2);
            }
            else if (iNeighbor == i && jNeighbor == j + 1)
            {
                Open(i, j, 3);
            }
            else
            {
                Console.WriteLine("Erreur ouverture");
            }
        }

        /// <summary>
        /// Obtient une liste de coordonnées correspondant aux voisins d'une cellule tout en considérant les bords du labyrinthe.
        /// </summary>
        /// <param name="i">Ligne où se trouve la cellule.</param>
        /// <param name="j">Colonne où se trouve la cellule.</param>
        /// <returns>Une liste de coordonnées correspondant à tout les voisins de la cellule d'entrée.</returns>
        private List<KeyValuePair<int, int>> CloseNeighbors(int i, int j)
        {
            List<KeyValuePair<int, int>> neighbors = new List<KeyValuePair<int, int>>();

            // Voisins en haut et en bas
            for (int iNeighbor = i - 1; iNeighbor <= i + 1; iNeighbor++)
            {
                if (iNeighbor != i && iNeighbor >= 0 && iNeighbor < Height)
                {
                    neighbors.Add(new KeyValuePair<int, int>(iNeighbor, j));
                }
            }

            // Voisins à gauche et à droite.
            for (int jNeighbor = j - 1; jNeighbor <= j + 1; jNeighbor++)
            {
                if (jNeighbor != j && jNeighbor >= 0 && jNeighbor < Width)
                {
                    neighbors.Add(new KeyValuePair<int, int>(i, jNeighbor));
                }
            }

            return neighbors;
        }

        // Generate parcourera nécessairement toutes les cases du labyrinthe car il s'arrête uniquement quand tous les voisins ont été visités
        /// <summary>
        /// Génère les chemins du labyrinthe en parcourant aléatoirement toutes les cellules.
        /// </summary>
        /// <returns>Les coordonnées du point d'entrée du labyrinthe.</returns>
        public KeyValuePair<int, int> Generate()
        {
            Random random = new Random();
            Stack<KeyValuePair<int, int>> cells = new Stack<KeyValuePair<int, int>>();

            int i = random.Next(0, Height);
            int j = random.Next(0, Width);
            int numberVisited = 0;
            bool backtracking = false;

            List<string> mazeLines = Display();
            for (int lines = 0; lines < mazeLines.Count; lines++)
            {
                Console.WriteLine(mazeLines[lines]);
            }

            cells.Push(new KeyValuePair<int, int>(i, j));

            while (cells.Count != 0 && numberVisited < Width * Height)
            {
                KeyValuePair<int,int> coordinates = cells.Pop();
                i = coordinates.Key;
                j = coordinates.Value;
                //Thread.Sleep(1);
                // On ne traite les coordonnées que si elles n'ont pas encore été visitées
                if (!_maze[i, j]._visited) 
                {
                    _maze[i, j]._visited = true;
                    numberVisited++;
                    // On obtient la liste des voisins pas encore visité
                    List<KeyValuePair<int, int>> neighbors = CloseNeighbors(i, j).Where(c => !_maze[c.Key, c.Value]._visited).ToList();

                    if (neighbors.Count > 0)
                    {
                        // Si on revient en arrière après avoir trouvé une impasse (une cellule sans voisin non visité), on ouvre
                        // une paroi avec un voisin visité (il en existe au moins un dans tous les cas) afin d'empêcher les ilôts de
                        // cellules
                        if (backtracking)
                        {
                            List<KeyValuePair<int, int>> neighs = CloseNeighbors(i, j).Where(c => _maze[c.Key, c.Value]._visited).ToList();
                            int iNeigh = neighs[0].Key;
                            int jNeigh = neighs[0].Value;

                            FindNeighborSideAndOpen(i, j, iNeigh, jNeigh);

                            backtracking = false;
                        }
                        int randomNeighbor = random.Next(0, neighbors.Count);

                        int iNeighbor = neighbors[randomNeighbor].Key;
                        int jNeighbor = neighbors[randomNeighbor].Value;

                        neighbors.Remove(neighbors[randomNeighbor]);

                        // Si il reste des voisins et qu'ils ne sont pas déjà dans la pile, on les rajoute.
                        for (int neigh = 0; neigh < neighbors.Count; neigh++)
                        {
                            if (!cells.Contains(new KeyValuePair<int, int>(neighbors[neigh].Key, neighbors[neigh].Value))) 
                            {
                                cells.Push(new KeyValuePair<int, int>(neighbors[neigh].Key, neighbors[neigh].Value));
                            }
                        }

                        cells.Push(new KeyValuePair<int, int>(iNeighbor, jNeighbor));

                        FindNeighborSideAndOpen(i, j, iNeighbor, jNeighbor);
                    }
                    // neighbors.Count == 0, c'est à dire qu'on se trouve dans une impasse.
                    else
                    {
                        backtracking = true;
                        // Si l'impasse est complètement emmurée on ouvre un de ces murs vers un voisin visité. 
                        if (_maze[i, j][0] && _maze[i, j][1] && _maze[i, j][2] && _maze[i, j][3])
                        {
                            List<KeyValuePair<int, int>> neighs = CloseNeighbors(i, j).Where(c => _maze[c.Key, c.Value]._visited).ToList();
                            int iNeigh = neighs[0].Key;
                            int jNeigh = neighs[0].Value;

                            FindNeighborSideAndOpen(i, j, iNeigh, jNeigh);
                        }
                    }
                }
            }

            int randomSide = random.Next(0, 4);
            i = random.Next(0, Height);
            j = random.Next(0, Width);

            // Détermination du point de sortie
            if (randomSide == 0)
            {
                _maze[0, j]._status = -1;
            }
            else if (randomSide == 1)
            {
                _maze[Height - 1, j]._status = -1;
            }
            else if (randomSide == 2)
            {
                _maze[i, 0]._status = -1;
            }
            else
            {
                _maze[i, Width - 1]._status = -1;
            }

            randomSide = random.Next(0, 4);
            i = random.Next(0, Height);
            j = random.Next(0, Width);

            // Détermination du point d'entrée en s'assurant qu'on ne choisit pas la même cellule que le point de sortie
            if (randomSide == 0)
            {
                while (_maze[0, j]._status == -1) 
                {
                    j = random.Next(0, Width);
                }
                _maze[0, j]._status = 1;
                return new KeyValuePair<int, int>(0, j);
            }
            else if (randomSide == 1)
            {
                while (_maze[Height - 1, j]._status == -1) 
                {
                    j = random.Next(0, Width);
                }
                _maze[Height - 1, j]._status = 1;
                return new KeyValuePair<int, int>(Height - 1, j);
            }
            else if (randomSide == 2)
            {
                while (_maze[i, 0]._status == -1) 
                {
                    i = random.Next(0, Height);
                }
                _maze[i, 0]._status = 1;
                return new KeyValuePair<int, int>(i, 0);
            }
            else
            {
                while (_maze[i, Width - 1]._status == -1)
                {
                    i = random.Next(0, Height);
                }
                _maze[i, Width - 1]._status = 1;
                return new KeyValuePair<int, int>(i, Width - 1);
            }
        }

        /// <summary>
        /// Traite une ligne d'intersections du labyrinthe pour trouver les caractères qui la représente.
        /// </summary>
        /// <param name="n">Numéro de ligne d'intersections à étudier.</param>
        /// <returns>Une chaîne de caractères contenant les caractères correspondant aux différentes intersections sur la ligne.</returns>
        private string DisplayLine(int n)
        {
            string mazeLine = "";
            int i = n;

            for (int j = 0; j <= Width; j++)
            {
                // On traite d'abord tous les cas de bordures avant de traiter le cas général.
                if (i == 0)
                {
                    if (j == 0)
                    {
                        mazeLine += "┌";
                    }
                    else if (j == Width)
                    {
                        mazeLine += "┐";
                    }
                    else
                    {
                        if (_maze[0, j][2])
                        {
                            mazeLine += "┬";
                        }
                        else
                        {
                            mazeLine += "─";
                        }
                    }
                }
                else if (i == Height)
                {
                    if (j == 0)
                    {
                        mazeLine += "└";
                    }
                    else if (j == Width)
                    {
                        mazeLine += "┘";
                    }
                    else
                    {
                        if (_maze[i - 1, j][2]) 
                        {
                            mazeLine += "┴";
                        }
                        else
                        {
                            mazeLine += "─";
                        }
                    }
                }
                else
                {
                    if (j == 0)
                    {
                        if (_maze[i, 0][0]) 
                        {
                            mazeLine += "├";
                        }
                        else
                        {
                            mazeLine += "│";
                        }
                    }
                    else if (j == Width) 
                    {
                        if (_maze[i, j - 1][0] && _maze[i - 1, j - 1][1]) 
                        {
                            mazeLine += "┤";
                        }
                        else
                        {
                            mazeLine += "│";
                        }
                    }
                    else
                    {
                        // Dans le cas général on détermine où sont les murs au point d'intersection et on associe un caractère
                        // selon la combinaison de booléens résultante.
                        bool up = _maze[i - 1, j][2] || _maze[i - 1, j - 1][3];
                        bool down = _maze[i, j][2] || _maze[i, j - 1][3];
                        bool left = _maze[i, j - 1][0] || _maze[i - 1, j - 1][1];
                        bool right = _maze[i, j][0] || _maze[i - 1, j][1];

                        if (up && down && left && right)
                        {
                            mazeLine += "┼";
                        }
                        else if (up && down && left && !right)
                        {
                            mazeLine += "┤";
                        }
                        else if (up && down && !left && right)
                        {
                            mazeLine += "├";
                        }
                        else if (up && !down && left && right)
                        {
                            mazeLine += "┴";
                        }
                        else if (!up && down && left && right)
                        {
                            mazeLine += "┬";
                        }
                        else if (up && down && !left && !right)
                        {
                            mazeLine += "│";
                        }
                        else if (up && !down && left && !right)
                        {
                            mazeLine += "┘";
                        }
                        else if (!up && down && left && !right)
                        {
                            mazeLine += "┐";
                        }
                        else if (up && !down && !left && right)
                        {
                            mazeLine += "└";
                        }
                        else if (!up && down && !left && right)
                        {
                            mazeLine += "┌";
                        }
                        else if (!up && !down && left && right)
                        {
                            mazeLine += "─";
                        }
                        else if (!up && !down && !left && right)
                        {
                            mazeLine += "-";
                        }
                        else if (!up && !down && left && !right)
                        {
                            mazeLine += "-";
                        }
                        else if (!up && down && !left && !right)
                        {
                            mazeLine += "¡";
                        }
                        else if (up && !down && !left && !right)
                        {
                            mazeLine += "'";
                        }
                        else if (!up && !down && !left && !right)
                        {
                            mazeLine += " ";
                        }
                        else
                        {
                            mazeLine += "?";
                        }
                    }
                }
            }
            return mazeLine;
        }

        /// <summary>
        /// Renvoie l'entiéreté du labyrinthe sous forme d'une liste de chaînes de caractères.
        /// </summary>
        /// <returns>Une liste de chaînes de caractères correspondant au labyrinthe.</returns>
        public List<string> Display()
        {
            List<string> mazeLines = new List<string>();
            for (int i = 0; i <= Height; i++)
            {
                mazeLines.Add(DisplayLine(i));
            }
            return mazeLines;
        }

        /// <summary>
        /// Change les caractères concernés par une ouverture de paroi dans le labyrinthe.
        /// </summary>
        /// <param name="i">Ligne où se trouve la cellule.</param>
        /// <param name="j">Colonne où se trouve la cellule.</param>
        /// <param name="w">Paroi que l'on vient d'ouvrir.</param>
        private void UpdateDisplay(int i, int j, int w)
        {
            string mazeLine;
            // Paroi du haut
            if (w == 0)
            {
                mazeLine = DisplayLine(i);
                Console.SetCursorPosition(j, i);
                Console.Write(mazeLine[j]);
                Console.Write(mazeLine[j + 1]);
            }
            // Paroi du bas
            else if (w == 1)
            {
                mazeLine = DisplayLine(i + 1);
                Console.SetCursorPosition(j, i + 1);
                Console.Write(mazeLine[j]);
                Console.Write(mazeLine[j + 1]);
            }
            // Paroi de gauche
            else if (w == 2)
            {
                mazeLine = DisplayLine(i);
                Console.SetCursorPosition(j, i);
                Console.Write(mazeLine[j]);
                mazeLine = DisplayLine(i + 1);
                Console.SetCursorPosition(j, i + 1);
                Console.Write(mazeLine[j]);
            }
            // Paroi de droite
            else if (w == 3)
            {
                mazeLine = DisplayLine(i);
                Console.SetCursorPosition(j + 1, i);
                Console.Write(mazeLine[j + 1]);
                mazeLine = DisplayLine(i + 1);
                Console.SetCursorPosition(j + 1, i + 1);
                Console.Write(mazeLine[j + 1]);
            }
            // Repositionnement du curseur pour ne pas afficher du texte dans le labyrinthe
            Console.SetCursorPosition(0, Height + 1);
        }
    }
}
