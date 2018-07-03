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

        public MainViewModel MainViewModel { get => mainViewModel; set => mainViewModel = value; }

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
            this.showPlayerShips();
            this.MainViewModel.StartTimer();
        }

        private void showPlayerShips()
        {
            List<Ship> playerShips = this.MainViewModel.MainGame.PlayerGameBoard.Ships;
            foreach (var ship in playerShips)
            {
                foreach (var shipPart in ship.ShipParts)
                {
                   // DataGridRow row = (DataGridRow)this.PlayerBoard.Items[shipPart.PositionX];
                }
            }
        }

        private void generateGrids()
        {
            this.PlayerBoard.Items.Clear();
            this.PlayerBoard.Columns.Clear();
            this.OpponentBoard.Items.Clear();
            this.OpponentBoard.Columns.Clear();

            for (int i = 0; i < this.MainViewModel.MainGame.Settings.BoardSize; i++)
            {
                DataGridTextColumn textColumn = new DataGridTextColumn();
                textColumn.MaxWidth = 10;
                this.PlayerBoard.Columns.Add(textColumn);

                textColumn = new DataGridTextColumn();
                textColumn.MaxWidth = 10;
                this.OpponentBoard.Columns.Add(textColumn);
            }

            for (int j = 0; j < this.MainViewModel.MainGame.Settings.BoardSize; j++)
            {
                this.PlayerBoard.Items.Add(new object[] {});
                this.OpponentBoard.Items.Add(new object[] {});
            }
        }

        private void QuitGame_Click(object sender, RoutedEventArgs e)
        {
            this.MainViewModel.MainScreenVisible = false;
            this.MainViewModel.StartScreenVisible = true;
        }

        private void attack_Click(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) &&
            !(dep is DataGridCell) && !(dep is DataGridColumnHeader))
    {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep is DataGridCell)
            {
                DataGridCell cell = dep as DataGridCell;

                while ((dep != null) && !(dep is DataGridRow))
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }

                DataGridRow row = dep as DataGridRow;
                
            }

            DataGridCell clickedCell = (DataGridCell)sender;

            int rowIndex = FindRowIndex((DataGridRow)dep);
            int columnIndex = clickedCell.Column.DisplayIndex;

            Boolean hit = this.MainViewModel.MainGame.ExecuteMove(columnIndex, rowIndex);

            Color enemyColor = this.MainViewModel.MainGame.Settings.EnemyColor;
            clickedCell.Background = hit ? new SolidColorBrush(enemyColor) : new SolidColorBrush(Colors.Gray);

            this.MainViewModel.MainGame.ExecuteMove();
        }

        private int FindRowIndex(DataGridRow row)
        {
            DataGrid dataGrid =
                ItemsControl.ItemsControlFromItemContainer(row)
                as DataGrid;

            int index = dataGrid.ItemContainerGenerator.
                IndexFromContainer(row);

            return index;
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
    }
}
