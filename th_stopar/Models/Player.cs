using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace th_stopar.Models
{
    public class Player
    {
        public string Name { get; set; }
        public Strategy PlayerStrategy { get; set; }

        private Game _game;
        private MainWindow _window;
        public Player(string name, Game game, MainWindow window)
        {
            Name = name;
            _game = game;
            _window = window;
        }
        public void Play()
        {
            if (PlayerStrategy == Strategy.Random)
                PlayRandom();
        }
        private void PlayRandom()
        {
            var rnd = new Random();
            var x = rnd.Next(Game.Xsize);
            var y = rnd.Next(Game.Ysize);

            
            var targetBtn = (Button)_window.GameButtons.First(bt=>bt.Name == $"Button_{x}_{y}");
            while (targetBtn.Content.ToString() != "")
            {
                x = rnd.Next(Game.Xsize);
                y = rnd.Next(Game.Ysize);
                targetBtn = (Button)_window.GameButtons.First(bt => bt.Name == $"Button_{x}_{y}");
            }
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
