using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProjectNMM.Model;

namespace ProjectNMM.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GameScreen : Window
    {
        private UiDelegateCollection _delegateCollection;
        public NewGameScreen NewGameScreen;

        public GameScreen(UiDelegateCollection delegateCollection)
        {
            InitializeComponent();

            _delegateCollection = delegateCollection;
        }

        private void NewGame_OnClick(object sender, RoutedEventArgs e)
        {
            NewGameScreen = new NewGameScreen();
            NewGameScreen.ShowDialog();

            if (!NewGameScreen.CreateNewGame)
            {
                NewGameScreen = null;
                return;
            }

            _delegateCollection.NewGame(NewGameScreen.GameType);
        }

        private void MnuSaveGame_OnClick(object sender, RoutedEventArgs e)
        {
            _delegateCollection.SaveGame();
        }

        private void MnuLoadGame_OnClickGame_OnClick(object sender, RoutedEventArgs e)
        {
            _delegateCollection.LoadGame();
        }

        private void MnuUndo_OnClick(object sender, RoutedEventArgs e)
        {
            _delegateCollection.Undo();
        }

        private void MnuRedo_OnClick(object sender, RoutedEventArgs e)
        {
            _delegateCollection.Redo();
        }

        private void MnuOptions_OnClick(object sender, RoutedEventArgs e)
        {
            _delegateCollection.ShowOptions();
        }

        private void MnuAbout_OnClick(object sender, RoutedEventArgs e)
        {
            _delegateCollection.ShowAbouts();
        }

        public void Ellipse_OnClick(object sender, MouseEventArgs e)
        {
            _delegateCollection.EllipseClick(sender, e);
        }

        private void GameScreen_OnClosing(object sender, CancelEventArgs e)
        {
            _delegateCollection.CloseMainWindow(sender, e);
        }
    }
}
