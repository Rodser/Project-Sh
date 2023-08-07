using System.Collections.Generic;
using UnityEngine;

namespace Rodser.Model
{
    public class HexGrid
    {
        public Ground[,] Grounds { get; internal set; }

        internal void Initiation()
        {
            for (int y = 0; y < Grounds.GetLength(1); y++)
            {
                for (int x = 0; x < Grounds.GetLength(0); x++)
                {
                    AddAllNeighbors(y, x);

                    // AppointPit
                    var r = Random.Range(0, 10);
                    if(r > 8)
                    {
                        Grounds[x, y].AppointPit();
                    }
                }
            }

            var xHole = Random.Range(0, Grounds.GetLength(0));
            var yHole = Random.Range(Grounds.GetLength(1) / 2, Grounds.GetLength(1));

            Grounds[xHole, yHole].AppointHole();
        }

        private void AddAllNeighbors(int y, int x)
        {
            List<Ground> neighbors = new List<Ground>();
            FindNeighbors(x, y, neighbors);

            Grounds[x, y].AddNeighbors(neighbors);
        }

        private void FindNeighbors(int x, int y, List<Ground> neighbors)
        {
            if (y % 2 == 0)
            {
                if (x - 1 >= 0)
                {
                    neighbors.Add(Grounds[x - 1, y]);
                }

                if (x + 1 < Grounds.GetLength(0))
                {
                    neighbors.Add(Grounds[x + 1, y]);

                    if (y + 1 < Grounds.GetLength(1))
                    {
                        neighbors.Add(Grounds[x + 1, y + 1]);
                    }
                }

                if (y + 1 < Grounds.GetLength(1))
                {
                    neighbors.Add(Grounds[x, y + 1]);
                }

                if (x + 1 < Grounds.GetLength(0) && y - 1 >= 0)
                {
                    neighbors.Add(Grounds[x + 1, y - 1]);
                }

                if (y - 1 >= 0)
                {
                    neighbors.Add(Grounds[x, y - 1]);
                }
            }
            else
            {
                if (x - 1 >= 0)
                {
                    neighbors.Add(Grounds[x - 1, y]);

                    if (y + 1 < Grounds.GetLength(1))
                    {
                        neighbors.Add(Grounds[x - 1, y + 1]);
                    }
                    if (y - 1 >= 0)
                    {
                        neighbors.Add(Grounds[x - 1, y - 1]);
                    }
                }

                if (y + 1 < Grounds.GetLength(1))
                {
                    neighbors.Add(Grounds[x, y + 1]);
                }

                if (x + 1 < Grounds.GetLength(0))
                {
                    neighbors.Add(Grounds[x + 1, y]);
                }

                if (y - 1 >= 0)
                {
                    neighbors.Add(Grounds[x, y - 1]);
                }
            }                   
        }
    }
}