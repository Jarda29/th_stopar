using System;
using System.Collections.Generic;

namespace th_stopar.Models
{
    public class Game
    {
        public const int Xsize = 8;
        public const int Ysize = 8;

        public const string ThrophyMark = "X";
        public const string NearByMark = "!";
        public const string EmptyMark = "O";

        public CellState[,] Field { get; private set; }

        public Game()
        {
            Field = new CellState[Xsize, Ysize];
            PlaceAThrophy();
        }

        private void PlaceAThrophy()
        {
            var rnd = new Random();
            var x = rnd.Next(Xsize);
            var y = rnd.Next(Ysize);

            Field[x, y] = CellState.Throphy;
            foreach(var cell in GetNearCells(x, y))
            {
                Field[cell[0], cell[1]] = CellState.NearThrophy;
            }
        }

        public List<int[]> GetNearCells(int x, int y)
        {
            var list = new List<int[]>();
            for (int i = -1; i <= 1; i++)
            {
                if (y + i < 0)
                    continue;
                if (y + i >= Ysize)
                    continue;
                for (int j = -1; j <= 1; j++)
                {
                    if (x + j < 0)
                        continue;
                    if (x + j >= Xsize)
                        continue;
                    if ((x + j) == x && (y + i) == y)
                        continue;
                    list.Add(new int[] { (x + j), (y + i) });
                }
            }
            return list;
        }

        public enum CellState
        {
            Empty = 0,
            Throphy = 1,
            NearThrophy = 2
        }
    }
}
