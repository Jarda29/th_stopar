using System;

namespace th_stopar.Models
{
    public class Game
    {
        public const short Xsize = 8;
        public const short Ysize = 8;

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
            for (int i = 0; i < Game.Xsize; i++)
            {
                if (i - 1 != x &&
                        i != x &&
                        i + 1 != x)
                    continue;
                for (int j = 0; j < Game.Ysize; j++)
                {
                    if (Field[i, j] == CellState.Throphy)
                        continue;

                    if (j - 1 != y &&
                        j != y &&
                        j + 1 != y)
                        continue;

                    Field[i, j] = CellState.NearThrophy;
                }
            }


        }

        public enum CellState
        {
            Empty = 0,
            Throphy = 1,
            NearThrophy = 2
        }
    }
}
