using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using th_stopar.Models;

namespace th_stopar
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Game _currentGame;
        private readonly Player _playerOne;
        private readonly Player _playerTwo;
        public List<Button> GameButtons = new List<Button>();

        private Player _playing;
        private bool _isSimulated;
        private int _round;
        private float _avgRounds;
        private int _gamesPlayed;
        public MainWindow()
        {
            InitializeComponent();
            _playerOne = new Player("Player 1", this);
            _playerTwo = new Player("Player 2", this);
            SetUpTheField();
        }

        private void SetUpTheField()
        {
            GameButtons.Clear();
            gridGameTable.Children.Clear();
            _round = 1;
            for (var i = 0; i < Game.Xsize; i++)
            {
                for (var j = 0; j < Game.Ysize; j++)
                {
                    var btn = new Button
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
            _currentGame = new Game();
            _playing = _playerOne;
        }

        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            var b = (Button)sender;

            var posX = Convert.ToInt32(b.Name.Split('_')[1]);
            var posY = Convert.ToInt32(b.Name.Split('_')[2]);

            if (_currentGame.Field[posX, posY] == Game.CellState.Throphy)
            {
                b.Content = Game.ThrophyMark;
                _gamesPlayed += 1;
                _playing.Wins += 1;
                _avgRounds = ((_avgRounds * (_gamesPlayed - 1)) + _round) / _gamesPlayed;
                UpdateStatsLabels();
                if (!_isSimulated)
                    MessageBox.Show($"{_playing.Name} is the winner (Round: {_round})");
                return;
            }

            if (_currentGame.Field[posX, posY] == Game.CellState.NearThrophy)
            {
                _currentGame.FieldShared[posX, posY] = Game.CellState.NearThrophy;
                b.Content = Game.NearByMark;
            }
            else
            {
                _currentGame.FieldShared[posX, posY] = Game.CellState.Empty;
                b.Content = Game.EmptyMark;
            }
            _round += 1;
            _playing = _playing == _playerOne ? _playerTwo : _playerOne;
            _playing.Play(_currentGame);
        }

        private void BtnNewField_Click(object sender, RoutedEventArgs e)
        {
            SetUpTheField();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _playerOne.PlayerStrategy = (Player.Strategy)strategyOne.SelectedIndex;
            _playerTwo.PlayerStrategy = (Player.Strategy)strategyTwo.SelectedIndex;
            _playing.Play(_currentGame);
        }

        private void UpdateStatsLabels()
        {
            LblWinsP1.Content = $"{_playerOne.Wins} ({Math.Round((_playerOne.Wins / (float)_gamesPlayed) * 100, 1)} %)";
            LblWinsP2.Content = $"{_playerTwo.Wins} ({Math.Round((_playerTwo.Wins / (float)_gamesPlayed) * 100, 1)} %)";
            LblGames.Content = $"Games: {_gamesPlayed}";
            LblAvgRounds.Content = $"Avg rounds: {Math.Round(_avgRounds, 1)}";
        }

        private void BtnSimulation100_OnClick(object sender, RoutedEventArgs e)
        {
            SetUpTheField();
            _isSimulated = true;
            var tempGamesPlayed = _gamesPlayed;
            _playerOne.PlayerStrategy = (Player.Strategy)strategyOne.SelectedIndex;
            _playerTwo.PlayerStrategy = (Player.Strategy)strategyTwo.SelectedIndex;
            _playing.Play(_currentGame);
            while (tempGamesPlayed + 99 >= _gamesPlayed)
            {
                SetUpTheField();
                _playing.Play(_currentGame);
            }
            _isSimulated = false;
        }

        private void BtnClearStats_OnClick(object sender, RoutedEventArgs e)
        {
            _round = 0;
            _gamesPlayed = 0;
            _avgRounds = 0;
            _playerOne.Wins = 0;
            _playerTwo.Wins = 0;
            UpdateStatsLabels();
        }
    }
}
