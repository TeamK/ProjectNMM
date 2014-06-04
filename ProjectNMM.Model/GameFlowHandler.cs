using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectNMM.Model
{
    class GameFlowHandler
    {
        public GameData Game { get; private set; }
        public bool GameHasStarted { get; private set; }
        public bool LastTurnWasMill { get; private set; }
        public GameEvent GameEventPlayer1 { get; private set; }
        public GameEvent GameEventPlayer2 { get; private set; }
        private List<BoardState> _boardStates;
        private BoardState _lastBoardState;
        private bool _beforeLastTurnWasMill;
        private int LegimateTurns;
        
        public GameFlowHandler(GameType gameType, GameStartType startType)
        {
            Game = new GameData(gameType);
            _boardStates = Game.BoardStates;

            _boardStates.Add(new BoardState());
            
            CurrentPlayer = PlaystoneState.Player1;
            ModelHelpFunctions.InitializePlaystoneStates(CurrentPlaystones, PlaystoneState.Selectable);

            GameHasStarted = false;
            LastTurnWasMill = false;
            LegimateTurns = 0;
            GameEventPlayer1 = GameEvent.NoEvent;
            GameEventPlayer2 = GameEvent.NoEvent;
        }

        #region Properties

        public PlaystoneState[,] CurrentPlaystones
        {
            get { return _boardStates[_boardStates.Count - 1].Playstones; }
            private set { _boardStates[_boardStates.Count - 1].Playstones = value; }
        }

        public PlaystoneState CurrentPlayer
        {
            get { return _boardStates[_boardStates.Count - 1].ActivePlayer; }
            private set { _boardStates[_boardStates.Count - 1].ActivePlayer = value; }
        }

        public GameEvent CurrentPlayerEvent
        {
            get
            {
                if (CurrentPlayer == PlaystoneState.Player1)
                    return GameEventPlayer1;
                else
                    return GameEventPlayer2;
                
            }
            private set
            {
                if (CurrentPlayer == PlaystoneState.Player1)
                    GameEventPlayer1 = value;
                else
                    GameEventPlayer2 = value;
            }
        }

        public PlaystoneState NotCurrentPlayer
        {
            get
            {
                if (CurrentPlayer == PlaystoneState.Player1)
                    return PlaystoneState.Player2;
                else
                    return PlaystoneState.Player1;
            }
        }

        #endregion

        public void PlaystoneChanged(int index1, int index2)
        {
            if (Game.GameIsOver)
                return;
            
            bool skipLastStep = false;

            GameEventPlayer1 = GameEvent.NoEvent;
            GameEventPlayer2 = GameEvent.NoEvent;

            if (LastTurnWasMill)
                skipLastStep = RemovePlaystone(index1, index2);
            else if (GameHasStarted)
                MovePlaystone(index1, index2);
            else
                SetPlaystone(index1, index2);

            if (!skipLastStep)
            {
                if (LastTurnWasMill)
                {
                    CurrentPlayer = NotCurrentPlayer;
                    ModelHelpFunctions.ReplacePlaystoneStates(PlaystoneState.Selectable, PlaystoneState.Neutral, CurrentPlaystones);
                }
            }

            if (LastTurnWasMill && !ArePlaystonesNotInMills(index1, index2, NotCurrentPlayer, CurrentPlaystones))
            {
                LastTurnWasMill = false;

                if (CurrentPlayer == PlaystoneState.Player1)
                {
                    GameEventPlayer1 = GameEvent.NoPlaystonesRemovable;
                    GameEventPlayer2 = GameEvent.NoEvent;
                }
                else
                {
                    GameEventPlayer1 = GameEvent.NoEvent;
                    GameEventPlayer2 = GameEvent.NoPlaystonesRemovable;
                }

                CurrentPlayer = NotCurrentPlayer;
            }

            if (_boardStates[_boardStates.Count - 1].PlaystonesPlayer1 > 6 ||
                _boardStates[_boardStates.Count - 1].PlaystonesPlayer2 > 6)
            {
                Game.GameIsOver = true;
                CurrentPlayerEvent = GameEvent.GameOverNoPlaystonesLeft;

                if (_boardStates[_boardStates.Count - 1].PlaystonesPlayer1 > 6)
                    Game.Winner = PlaystoneState.Player1;
                else
                    Game.Winner = PlaystoneState.Player2;
            }
        }

        public void UndoLastTurn()
        {
            if (!GameHasStarted || _boardStates.Count < 2 || Game.GameIsOver)
                return;
            if (Game.MoveIsActive || LastTurnWasMill)
            {
                CurrentPlayerEvent = GameEvent.FinishTurn;
                return;
            }
            if (_boardStates[_boardStates.Count - 1].BeforeLastTurnWasMill)
            {
                CurrentPlayerEvent = GameEvent.CannotUndoMill;
                return;
            }

            _lastBoardState = _boardStates[_boardStates.Count - 1];
            _boardStates.RemoveAt(_boardStates.Count - 1);
            LastTurnWasMill = false;
        }

        public void RedoLastTurn()
        {
            if (_lastBoardState == null)
                return;
            if (Game.MoveIsActive || LastTurnWasMill)
            {
                CurrentPlayerEvent = GameEvent.FinishTurn;
                return;
            }

            _boardStates.RemoveAt(_boardStates.Count - 1);
            _boardStates.Add(_lastBoardState);
            LastTurnWasMill = _beforeLastTurnWasMill;

            _lastBoardState = null;
            _beforeLastTurnWasMill = false;
        }

        private void SetPlaystone(int index1, int index2)
        {
            if (CurrentPlaystones[index1, index2] != PlaystoneState.Selectable)
                return;

            if (IsMill(index1, index2, CurrentPlayer, CurrentPlaystones, true))
            {
                LastTurnWasMill = true;
                CurrentPlayerEvent = GameEvent.PlayerHasMill;
            }

            NextStep(index1, index2, CurrentPlayer);

            if (LegimateTurns > 16)
            {
                GameHasStarted = true;
                LegimateTurns = 0;

                BoardState tmpState = _boardStates[_boardStates.Count - 1].Clone();
                Game.BoardStates = new List<BoardState>();
                Game.BoardStates.Add(tmpState);
                _boardStates = Game.BoardStates;

                ModelHelpFunctions.ReplacePlaystoneStates(
                    PlaystoneState.Selectable,
                    PlaystoneState.Neutral,
                    CurrentPlaystones
                    );
            }
            else
                LegimateTurns++;
        }

        private void MovePlaystone(int index1, int index2)
        {
            if (!Game.MoveIsActive && CurrentPlaystones[index1, index2] == CurrentPlayer)
            {
                if (!AreMovesPossible(index1, index2, CurrentPlayer, CurrentPlaystones))
                {
                    Game.GameIsOver = true;
                    CurrentPlayerEvent = GameEvent.GameOverNoMovesPossible;
                    Game.Winner = NotCurrentPlayer;
                    return;
                }
                if (ModelHelpFunctions.CountPlaystoneStates(CurrentPlayer, CurrentPlaystones) < 3)
                {
                    Game.GameIsOver = true;
                    CurrentPlayerEvent = GameEvent.GameOverNoPlaystonesLeft;
                    Game.Winner = NotCurrentPlayer;
                    return;
                }
                
                NextStep(index1, index2, CurrentPlaystones[index1, index2]);

                CurrentPlayer = NotCurrentPlayer;

                if (ModelHelpFunctions.CountPlaystoneStates(CurrentPlayer, CurrentPlaystones) <= 3)
                    PossibleMoves(index1, index2, CurrentPlayer, CurrentPlaystones, true);
                else
                    PossibleMoves(index1, index2, CurrentPlayer, CurrentPlaystones);
                
                Game.InactiveMoveIndex1 = index1;
                Game.InactiveMoveIndex2 = index2;

                Game.MoveIsActive = true;
                LastTurnWasMill = false;

                return;
            }
            else if (!Game.MoveIsActive)
            {
                CurrentPlayerEvent = GameEvent.InvalidPlaystone;
                return;
            }
            else if (CurrentPlaystones[index1, index2] == CurrentPlayer)
            {
                ModelHelpFunctions.ReplacePlaystoneStates(
                    PlaystoneState.Selectable,
                    PlaystoneState.Neutral,
                    CurrentPlaystones
                    );

                PossibleMoves(index1, index2, CurrentPlayer, CurrentPlaystones);

                Game.InactiveMoveIndex1 = index1;
                Game.InactiveMoveIndex2 = index2;

                return;
            }
            if (CurrentPlaystones[index1, index2] != PlaystoneState.Selectable)
            {
                CurrentPlayerEvent = GameEvent.InvalidPlaystone;
                return;
            }

            CurrentPlaystones[Game.InactiveMoveIndex1, Game.InactiveMoveIndex2] = PlaystoneState.Neutral;

            if (IsMill(index1, index2, CurrentPlayer, CurrentPlaystones, true))
            {
                LastTurnWasMill = true;
                CurrentPlayerEvent = GameEvent.PlayerHasMill;
            }

            CurrentPlaystones[index1, index2] = CurrentPlayer;

            ModelHelpFunctions.ReplacePlaystoneStates(
                    PlaystoneState.Selectable,
                    PlaystoneState.Neutral,
                    CurrentPlaystones
                    );
            
            Game.InactiveMoveIndex1 = -1;
            Game.InactiveMoveIndex2 = -1;

            CurrentPlayer = NotCurrentPlayer;

            Game.MoveIsActive = false;
        }

        private bool RemovePlaystone(int index1, int index2)
        {
            if (!LastTurnWasMill)
                return true;
            if (CurrentPlaystones[index1, index2] != NotCurrentPlayer)
            {
                CurrentPlayerEvent = GameEvent.WrongPlaystoneAfterMill;
                return true;
            }
            if (IsMill(index1, index2, NotCurrentPlayer, CurrentPlaystones))
            {
                CurrentPlayerEvent = GameEvent.CannotBreakMill;
                return true;
            }

            if (GameHasStarted)
                NextStep(index1, index2, PlaystoneState.Neutral);
            else
                NextStep(index1, index2, PlaystoneState.Selectable);

            if (NotCurrentPlayer == PlaystoneState.Player1)
                _boardStates[_boardStates.Count - 1].PlaystonesPlayer1++;
            else
                _boardStates[_boardStates.Count - 1].PlaystonesPlayer2++;

            LastTurnWasMill = false;

            if (!GameHasStarted)
                ModelHelpFunctions.ReplacePlaystoneStates(PlaystoneState.Neutral, PlaystoneState.Selectable, CurrentPlaystones);

            if (_boardStates.Count >= 2)
                _boardStates[_boardStates.Count - 1].BeforeLastTurnWasMill = true;

            return false;
        }

        private void NextStep(int index1, int index2, PlaystoneState state)
        {
            BoardState actualState = _boardStates[_boardStates.Count - 1];

            _boardStates.Add(new BoardState());

            CurrentPlaystones = ModelHelpFunctions.MergePlaystones(actualState.Playstones, index1, index2, state);

            if (actualState.ActivePlayer == PlaystoneState.Player1)
                CurrentPlayer = PlaystoneState.Player2;
            else
                CurrentPlayer = PlaystoneState.Player1;
            _boardStates[_boardStates.Count - 1].PlaystonesPlayer1 = actualState.PlaystonesPlayer1;
            _boardStates[_boardStates.Count - 1].PlaystonesPlayer2 = actualState.PlaystonesPlayer2;
        }

        static public bool IsMill(int index1, int index2, PlaystoneState playerToCheck, PlaystoneState[,] playstones,
            bool checkPartial = false)
        {
            bool isMill = false;
            
            if (index1 == 3)
            {
                if (index2 <= 2)
                    isMill = CheckHorizontalForMill(3, playerToCheck, playstones, checkPartial, 0, 2);
                else if (index2 >= 4)
                    isMill = CheckHorizontalForMill(3, playerToCheck, playstones, checkPartial, 4, 6);

                if (isMill)
                    return true;

                if (CheckVerticalForMill(index2, playerToCheck, playstones, checkPartial))
                    return true;
                else
                    return false;
            }
            else if (index2 == 3)
            {
                if (index1 <= 2)
                    isMill = CheckVerticalForMill(3, playerToCheck, playstones, checkPartial, 0, 2);
                else if (index1 >= 4)
                    isMill = CheckVerticalForMill(3, playerToCheck, playstones, checkPartial, 4, 6);

                if (isMill)
                    return true;

                if (CheckHorizontalForMill(index1, playerToCheck, playstones, checkPartial))
                    return true;
                else
                    return false;
            }

            if (CheckHorizontalForMill(index1, playerToCheck, playstones, checkPartial))
                return true;
            if (CheckVerticalForMill(index2, playerToCheck, playstones, checkPartial))
                return true;

            return false;
        }

        static private bool CheckHorizontalForMill(int line, PlaystoneState playerToCheck, PlaystoneState[,] playstones,
            bool checkPartial = false, int from = 0, int to = 6)
        {
            int tmpInt = 0;
            
            for (int i = from; i <= to; i++)
            {
                if (playstones[line, i] == playerToCheck)
                    tmpInt++;
            }

            if (tmpInt == 2 && checkPartial)
                return true;
            else if (tmpInt == 3)
                return true;
            else
                return false;
        }

        static private bool CheckVerticalForMill(int column, PlaystoneState playerToCheck, PlaystoneState[,] playstones,
            bool checkPartial = false, int from = 0, int to = 6)
        {
            int tmpInt = 0;

            for (int i = from; i <= to; i++)
            {
                if (playstones[i, column] == playerToCheck)
                    tmpInt++;
            }

            if (tmpInt == 2 && checkPartial)
                return true;
            else if (tmpInt == 3)
                return true;
            else
                return false;
        }

        static public void PossibleMoves(int index1, int index2, PlaystoneState activePlayer, PlaystoneState[,] playstones,
            bool freeJumps = false)
        {
            if (freeJumps)
            {
                ModelHelpFunctions.ReplacePlaystoneStates(PlaystoneState.Neutral, PlaystoneState.Selectable, playstones);

                return;
            }
            
            switch (ModelHelpFunctions.GetPosition(index1, index2))
            {
                case SquarePosition.TopLeft:
                    CheckCornerForMove(index1, index2, activePlayer, playstones, SquarePosition.Left, SquarePosition.TopMiddle);
                    break;
                case SquarePosition.TopMiddle:
                    CheckMiddleForMove(index1, index2, activePlayer, playstones, SquarePosition.TopLeft, SquarePosition.TopRight);
                    break;
                case SquarePosition.TopRight:
                    CheckCornerForMove(index1, index2, activePlayer, playstones, SquarePosition.TopMiddle, SquarePosition.Right);
                    break;
                case SquarePosition.Right:
                    CheckMiddleForMove(index1, index2, activePlayer, playstones, SquarePosition.TopRight, SquarePosition.BottomRight);
                    break;
                case SquarePosition.BottomRight:
                    CheckCornerForMove(index1, index2, activePlayer, playstones, SquarePosition.Right, SquarePosition.BottomMiddle);
                    break;
                case SquarePosition.BottomMiddle:
                    CheckMiddleForMove(index1, index2, activePlayer, playstones, SquarePosition.BottomRight, SquarePosition.BottomLeft);
                    break;
                case SquarePosition.BottomLeft:
                    CheckCornerForMove(index1, index2, activePlayer, playstones, SquarePosition.BottomMiddle, SquarePosition.Left);
                    break;
                case SquarePosition.Left:
                    CheckMiddleForMove(index1, index2, activePlayer, playstones, SquarePosition.BottomLeft, SquarePosition.TopLeft);
                    break;
                default:
                    return;
            }
        }

        static private void CheckCornerForMove(int index1, int index2, PlaystoneState activePlayer,
            PlaystoneState[,] playstones, SquarePosition pos1, SquarePosition pos2)
        {
            BoardSquare square = ModelHelpFunctions.GetSquare(index1, index2);

            CheckPositionForMove(activePlayer, playstones, square, pos1);
            CheckPositionForMove(activePlayer, playstones, square, pos2);
        }

        static private void CheckMiddleForMove(int index1, int index2, PlaystoneState activePlayer,
            PlaystoneState[,] playstones, SquarePosition pos1, SquarePosition pos2)
        {
            BoardSquare square = ModelHelpFunctions.GetSquare(index1, index2);

            CheckPositionForMove(activePlayer, playstones, square, pos1);
            CheckPositionForMove(activePlayer, playstones, square, pos2);

            switch (square)
            {
                case BoardSquare.OutterSquare:
                case BoardSquare.InnerSquare:
                    CheckPositionForMove(activePlayer, playstones, BoardSquare.MiddleSquare, ModelHelpFunctions.GetPosition(index1, index2));
                    break;
                case BoardSquare.MiddleSquare:
                    CheckPositionForMove(activePlayer, playstones, BoardSquare.OutterSquare, ModelHelpFunctions.GetPosition(index1, index2));
                    CheckPositionForMove(activePlayer, playstones, BoardSquare.InnerSquare, ModelHelpFunctions.GetPosition(index1, index2));
                    break;
            }
        }

        static private void CheckPositionForMove(PlaystoneState activePlayer,PlaystoneState[,] playstones,
            BoardSquare square, SquarePosition pos)
        {
            int i = 0, j = 0;

            ModelHelpFunctions.GetIndexes(square, pos, ref i, ref j);
            if (playstones[i, j] == PlaystoneState.Neutral)
                playstones[i, j] = PlaystoneState.Selectable;
        }

        static public bool AreMovesPossible(int index1, int index2, PlaystoneState activePlayer,
            PlaystoneState[,] playstones)
        {
            for (int i = 0; i <= 6; i++)
            {
                for (int j = 0; j <= 6; j++)
                {
                    if (playstones[i, j] == activePlayer)
                    {
                        PlaystoneState[,] stones = ModelHelpFunctions.CopyPlaystoneStates(playstones);

                        PossibleMoves(i, j, activePlayer, stones);

                        if (ModelHelpFunctions.CountPlaystoneStates(PlaystoneState.Selectable, stones) > 0)
                            return true;
                    }
                }
            }

            return false;
        }

        static public bool ArePlaystonesInMills(int index1, int index2, PlaystoneState player,
            PlaystoneState[,] playstones)
        {
            for (int i = 0; i <= 6; i++)
            {
                for (int j = 0; j <= 6; j++)
                {
                    if (playstones[i, j] == player)
                    {
                        if (IsMill(i, j, player, playstones))
                            return true;
                    }
                }
            }

            return false;
        }

        static public bool ArePlaystonesNotInMills(int index1, int index2, PlaystoneState player,
            PlaystoneState[,] playstones)
        {
            for (int i = 0; i <= 6; i++)
            {
                for (int j = 0; j <= 6; j++)
                {
                    if (playstones[i, j] == player)
                    {
                        if (!IsMill(i, j, player, playstones))
                            return true;
                    }
                }
            }

            return false;
        }

        static private PlaystoneState OtherPlayer(PlaystoneState player)
        {
            if (player == PlaystoneState.Player1)
                return PlaystoneState.Player2;
            else
                return PlaystoneState.Player1;
        }
    }
}
