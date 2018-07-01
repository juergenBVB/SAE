using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace SAE
{
    /// <summary>
    /// Interaktionslogik für Settings.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private Settings settings;
        public SettingsWindow()
        {
            InitializeComponent();
            settings = Settings.LoadSettings();
            updateForm();
        }

        private void updateForm()
        {
            this.boardSize.Text = this.settings.BoardSize.ToString();
            this.playerColor.Text = this.settings.PlayerColor;
            this.enemyColor.Text = this.settings.EnemyColor;
            this.diffictuly.Text = this.settings.Difficulty.ToString();
            this.shipCount.Text = this.settings.ShipCount.ToString();
            this.playerName.Text = this.settings.PlayerName;
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            this.settings.BoardSize = Int32.Parse(this.boardSize.Text);
            this.settings.PlayerColor = this.playerColor.Text;
            this.settings.EnemyColor = this.enemyColor.Text;
            //this.settings.Difficulty = this.diffictuly.Text;
            this.settings.ShipCount = Int32.Parse(this.shipCount.Text);
            this.settings.PlayerName = this.playerName.Text;

            this.settings.SaveSettings();

            MessageBoxResult result = MessageBox.Show("Settings succesfully saved.",
                                          "Settings",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Information);
        }
    }
}
