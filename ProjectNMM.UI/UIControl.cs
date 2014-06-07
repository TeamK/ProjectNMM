using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Win32;
using ProjectNMM.Model;
using ProjectNMM.UI.Properties;

namespace ProjectNMM.UI
{
    class UiControl
    {
        private GameScreen _gameScreen;
        private ModelControl _modelControl;
        private UiHelpFunctions _helpFunctions;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public UiControl()
        {
            _modelControl = new ModelControl();
            _gameScreen = new GameScreen(new UiDelegateCollection()
            {
                NewGame = NewGame,
                EllipseClick = EllipseClickEvent,
                CloseMainWindow = CloseMainWindowEvent,
                SaveGame = SaveGame,
                LoadGame = LoadGame,
                Undo = UndoTurn,
                Redo = RedoTurn,
                NextStep = NextStep,
                AllSteps = AllSteps,
                ShowOptions = ShowOptionsDialog,
                ShowAbouts = ShowAboutDialog
            });
            _helpFunctions = new UiHelpFunctions(_gameScreen.BoardGrid, EllipseClickEvent);
            _helpFunctions.DrawLines(_gameScreen.LineGrid);
            _gameScreen.LblPlaystonesPlayer1.Foreground = Brushes.DarkCyan;
            _gameScreen.LblPlaystonesPlayer1.FontWeight = FontWeights.Bold;
            _gameScreen.LblPlaystonesPlayer2.Foreground = Brushes.Crimson;

            AdjustMenuItems();
            AdjustButtons();
        }

        /// <summary>
        /// Starts a new game
        /// </summary>
        /// <param name="gameType">Type of game</param>
        private void NewGame(GameType gameType)
        {
            if (!CanCurrentGameBeAborted())
            {
                _gameScreen.NewGameScreen = null;
                return;
            }

            // Fills the model and reloads the gui

            _modelControl.StartNewGame(gameType);

            _modelControl.PlayerName1 = _gameScreen.NewGameScreen.TxtPlayer1.Text;
            _modelControl.PlayerName2 = _gameScreen.NewGameScreen.TxtPlayer2.Text;
            _gameScreen.NewGameScreen = null;

            _modelControl.Description = Settings.Default.DefaultDescription;

            switch (gameType)
            {
                case GameType.PlayerVsMachine:
                    _modelControl.PlayerName2 += " CPU";
                    break;
                case GameType.MachineVsMachine:
                    _modelControl.PlayerName1 += " CPU";
                    _modelControl.PlayerName2 += " CPU";
                    break;
            }

            _gameScreen.LblNamePlayer1.Content = _modelControl.PlayerName1;
            _gameScreen.LblNamePlayer2.Content = _modelControl.PlayerName2;

            AdjustMenuItems();
            AdjustButtons();
            ReloadGui();
        }

        /// <summary>
        /// Adjusts menu items in main screen
        /// </summary>
        private void AdjustMenuItems()
        {
            if (_modelControl.GameInProgress)
            {
                _gameScreen.MnuSaveGame.IsEnabled = true;
                _gameScreen.MnuUndo.IsEnabled = true;
                _gameScreen.MnuRedo.IsEnabled = true;
            }
            else
            {
                _gameScreen.MnuSaveGame.IsEnabled = false;
                _gameScreen.MnuUndo.IsEnabled = false;
                _gameScreen.MnuRedo.IsEnabled = false;
            }

        }

