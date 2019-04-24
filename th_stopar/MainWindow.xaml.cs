using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using th_stopar.Models;

namespace th_stopar
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game CurrentGame;
        private Player _playerOne;
        private Player _playerTwo;
        public List<Button> GameButtons = new List<Button>();

        private Player _playing;
        private int _round = 0;
        public MainWindow()
        {
            InitializeComponent();
            SetUpTheField();
        }

        private void SetUpTheField()
        {
            CurrentGame = new Game();
            GameButtons.Clear();
            _round = 1;
            for (int i = 0; i < Game.Xsize; i++)
            {
                for (int j = 0; j < Game.Ysize; j++)
                {
                    Button btn = new Button
                    {
                        Content = "",
                        Name = $"Button_{i}_{j}"
                    };

                    btn.Click += GameButton_Click;

                    Grid.SetColumn(btn, j);
                    Grid.SetRow(btn, i);
                    gridGameTable.Children.Add(btn);
                    GameButtons.Add(btn);
                }
            }
            _playerOne = new Player("Player 1", CurrentGame, this);
            _playerTwo = new Player("Player 2", CurrentGame, this);
            _playing = _playerOne;
        }

        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            var s = b.Name;

            var posX = Convert.ToInt32(b.Name.Split('_')[1]);
            var posY = Convert.ToInt32(b.Name.Split('_')[2]);

            var test = CurrentGame.GetNearCells(posX, posY);

            if(CurrentGame.Field[posX, posY] == Game.CellState.Throphy)
            {
                b.Content = Game.ThrophyMark;
                MessageBox.Show($"{_playing.Name} is the winner (Round: {_round})");                
                return;
            }

            if(CurrentGame.Field[posX, posY] == Game.CellState.NearThrophy)
            {
                CurrentGame.FieldShared[posX, posY] = Game.CellState.NearThrophy;
                b.Content = Game.NearByMark;
            }
            else
            {
                CurrentGame.FieldShared[posX, posY] = Game.CellState.Empty;
                b.Content = Game.EmptyMark;
            }
            _round += 1;
            if (_playing == _playerOne)
                _playing = _playerTwo;
            else
                _playing = _playerOne;
            _playing.Play();
        }

        private void BtnNewField_Click(object sender, RoutedEventArgs e)
        {
            SetUpTheField();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _playerOne.PlayerStrategy = (Player.Strategy)strategyOne.SelectedIndex;
            _playerTwo.PlayerStrategy = (Player.Strategy)strategyTwo.SelectedIndex;
            _playing.Play();
        }
    }
}
