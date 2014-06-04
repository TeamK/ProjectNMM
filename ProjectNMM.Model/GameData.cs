using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectNMM.Model
{
    class GameData
    {
        public GameType GameType;
        public List<BoardState> BoardStates;
        public bool MoveIsActive;
        public int InactiveMoveIndex1;
        public int InactiveMoveIndex2;
        public string PlayerName1;
        public string PlayerName2;
        public DateTime StartTime;
        public DateTime EndTime;
        public bool GameIsOver;
        public PlaystoneState Winner;
        public string Description;

        public GameData(GameType gameType)
        {
            GameType = gameType;

            PlayerName1 = "";
            PlayerName2 = "";
            MoveIsActive = false;
            InactiveMoveIndex1 = -1;
            InactiveMoveIndex2 = -1;
            BoardStates = new List<BoardState>();
            StartTime = new DateTime();
            EndTime = new DateTime();
            GameIsOver = false;
            Winner = PlaystoneState.NotAvailable;
            Description = "";
        }
    }
    
    class BoardState
    {
        public PlaystoneState[,] Playstones;
        public PlaystoneState ActivePlayer;
        public int PlaystonesPlayer1;
        public int PlaystonesPlayer2;
        public bool BeforeLastTurnWasMill;

        public BoardState()
        {
            Playstones = new PlaystoneState[7, 7];
            ModelHelpFunctions.InitializePlaystoneStates(Playstones);

            ActivePlayer = PlaystoneState.Neutral;

            PlaystonesPlayer1 = 0;
            PlaystonesPlayer2 = 0;

            BeforeLastTurnWasMill = false;
        }

        public BoardState Clone()
        {
            BoardState newState = new BoardState();

            for (int i = 0; i <= 6; i++)
            {
                for (int j = 0; j <= 6; j++)
                {
                    newState.Playstones[i, j] = Playstones[i, j];
                }
            }
            newState.ActivePlayer = ActivePlayer;
            newState.PlaystonesPlayer1 = PlaystonesPlayer1;
            newState.PlaystonesPlayer2 = PlaystonesPlayer2;

            return newState;
        }
    }
}
