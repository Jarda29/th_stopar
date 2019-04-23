using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace th_stopar.Models
{
    class Game
    {
        public const short Xsize = 4;
        public const short Ysize = 4;

        public CellState[,] Field { get; private set; }

        public Game()
        {
            Field = new CellState[Xsize, Ysize]
            {
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 }
            };
            PlaceAThrophy();
        }

        private void PlaceAThrophy()
        {
            var rnd = new Random();
            var x = rnd.Next(4);
            var y = rnd.Next(4);

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
