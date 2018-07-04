using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SAE
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel mainViewModel;
        public MainWindow()
        {
            InitializeComponent();
            this.MainViewModel = new MainViewModel();
            this.DataContext = this.MainViewModel;
            this.MainViewModel.StartScreenVisible = true;
            ((INotifyCollectionChanged)this.PlayerHitlog.Items).CollectionChanged += PlayerHitLog_CollectionChanged;
            ((INotifyCollectionChanged)this.OpponentHitlog.Items).CollectionChanged += OpponentHitLog_CollectionChanged;
        }

        public MainViewModel MainViewModel {
            get { return mainViewModel; }
            set { mainViewModel = value; }
        }

        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }

        private void OpenStartScreen_Click(object sender, RoutedEventArgs e)
        {
            this.MainViewModel.StartScreenVisible = true;
            this.MainViewModel.MainScreenVisible = false;
            this.MainViewModel.EndScreenVisible = false;
        }

        private void StartGame_click(object sender, RoutedEventArgs e)
        {
            this.MainViewModel.StartScreenVisible = false;
            this.MainViewModel.MainScreenVisible = true;
            this.MainViewModel.MainGame = new Game();
            this.PlayerHitlog.ItemsSource = this.MainViewModel.MainGame.Player.HitLog;
            this.OpponentHitlog.ItemsSource = this.MainViewModel.MainGame.Ai.HitLog;

            this.generateGrids();
            this.MainViewModel.StartTimer();
        }

        private void generateGrids()
        {
            int boardSize = this.MainViewModel.MainGame.Settings.BoardSize;

            this.MainViewModel.PlayerSquareViewList.Clear();
            this.MainViewModel.AISquareViewList.Clear();     

            foreach (var square in this.MainViewModel.MainGame.Ai.Board.Squares)
            {
                SolidColorBrush color = Brushes.White;
                if (square.IsShipPart())
                {
                    color = new SolidColorBrush(this.MainViewModel.MainGame.Settings.PlayerColor);
                }

                this.MainViewModel.PlayerSquareViewList.Add(new SquareView(square.PositionX, square.PositionY, color));
            }

            foreach (var square in this.MainViewModel.MainGame.Player.Board.Squares)
            {
                this.MainViewModel.AISquareViewList.Add(new SquareView(square.PositionX, square.PositionY, Brushes.White));
            }

            this.MainViewModel.ColumnWidth = this.PlayerBoard.Width / boardSize;
            this.MainViewModel.RowHeight = this.PlayerBoard.Height / boardSize;

            this.OpponentBoard.ItemsSource = this.MainViewModel.AISquareViewList;
            this.PlayerBoard.ItemsSource = this.MainViewModel.PlayerSquareViewList;               
        }

        private void QuitGame_Click(object sender, RoutedEventArgs e)
        {
            this.MainViewModel.MainScreenVisible = false;
            this.MainViewModel.StartScreenVisible = true;
        }

        private void PlayerHitLog_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                // scroll the new item into view   
                this.PlayerHitlog.ScrollIntoView(e.NewItems[0]);
            }
        }

        private void OpponentHitLog_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                // scroll the new item into view   
                this.OpponentHitlog.ScrollIntoView(e.NewItems[0]);
            }
        }

        private void Opponent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.OpponentBoard.SelectedItem != null)
            {
                SquareView selected = ((SquareView)this.OpponentBoard.SelectedItem);
                Boolean playerhit = this.MainViewModel.MainGame.ExecuteMove(selected.PositionX, selected.PositionY);

                Color enemyColor = this.MainViewModel.MainGame.Settings.EnemyColor;
                selected.BackgroundColor = playerhit ? new SolidColorBrush(enemyColor) : new SolidColorBrush(Colors.Gray);

                Boolean aiHit = this.MainViewModel.MainGame.ExecuteMove();
                Square hitSquare = this.MainViewModel.MainGame.Ai.HitLog.Last();
                this.MainViewModel.PlayerSquareViewList[GameBoard.GetIndexOfCoordinates(
                    hitSquare.PositionX, hitSquare.PositionY, this.MainViewModel.MainGame.Settings.BoardSize)].BackgroundColor = aiHit ? Brushes.Red : Brushes.Gray;

                if (this.MainViewModel.MainGame.IsGameOver())
                {
                    this.MainViewModel.MainScreenVisible = false;
                    this.MainViewModel.EndScreenVisible = true;
                    this.MainViewModel.EndScreenText = this.MainViewModel.MainGame.CalculateWinner() ? "Congratulations, you won!" : "Sorry, but you lost.";
                }

                this.OpponentBoard.SelectedItem = null;
            }
        }
    }
}
