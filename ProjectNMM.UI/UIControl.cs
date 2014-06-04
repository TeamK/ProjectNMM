using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Win32;
using ProjectNMM.Model;

namespace ProjectNMM.UI
{
    class UiControl
    {
        private GameScreen _gameScreen;
        private ModelControl _modelControl;
        private UiHelpFunctions _helpFunctions;

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
                ShowOptions = ShowOptionsDialog,
                ShowAbouts = ShowAboutDialog
            });
            _helpFunctions = new UiHelpFunctions(_gameScreen.BoardGrid, EllipseClickEvent);
            _helpFunctions.DrawLines(_gameScreen.LineGrid);
            _gameScreen.LblPlaystonesPlayer1.Foreground = Brushes.DarkCyan;
            _gameScreen.LblPlaystonesPlayer1.FontWeight = FontWeights.Bold;
            _gameScreen.LblPlaystonesPlayer2.Foreground = Brushes.Crimson;
            AdjustMenuItems();
        }

        public void NewGame(GameType gameType)
        {
            if (!CanCurrentGameBeAborted())
            {
                _gameScreen.NewGameScreen = null;
                return;
            }

            _modelControl.StartNewGame(gameType, GameStartType.StartNew);

            AdjustMenuItems();

            _modelControl.PlayerName1 = _gameScreen.NewGameScreen.TxtPlayer1.Text;
            _modelControl.PlayerName2 = _gameScreen.NewGameScreen.TxtPlayer2.Text;
            _gameScreen.NewGameScreen = null;

            switch (gameType)
            {
                case GameType.PlayerVsMachine:
                    _modelControl.PlayerName2 += " (CPU)";
                    break;
                case GameType.MachineVsMachine:
                    _modelControl.PlayerName1 += " (CPU)";
                    _modelControl.PlayerName2 += " (CPU)";
                    break;
            }

            _gameScreen.LblNamePlayer1.Content = _modelControl.PlayerName1;
            _gameScreen.LblNamePlayer2.Content = _modelControl.PlayerName2;

            ReloadGui();

            //if (gameType == GameType.MachineVsMachine)
            //    StartManagedGame();
        }

        private void StartManagedGame()
        {
            while (!_modelControl.GameIsOver)
            {
                Thread.Sleep(500);
                _modelControl.NextManagedStep();
                ReloadGui();
            }
            
        }

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

        private void ReloadGui()
        {
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

            _gameScreen.LblPlaystonesPlayer1.Content = Convert.ToString(_modelControl.PlaystonesPlayer1);
            _gameScreen.LblPlaystonesPlayer2.Content = Convert.ToString(_modelControl.PlaystonesPlayer2);

            _gameScreen.BoardGrid.Children.Clear();
            _helpFunctions.DrawPlaystones(_modelControl.Playstones);

            _gameScreen.LblPlayer1Events.Content = GetGameEvent(PlaystoneState.Player1);
            _gameScreen.LblPlayer2Events.Content = GetGameEvent(PlaystoneState.Player2);

            if (_modelControl.GameIsOver)
                ShowWinner();
        }

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

        private void EllipseClickEvent(object sender, MouseEventArgs e)
        {
            int index1 = 0, index2 = 0;
            Thickness margin = ((Ellipse)sender).Margin;

            _helpFunctions.GetIndexes(margin.Left, margin.Top, ref index1, ref index2);

            _modelControl.PlaystoneChanged(index1, index2);

            ReloadGui();
        }

        private void CloseMainWindowEvent(object sender, CancelEventArgs e)
        {
            if (!CanCurrentGameBeAborted())
                e.Cancel = true;
        }

        private bool SaveGame()
        {
            bool saveStatus = false;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Mühle Speicherstand|*.nmm";
            saveFileDialog.Title = "Spiel speichern";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                saveStatus = _modelControl.SaveGame(saveFileDialog.FileName);

                if (!saveStatus)
                {
                    MessageBox.Show(
                        "Das Spiel konnte nicht gespeichert werden!",
                        "Fehler",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }

            return saveStatus;
        }

        private void LoadGame()
        {

        }

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

        private void ShowOptionsDialog()
        {

        }

        private void ShowAboutDialog()
        {
            AboutScreen aboutScreen = new AboutScreen();
            aboutScreen.ShowDialog();
        }

        public void StartGame()
        {
            _gameScreen.Show();
        }

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
    }
}
