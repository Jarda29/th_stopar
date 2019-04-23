using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using th_stopar.Models;

namespace th_stopar
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Game CurrentGame = new Game();
        private Player _playerOne;
        public List<Button> GameButtons = new List<Button>();
        public MainWindow()
        {
            InitializeComponent();
            SetUpTheField();
            _playerOne = new Player("Player 1", CurrentGame, this);
        }

        private void SetUpTheField()
        {
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
        }

        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            var s = b.Name;

            var posX = Convert.ToInt16(b.Name.Split('_')[1]);
            var posY = Convert.ToInt16(b.Name.Split('_')[2]);

            if(CurrentGame.Field[posX, posY] == Game.CellState.Throphy)
            {
                b.Content = "X";
                MessageBox.Show("You are the winner");
                return;
            }

            if(CurrentGame.Field[posX, posY] == Game.CellState.NearThrophy)
            {
                b.Content = "!";
            }

        }

        private void BtnNewField_Click(object sender, RoutedEventArgs e)
        {
            SetUpTheField();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _playerOne.PlayRandom();
        }
    }
}
