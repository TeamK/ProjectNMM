using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProjectNMM.Model;

namespace ProjectNMM.UI
{
    /// <summary>
    /// Interaction logic for NewGameScreen.xaml
    /// </summary>
    public partial class NewGameScreen : Window
    {
        public bool CreateNewGame { get; private set; }
        
        public NewGameScreen()
        {
            InitializeComponent();

            CboGameType.ItemsSource = new List<string>
            {
                "Spieler gegen Spieler",
                "Spieler gegen Computer",
                "Computer gegen Computer",
                "Online Modus"
            };
            CreateNewGame = false;
        }

        public GameType GameType
        {
            get
            {
                if (CboGameType.Text == "Spieler gegen Spieler")
                    return GameType.PlayerVsPlayer;
                else if (CboGameType.Text == "Spieler gegen Computer")
                    return GameType.PlayerVsMachine;
                else if (CboGameType.Text == "Computer gegen Computer")
                    return GameType.MachineVsMachine;
                else
                    return GameType.Online;
            }
        }

        private void BtnStartGame_Click(object sender, RoutedEventArgs e)
        {
            if (TxtPlayer1.Text == "")
            {
                TxtPlayer1.Background = Brushes.Red;
                return;
            }
            else
                TxtPlayer1.Background = Brushes.White;

            if (TxtPlayer2.Text == "")
            {
                TxtPlayer2.Background = Brushes.Red;
                return;
            }
            else
                TxtPlayer2.Background = Brushes.White;

            if (CboGameType.Text == "")
            {
                CboGameType.Background = Brushes.Red;
                return;
            }
            else
                CboGameType.Background = Brushes.White;

            CreateNewGame = true;

            Close();
        }
    }
}
