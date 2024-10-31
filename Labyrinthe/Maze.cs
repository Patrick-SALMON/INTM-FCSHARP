using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Labyrinth
{
    class Maze
    {
        // Nombre de lignes de celulles
        public int Height { get; private set; }
        // Nombre de colonnes de cellules
        public int Width { get; private set; }
        private readonly Cell[,] _maze;
        
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

        private bool IsOpen(int i, int j, int w)
        {
            return !_maze[i, j][w];
        }

        private bool IsMazeStart(int i, int j)
        {
            if (_maze[i,j]._status == 1)
            {
                return true;
            }
            return false;
        }

        private bool IsMazeEnd(int i, int j)
        {
            if (_maze[i, j]._status == -1) 
            {
                return true;
            }
            return false;
        }

        private void Open(int i, int j, int w)
        {
            List<KeyValuePair<int, int>> neighbors = CloseNeighbors(i, j);

            if (w == 0 && neighbors.Contains(new KeyValuePair<int, int>(i - 1, j)))
            {
                _maze[i, j][0] = false;
                _maze[i - 1, j][1] = false;
            }
            else if (w == 1 && neighbors.Contains(new KeyValuePair<int, int>(i + 1, j)))
            {
                _maze[i, j][1] = false;
                _maze[i + 1, j][0] = false;
            }
            else if (w == 2 && neighbors.Contains(new KeyValuePair<int, int>(i, j - 1)))
            {
                _maze[i, j][2] = false;
                _maze[i, j - 1][3] = false;
            }
            else if (w == 3 && neighbors.Contains(new KeyValuePair<int, int>(i, j + 1)))
            {
                _maze[i, j][3] = false;
                _maze[i, j + 1][2] = false;
            }
            UpdateDisplay(i, j, w);
        }

        private List<KeyValuePair<int, int>> CloseNeighbors(int i, int j)
        {
            List<KeyValuePair<int, int>> neighbors = new List<KeyValuePair<int, int>>();

            for (int iNeighbor = i - 1; iNeighbor <= i + 1; iNeighbor++)
            {
                if (iNeighbor != i && iNeighbor >= 0 && iNeighbor < Height)
                {
                    neighbors.Add(new KeyValuePair<int, int>(iNeighbor, j));
                }
            }

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
                if (!_maze[i, j]._visited) 
                {
                    _maze[i, j]._visited = true;
                    numberVisited++;
                    List<KeyValuePair<int, int>> neighbors = CloseNeighbors(i, j).Where(c => !_maze[c.Key, c.Value]._visited).ToList();

                    if (neighbors.Count > 0)
                    {
                        if (backtracking)
                        {
                            List<KeyValuePair<int, int>> neighs = CloseNeighbors(i, j).Where(c => _maze[c.Key, c.Value]._visited).ToList();
                            int iNeigh = neighs[0].Key;
                            int jNeigh = neighs[0].Value;

                            if (iNeigh == i - 1 && jNeigh == j)
                            {
                                Open(i, j, 0);
                            }
                            else if (iNeigh == i + 1 && jNeigh == j)
                            {
                                Open(i, j, 1);
                            }
                            else if (iNeigh == i && jNeigh == j - 1)
                            {
                                Open(i, j, 2);
                            }
                            else if (iNeigh == i && jNeigh == j + 1)
                            {
                                Open(i, j, 3);
                            }
                            else
                            {
                                Console.WriteLine("Erreur ouverture");
                            }
                            backtracking = false;
                        }
                        int randomNeighbor = random.Next(0, neighbors.Count);

                        int iNeighbor = neighbors[randomNeighbor].Key;
                        int jNeighbor = neighbors[randomNeighbor].Value;

                        neighbors.Remove(neighbors[randomNeighbor]);

                        for (int neigh = 0; neigh < neighbors.Count; neigh++)
                        {
                            if (!cells.Contains(new KeyValuePair<int, int>(neighbors[neigh].Key, neighbors[neigh].Value))) 
                            {
                                cells.Push(new KeyValuePair<int, int>(neighbors[neigh].Key, neighbors[neigh].Value));
                            }
                        }

                        cells.Push(new KeyValuePair<int, int>(iNeighbor, jNeighbor));

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
                    else
                    {
                        backtracking = true;
                        if (_maze[i, j][0] && _maze[i, j][1] && _maze[i, j][2] && _maze[i, j][3])
                        {
                            List<KeyValuePair<int, int>> neighs = CloseNeighbors(i, j).Where(c => _maze[c.Key, c.Value]._visited).ToList();
                            int iNeigh = neighs[0].Key;
                            int jNeigh = neighs[0].Value;

                            if (iNeigh == i - 1 && jNeigh == j)
                            {
                                Open(i, j, 0);
                            }
                            else if (iNeigh == i + 1 && jNeigh == j)
                            {
                                Open(i, j, 1);
                            }
                            else if (iNeigh == i && jNeigh == j - 1)
                            {
                                Open(i, j, 2);
                            }
                            else if (iNeigh == i && jNeigh == j + 1)
                            {
                                Open(i, j, 3);
                            }
                            else
                            {
                                Console.WriteLine("Erreur ouverture");
                            }
                        }
                    }
                }
            }

            int randomSide = random.Next(0, 4);
            i = random.Next(0, Height);
            j = random.Next(0, Width);

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

        private string DisplayLine(int n)
        {
            string mazeLine = "";
            int i = n;

            for (int j = 0; j <= Width; j++)
            {
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

        public List<string> Display()
        {
            List<string> mazeLines = new List<string>();
            for (int i = 0; i <= Height; i++)
            {
                mazeLines.Add(DisplayLine(i));
            }
            return mazeLines;
        }

        private void UpdateDisplay(int i, int j, int w)
        {
            string mazeLine;
            if (w == 0)
            {
                mazeLine = DisplayLine(i);
                Console.SetCursorPosition(j, i);
                Console.Write(mazeLine[j]);
                Console.Write(mazeLine[j + 1]);
            }
            else if (w == 1)
            {
                mazeLine = DisplayLine(i + 1);
                Console.SetCursorPosition(j, i + 1);
                Console.Write(mazeLine[j]);
                Console.Write(mazeLine[j + 1]);
            }
            else if (w == 2)
            {
                mazeLine = DisplayLine(i);
                Console.SetCursorPosition(j, i);
                Console.Write(mazeLine[j]);
                mazeLine = DisplayLine(i + 1);
                Console.SetCursorPosition(j, i + 1);
                Console.Write(mazeLine[j]);
            }
            else if (w == 3)
            {
                mazeLine = DisplayLine(i);
                Console.SetCursorPosition(j + 1, i);
                Console.Write(mazeLine[j + 1]);
                mazeLine = DisplayLine(i + 1);
                Console.SetCursorPosition(j + 1, i + 1);
                Console.Write(mazeLine[j + 1]);
            }
            Console.SetCursorPosition(0, Height + 1);
        }
    }
}
