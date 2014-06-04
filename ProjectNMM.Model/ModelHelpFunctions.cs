using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectNMM.Model
{
    static class ModelHelpFunctions
    {
        public const PlaystoneState DefaultAvailablePlaystoneState = PlaystoneState.Neutral;
        public const PlaystoneState DefaultNotAvailablePlaystoneState = PlaystoneState.NotAvailable;

        static public void InitializePlaystoneStates(PlaystoneState[,] playstones, PlaystoneState defaultValue = DefaultAvailablePlaystoneState)
        {
            SetPlaystoneStates(DefaultNotAvailablePlaystoneState, playstones);

            #region first square
            playstones[0, 0] = defaultValue;
            playstones[0, 3] = defaultValue;
            playstones[0, 6] = defaultValue;
            playstones[3, 6] = defaultValue;
            playstones[6, 6] = defaultValue;
            playstones[6, 3] = defaultValue;
            playstones[6, 0] = defaultValue;
            playstones[3, 0] = defaultValue;
            #endregion

            #region second square
            playstones[1, 1] = defaultValue;
            playstones[1, 3] = defaultValue;
            playstones[1, 5] = defaultValue;
            playstones[3, 5] = defaultValue;
            playstones[5, 5] = defaultValue;
            playstones[5, 3] = defaultValue;
            playstones[5, 1] = defaultValue;
            playstones[3, 1] = defaultValue;
            #endregion

            #region third square
            playstones[2, 2] = defaultValue;
            playstones[2, 3] = defaultValue;
            playstones[2, 4] = defaultValue;
            playstones[3, 4] = defaultValue;
            playstones[4, 4] = defaultValue;
            playstones[4, 3] = defaultValue;
            playstones[4, 2] = defaultValue;
            playstones[3, 2] = defaultValue;
            #endregion
        }

        static public void SetPlaystoneStates(PlaystoneState state, PlaystoneState[,] playstones)
        {
            for (int i = 0; i <= 6; i++)
            {
                for (int j = 0; j <= 6; j++)
                {
                    playstones[i, j] = state;
                }
            }
        }

        static public void ReplacePlaystoneStates(PlaystoneState stateOld, PlaystoneState stateNew, PlaystoneState[,] playstones)
        {
            for (int i = 0; i <= 6; i++)
            {
                for (int j = 0; j <= 6; j++)
                {
                    if (playstones[i, j] == stateOld)
                        playstones[i, j] = stateNew;
                }
            }
        }

        static public int CountPlaystoneStates(PlaystoneState state, PlaystoneState[,] playstones)
        {
            int tmpInt = 0;

            for (int i = 0; i <= 6; i++)
            {
                for (int j = 0; j <= 6; j++)
                {
                    if (playstones[i, j] == state)
                        tmpInt++;
                }
            }

            return tmpInt;
        }

        static public PlaystoneState[,] CopyPlaystoneStates(PlaystoneState[,] playstones)
        {
            PlaystoneState[,] stones = new PlaystoneState[7, 7];
            
            for (int i = 0; i <= 6; i++)
            {
                for (int j = 0; j <= 6; j++)
                {
                    stones[i, j] = playstones[i, j];
                }
            }

            return stones;
        }

        static public PlaystoneState[,] MergePlaystones(PlaystoneState[,] actualBoard, int index1, int index2,
            PlaystoneState newState)
        {
            PlaystoneState[,] playstones = (PlaystoneState[,])actualBoard.Clone();

            playstones[index1, index2] = newState;

            return playstones;
        }

        static public BoardSquare GetSquare(int index1, int index2)
        {
            if ((index1 == 0 && index2 == 0) ||
                (index1 == 0 && index2 == 3) ||
                (index1 == 0 && index2 == 6) ||
                (index1 == 3 && index2 == 6) ||
                (index1 == 6 && index2 == 6) ||
                (index1 == 6 && index2 == 3) ||
                (index1 == 6 && index2 == 0) ||
                (index1 == 3 && index2 == 0))
                return BoardSquare.OutterSquare;

            if ((index1 == 1 && index2 == 1) ||
                (index1 == 1 && index2 == 3) ||
                (index1 == 1 && index2 == 5) ||
                (index1 == 3 && index2 == 5) ||
                (index1 == 5 && index2 == 5) ||
                (index1 == 5 && index2 == 3) ||
                (index1 == 5 && index2 == 1) ||
                (index1 == 3 && index2 == 1))
                return BoardSquare.MiddleSquare;

            if ((index1 == 2 && index2 == 2) ||
                (index1 == 2 && index2 == 3) ||
                (index1 == 2 && index2 == 4) ||
                (index1 == 3 && index2 == 4) ||
                (index1 == 4 && index2 == 4) ||
                (index1 == 4 && index2 == 3) ||
                (index1 == 4 && index2 == 2) ||
                (index1 == 3 && index2 == 2))
                return BoardSquare.InnerSquare;

            return BoardSquare.NoSquare;
        }

        static public SquarePosition GetPosition(int index1, int index2)
        {
            switch (ModelHelpFunctions.GetSquare(index1, index2))
            {
                case BoardSquare.OutterSquare:
                    if (index1 == 0 && index2 == 0)
                        return SquarePosition.TopLeft;
                    if (index1 == 0 && index2 == 3)
                        return SquarePosition.TopMiddle;
                    if (index1 == 0 && index2 == 6)
                        return SquarePosition.TopRight;
                    if (index1 == 3 && index2 == 6)
                        return SquarePosition.Right;
                    if (index1 == 6 && index2 == 6)
                        return SquarePosition.BottomRight;
                    if (index1 == 6 && index2 == 3)
                        return SquarePosition.BottomMiddle;
                    if (index1 == 6 && index2 == 0)
                        return SquarePosition.BottomLeft;
                    if (index1 == 3 && index2 == 0)
                        return SquarePosition.Left;
                    break;
                case BoardSquare.MiddleSquare:
                    if (index1 == 1 && index2 == 1)
                        return SquarePosition.TopLeft;
                    if (index1 == 1 && index2 == 3)
                        return SquarePosition.TopMiddle;
                    if (index1 == 1 && index2 == 5)
                        return SquarePosition.TopRight;
                    if (index1 == 3 && index2 == 5)
                        return SquarePosition.Right;
                    if (index1 == 5 && index2 == 5)
                        return SquarePosition.BottomRight;
                    if (index1 == 5 && index2 == 3)
                        return SquarePosition.BottomMiddle;
                    if (index1 == 5 && index2 == 1)
                        return SquarePosition.BottomLeft;
                    if (index1 == 3 && index2 == 1)
                        return SquarePosition.Left;
                    break;
                case BoardSquare.InnerSquare:
                    if (index1 == 2 && index2 == 2)
                        return SquarePosition.TopLeft;
                    if (index1 == 2 && index2 == 3)
                        return SquarePosition.TopMiddle;
                    if (index1 == 2 && index2 == 4)
                        return SquarePosition.TopRight;
                    if (index1 == 3 && index2 == 4)
                        return SquarePosition.Right;
                    if (index1 == 4 && index2 == 4)
                        return SquarePosition.BottomRight;
                    if (index1 == 4 && index2 == 3)
                        return SquarePosition.BottomMiddle;
                    if (index1 == 4 && index2 == 2)
                        return SquarePosition.BottomLeft;
                    if (index1 == 3 && index2 == 2)
                        return SquarePosition.Left;
                    break;
                default:
                    break;
            }

            return SquarePosition.NoCorner;
        }

        static public void GetIndexes(BoardSquare square, SquarePosition position, ref int index1, ref int index2)
        {
            index1 = -1;
            index2 = -1;

            for (int i = 0; i <= 6; i++)
            {
                for (int j = 0; j <= 6; j++)
                {
                    if (GetSquare(i, j) == square && GetPosition(i, j) == position)
                    {
                        index1 = i;
                        index2 = j;

                        return;
                    }
                }
            }
        }
    }
}