        /// <summary>
        /// Adjusts buttons in main screen
        /// </summary>
        private void AdjustButtons()
        {
            if (_modelControl.GameInProgress &&
                _modelControl.GameType == GameType.MachineVsMachine)
            {
                _gameScreen.BtnNextStep.Visibility = Visibility.Visible;
                _gameScreen.BtnAllSteps.Visibility = Visibility.Visible;
            }
            else
            {
                _gameScreen.BtnNextStep.Visibility = Visibility.Hidden;
                _gameScreen.BtnAllSteps.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Reloads board visualization in main screen
        /// </summary>
        private void ReloadGui()
        {
            if (!_modelControl.GameInProgress)
                return;
            
            // Marks actual player
            if (_modelControl.ActivePlayer == PlaystoneState.Player1)
            {
                _gameScreen.LblNamePlayer1.Background = Brushes.Crimson;
                _gameScreen.LblNamePlayer2.Background = Brushes.White;
            }
            else
            {
                _gameScreen.LblNamePlayer1.Background = Brushes.White;
                _gameScreen.LblNamePlayer2.Background = Brushes.DarkCyan;
            }

            // Set player names
            _gameScreen.LblPlaystonesPlayer1.Content = Convert.ToString(_modelControl.PlaystonesPlayer1);
            _gameScreen.LblPlaystonesPlayer2.Content = Convert.ToString(_modelControl.PlaystonesPlayer2);

            // Refresh playstones
            _gameScreen.BoardGrid.Children.Clear();
            _helpFunctions.DrawPlaystones(_modelControl.Playstones);

            // Set player events
            _gameScreen.LblPlayer1Events.Content = GetGameEvent(PlaystoneState.Player1);
            _gameScreen.LblPlayer2Events.Content = GetGameEvent(PlaystoneState.Player2);
            
            // Show winner
            if (_modelControl.GameIsOver)
                ShowWinner();
        }

        /// <summary>
        /// Shows winner
        /// </summary>
        private void ShowWinner()
        {
            string winner = "";

            if (_modelControl.Winner == PlaystoneState.Player1)
                winner = _modelControl.PlayerName1;
            else
                winner = _modelControl.PlayerName2;

            MessageBox.Show(
                winner + " gewinnt!",
                "Spiel vorbei",
                MessageBoxButton.OK,
                MessageBoxImage.Information
                );
        }
        
        /// <summary>
        /// Method to save game, if a game is running
        /// </summary>
        /// <returns>True, if the process can be continued, false if not</returns>
        private bool CanCurrentGameBeAborted()
        {
            if (!_modelControl.GameInProgress)
                return true;

            MessageBoxResult msgResult =
                MessageBox.Show(
                "Möchten Sie das aktuelle Spiel speichern?",
                "Spiel speichern",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question
                );

            switch (msgResult)
            {
                case MessageBoxResult.Yes:
                    break;
                case MessageBoxResult.No:
                    return true;
                default:
                    return false;
            }

            return SaveGame();
        }

        /// <summary>
        /// Event for the clicked playstones
        /// </summary>
        private void EllipseClickEvent(object sender, MouseEventArgs e)
        {
            if (_modelControl.GameType == GameType.MachineVsMachine)
                return;
            
            // Gets the indexes of the clicked ellipse and gives it to the model

            int index1 = 0, index2 = 0;
            Thickness margin = ((Ellipse)sender).Margin;

            _helpFunctions.GetIndexes(margin.Left, margin.Top, ref index1, ref index2);

            _modelControl.PlaystoneChanged(index1, index2);

            ReloadGui();
        }

        /// <summary>
        /// Event for close the main window
        /// </summary>
        private void CloseMainWindowEvent(object sender, CancelEventArgs e)
        {
            if (!CanCurrentGameBeAborted())
                e.Cancel = true;
        }

        /// <summary>
        /// Saves a game
        /// </summary>
        /// <returns>True if successful, false if failure</returns>
        private bool SaveGame()
        {
            if (!_modelControl.GameInProgress)
                return false;
            
            bool saveStatus = false;
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "Mühle Speicherstand|*.nmm",
                Title = "Spiel speichern"
            };
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                saveStatus = _modelControl.SaveGame(saveFileDialog.FileName);

                if (!saveStatus)
                {
                    string errorMsg = "";

                    if (_modelControl.MoveInProgress)
                        errorMsg = "Beenden Sie zuerst den aktuellen Zug!";
                    else
                        errorMsg = "Das Spiel konnte nicht gespeichert werden!";
                    
                    MessageBox.Show(
                        errorMsg,
                        "Fehler",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }

            return saveStatus;
        }

        /// <summary>
        /// Loads a game (after user has chosen a file)
        /// </summary>
        private void LoadGame()
        {
            if (!CanCurrentGameBeAborted())
                return;
            
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Mühle Speicherstand|*.nmm",
                Title = "Spiel laden"
            };
            openFileDialog.ShowDialog();

            LoadGame(openFileDialog.FileName);
        }

        /// <summary>
        /// Loads a game
        /// </summary>
        /// <param name="path">Filepath</param>
        public void LoadGame(string path)
        {
            if (string.IsNullOrEmpty(path))
                return;

            if (!_modelControl.LoadGame(path))
            {
                MessageBox.Show(
                    "Das Spiel konnte nicht geladen werden!\n" +
                    "Ist die Datei beschädigt?",
                    "Fehler",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            _gameScreen.LblNamePlayer1.Content = _modelControl.PlayerName1;
            _gameScreen.LblNamePlayer2.Content = _modelControl.PlayerName2;

            AdjustMenuItems();
            AdjustButtons();
            ReloadGui();
        }

        /// <summary>
        /// Shows the main window
        /// </summary>
        public void StartGame()
        {
            _gameScreen.Show();
        }

        /// <summary>
        /// Get string for game event
        /// </summary>
        /// <param name="player">Active player</param>
        /// <returns>Player event</returns>
        private string GetGameEvent(PlaystoneState player)
        {
            GameEvent gameEvent;

            if (player == PlaystoneState.Player1)
                gameEvent = _modelControl.GameEventPlayer1;
            else
                gameEvent = _modelControl.GameEventPlayer2;

            switch (gameEvent)
            {
                case GameEvent.PlayerHasMill:
                    return "Mühle! Sie können einen Stein entfernen.";
                case GameEvent.WrongPlaystoneAfterMill:
                    return "Sie können nur Steine Ihres Gegners auswählen!";
                case GameEvent.CannotBreakMill:
                    return "Sie können nicht Steine aus einer Mühle entfernen!";
                case GameEvent.InvalidPlaystone:
                    return "Sie können diesen Stein nicht auswählen!";
                case GameEvent.NoPlaystonesRemovable:
                    return "Es können keine Steine entfernt werden!";
                case GameEvent.GameOverNoMovesPossible:
                    return "Spieler hat verloren (alle Steine eingeschlossen)!";
                case GameEvent.GameOverNoPlaystonesLeft:
                    return "Spieler hat verloren (weniger als drei Steine)!";
                case GameEvent.FinishTurn:
                    return "Beenden Sie Ihren Spielzug!";
                case GameEvent.CannotUndoMill:
                    return "Eine Mühle kann nicht rückgängig gemacht werden!";
                default:
                    return "";
            }
        }

        // Methods for event handling
        private void UndoTurn()
        {
            _modelControl.UndoTurn();
            ReloadGui();
        }

        private void RedoTurn()
        {
            _modelControl.RedoTurn();
            ReloadGui();
        }

        private void NextStep()
        {
            _modelControl.NextManagedStep();
            ReloadGui();
        }

        private void AllSteps()
        {
            _modelControl.EndManagedGame();
            ReloadGui();
        }

        private void ShowOptionsDialog()
        {
            OptionsScreen optionsScreen = new OptionsScreen();
            optionsScreen.ShowDialog();
        }

        private void ShowAboutDialog()
        {
            AboutScreen aboutScreen = new AboutScreen();
            aboutScreen.ShowDialog();
        }
    }
}
