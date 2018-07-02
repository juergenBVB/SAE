using System;
using System.Collections.Generic;
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
        }

        private void generateGrids()
        {
            this.PlayerBoard.Items.Clear();
            this.PlayerBoard.Columns.Clear();
            this.OpponentBoard.Items.Clear();
            this.OpponentBoard.Columns.Clear();
            //generate opponent grid    
            for (int i = 1; i < this.MainViewModel.MainGame.Settings.BoardSize; i++)
            {
                DataGridTextColumn textColumn = new DataGridTextColumn();
                textColumn.MaxWidth = 10;
                this.PlayerBoard.Columns.Add(textColumn);
                textColumn = new DataGridTextColumn();
                textColumn.MaxWidth = 10;
                this.OpponentBoard.Columns.Add(textColumn);
            }

            for (int j = 1; j < this.MainViewModel.MainGame.Settings.BoardSize; j++)
            {
                this.PlayerBoard.Items.Add(new object[] { });
                this.OpponentBoard.Items.Add(new object[] { });
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

            int rowIndex = FindRowIndex((DataGridRow)dep);
            int columnIndex = ((DataGridCell)sender).Column.DisplayIndex;
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
    }
}
