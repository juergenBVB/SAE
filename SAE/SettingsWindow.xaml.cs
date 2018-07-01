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

        internal Settings Settings
        {
            get { return settings; }
            set { settings = value; }
        }     

        public SettingsWindow()
        {
            InitializeComponent();

            //this.difficulty.Items.Add(AIDifficulty.Easy.ToString());
            //this.difficulty.Items.Add(AIDifficulty.Normal.ToString());

            Settings = Settings.LoadSettings();
            this.SettingsForm.DataContext = this.Settings;
            updateForm();
        }

        private void updateForm()
        {
            //this.boardSize.Text = this.Settings.BoardSize.ToString();
            //this.playerColor.Text = this.Settings.PlayerColor;
            //this.enemyColor.Text = this.Settings.EnemyColor;
            //this.difficulty.SelectedIndex = this.difficulty.Items.IndexOf(this.Settings.Difficulty.ToString());
            //this.shipCount.Text = this.Settings.ShipCount.ToString();
            //this.playerName.Text = this.Settings.PlayerName;
        }

     

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            this.Settings.SaveSettings();

            MessageBox.Show("Settings succesfully saved.",
                                          "Settings",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Information);
        }
    }
}
