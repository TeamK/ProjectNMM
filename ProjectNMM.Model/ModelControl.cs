using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace ProjectNMM.Model
{
    public class ModelControl
    {
        private GameFlowHandler _gameHandler;
        private Random _random;
        
        public ModelControl()
        {
            _random = new Random();
        }

        #region Properties

        public bool GameInProgress
        {
            get
            {
                if (_gameHandler == null)
                    return false;
                else
                    return true;
            }
        }

        public GameType GameType
        {
            get { return _gameHandler.Game.GameType; }
            set { _gameHandler.Game.GameType = value; }
        }

        public PlaystoneState[,] Playstones
        {
            get { return _gameHandler.CurrentPlaystones; }
        }

        public PlaystoneState ActivePlayer
        {
            get { return _gameHandler.CurrentPlayer; }
        }

        public bool MoveIsActive
        {
            get { return _gameHandler.Game.MoveIsActive; }
        }

        public string PlayerName1
        {
            get { return _gameHandler.Game.PlayerName1; }
            set { _gameHandler.Game.PlayerName1 = value; }
        }

        public string PlayerName2
        {
            get { return _gameHandler.Game.PlayerName2; }
            set { _gameHandler.Game.PlayerName2 = value; }
        }

        public int PlaystonesPlayer1
        {
            get { return _gameHandler.Game.BoardStates[_gameHandler.Game.BoardStates.Count - 1].PlaystonesPlayer1; }
        }

        public int PlaystonesPlayer2
        {
            get { return _gameHandler.Game.BoardStates[_gameHandler.Game.BoardStates.Count - 1].PlaystonesPlayer2; }
        }

        public DateTime StartTime
        {
            get { return _gameHandler.Game.StartTime; }
        }

        public DateTime EndTime
        {
            get { return _gameHandler.Game.EndTime; }
        }

        public bool GameIsOver
        {
            get { return _gameHandler.Game.GameIsOver; }
        }

        public PlaystoneState Winner
        {
            get { return _gameHandler.Game.Winner; }
        }

        public string Description
        {
            get { return _gameHandler.Game.Description; }
            set { _gameHandler.Game.Description = value; }
        }

        public bool LastTurnWasMill
        {
            get { return _gameHandler.LastTurnWasMill; }
        }

        public GameEvent GameEventPlayer1 
        {
            get { return _gameHandler.GameEventPlayer1; }
        }

        public GameEvent GameEventPlayer2
        {
            get { return _gameHandler.GameEventPlayer2; }
        }

        #endregion

        public void StartNewGame(GameType gameType, GameStartType startType)
        {
            _gameHandler = new GameFlowHandler(gameType, startType);
        }

        public void NextManagedStep()
        {
            if (_gameHandler != null &&
                _gameHandler.Game.GameType == GameType.MachineVsMachine)
                ExecuteMachineStep();
        }

        public void PlaystoneChanged(int index1, int index2)
        {
            if (!GameInProgress)
                return;

            _gameHandler.PlaystoneChanged(index1, index2);

            if (GameType == GameType.PlayerVsMachine &&
                ActivePlayer == PlaystoneState.Player2)
                ExecuteMachineStep();
        }

        public void UndoTurn()
        {
            if (GameInProgress)
                _gameHandler.UndoLastTurn();
        }

        public void RedoTurn()
        {
            if (GameInProgress)
                _gameHandler.RedoLastTurn();
        }

        public bool SaveGame(string path)
        {
            if (path == "" || !_gameHandler.GameHasStarted ||
                _gameHandler.Game.MoveIsActive || !GameInProgress)
                return false;
            
            if (File.Exists(path))
                File.Delete(path);
            if (File.Exists(path))
                return false;

            bool returnValue = false;

            try
            {
                returnValue = GameFileFunctions.SaveGame(_gameHandler.Game, path);
            }
            catch(Exception ex)
            {
                returnValue = false;
            }

            return returnValue;
        }

        private void ExecuteMachineStep()
        {
            if (GameIsOver)
                return;

            int index1 = -1, index2 = -1;

            if (LastTurnWasMill)
            {
                ArtificialIntelligence.ChoseRandomPlaystone(_gameHandler.NotCurrentPlayer, Playstones, ref index1, ref index2, _random);
            }
            else if (!_gameHandler.GameHasStarted || (MoveIsActive && ModelHelpFunctions.CountPlaystoneStates(PlaystoneState.Selectable, Playstones) > 0))
            {
                ArtificialIntelligence.ChoseRandomPlaystone(PlaystoneState.Selectable, Playstones, ref index1, ref index2, _random);
            }
            else
            {
                ArtificialIntelligence.ChoseRandomPlaystone(ActivePlayer, Playstones, ref index1, ref index2, _random);
            }

            PlaystoneChanged(index1, index2);
        }
    }
}
