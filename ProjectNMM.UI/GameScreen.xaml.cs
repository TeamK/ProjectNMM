using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace ProjectNMM.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GameScreen : Window
    {
        private UiDelegateCollection _delegateCollection;
        public NewGameScreen NewGameScreen;

        /// <summary>
        /// Creates a new screen
        /// </summary>
        /// <param name="delegateCollection">Delegates for events</param>
        public GameScreen(UiDelegateCollection delegateCollection)
        {
            InitializeComponent();

            _delegateCollection = delegateCollection;

            BtnNextStep.Content = "Nächster Schritt";
            BtnNextStep.Content = "Nächster Schritt";
        }

        // Event handlings
        private void MnuNewGame_OnClick(object sender, RoutedEventArgs e)
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

        private void BtnNextStep_Click(object sender, RoutedEventArgs e)
        {
            _delegateCollection.NextStep();
        }

        private void BtnAllSteps_Click(object sender, RoutedEventArgs e)
        {
            _delegateCollection.AllSteps();
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
