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
    /// Interaction logic for StartScreen.xaml
    /// </summary>
    public partial class StartScreen : Window
    {
        private StartNewGame _startNewGame;
        
        public StartScreen(StartNewGame startNewGame)
        {
            InitializeComponent();

            _startNewGame = startNewGame;
        }

        private void ChoosePlayerVsPlayer(object sender, MouseButtonEventArgs e)
        {
            _startNewGame(GameType.PlayerVsPlayer);
        }

        private void ChoosePlayerVsComputer(object sender, MouseButtonEventArgs e)
        {
            _startNewGame(GameType.PlayerVsMachine);
        }

        private void ChooseComputerVsComputer(object sender, MouseButtonEventArgs e)
        {
            _startNewGame(GameType.MachineVsMachine);
        }

        private void ChoosePlayOnline(object sender, MouseButtonEventArgs e)
        {
            _startNewGame(GameType.Online);
        }


        private void LblStartGameScreen_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("worked!");
        }

        private void LblExitProgram_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("worked!");
        }
    }
}
