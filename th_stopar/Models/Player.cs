using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace th_stopar.Models
{
    public class Player
    {
        public string Name { get; set; }
        public Strategy PlayerStrategy { get; set; }

        public int Wins { get; set; }

        private readonly MainWindow _window;

        private readonly Random _random;
        public Player(string name, MainWindow window)
        {
            Name = name;
            _window = window;
            _random = new Random();
        }
        public void Play(Game game)
        {
            if (PlayerStrategy == Strategy.Random)
                PlayRandom();
            else if (PlayerStrategy == Strategy.Smart)
                PlaySmart(game);
        }
        private void PlayRandom()
        {
            var x = _random.Next(Game.Xsize);
            var y = _random.Next(Game.Ysize);


            var targetBtn = _window.GameButtons.First(bt => bt.Name == $"Button_{x}_{y}");
            while (targetBtn.Content.ToString() != "")
            {
                x = _random.Next(Game.Xsize);
                y = _random.Next(Game.Ysize);
                targetBtn = _window.GameButtons.First(bt => bt.Name == $"Button_{x}_{y}");
            }
            targetBtn.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }
        private void PlaySmart(Game game)
        {
            var fieldP = new float[Game.Xsize, Game.Ysize];
            for (int k = 0; k < fieldP.GetLength(0); k++)
            {
                for (int l = 0; l < fieldP.GetLength(1); l++)
                {
                    var nearCells = game.GetNearCells(k, l);
                    if (game.FieldShared[k, l] == Game.CellState.NearThrophy)
                    {
                        var p = 1 / (float)nearCells.Count;
                        foreach (var cells in game.GetNearCells(k, l))
                        {
                            fieldP[cells[0], cells[1]] += p;
                        }
                    }
                    else if (game.FieldShared[k, l] == Game.CellState.Empty)
                    {
                        fieldP[k, l] = -1;
                        foreach (var cells in game.GetNearCells(k, l))
                        {
                            fieldP[cells[0], cells[1]] = -1;
                        }
                    }
                    else
                    {
                        fieldP[k, l] += 0.01f;
                    }
                }
            }


            var highestPCell = new[] { 0, 0 };
            float maxP = 0;
            for (int k = 0; k < fieldP.GetLength(0); k++)
            {
                for (int l = 0; l < fieldP.GetLength(1); l++)
                {
                    if (fieldP[k, l] <= maxP)
                        continue;

                    highestPCell[0] = k;
                    highestPCell[1] = l;
                    maxP = fieldP[k, l];
                }
            }
            var targetBtn = _window.GameButtons.First(bt => bt.Name == $"Button_{highestPCell[0]}_{highestPCell[1]}");
            targetBtn.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }
        public enum Strategy
        {
            Human = 0,
            Random = 1,
            Smart = 2
        }
    }
}
