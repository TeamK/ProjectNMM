using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ProjectNMM.Model;

namespace ProjectNMM.UI
{
    class UiHelpFunctions
    {
        private Grid _workGrid;
        private OnClickEvent _ellipsEvent;

        private const double selectablePlaystoneRadius = 30;
        private const double nonselectablePlaystoneRadius = 7;

        public double[,] CoordinatesX { get; private set; }
        public double[,] CoordinatesY { get; private set; }

        public UiHelpFunctions(Grid grid, OnClickEvent ellipsEvent)
        {
            CoordinatesX = new double[7, 7];
            CoordinatesY = new double[7, 7];
            InitializeCoordinates();

            _workGrid = grid;
            _ellipsEvent = ellipsEvent;
        }

        private void InitializeCoordinates()
        {
            for (int i = 0; i <= 6; i++)
            {
                for (int j = 0; j <= 6; j++)
                {
                    CoordinatesX[i, j] = -1;
                    CoordinatesY[i, j] = -1;
                }
            }

            #region first square
            CoordinatesX[0, 0] = 30;
            CoordinatesY[0, 0] = 50;
            CoordinatesX[0, 3] = 230;
            CoordinatesY[0, 3] = 50;
            CoordinatesX[0, 6] = 430;
            CoordinatesY[0, 6] = 50;
            CoordinatesX[3, 6] = 430;
            CoordinatesY[3, 6] = 250;
            CoordinatesX[6, 6] = 430;
            CoordinatesY[6, 6] = 450;
            CoordinatesX[6, 3] = 230;
            CoordinatesY[6, 3] = 450;
            CoordinatesX[6, 0] = 30;
            CoordinatesY[6, 0] = 450;
            CoordinatesX[3, 0] = 30;
            CoordinatesY[3, 0] = 250;
            #endregion

            #region second square
            CoordinatesX[1, 1] = 86;
            CoordinatesY[1, 1] = 106;
            CoordinatesX[1, 3] = 229;
            CoordinatesY[1, 3] = 106;
            CoordinatesX[1, 5] = 373;
            CoordinatesY[1, 5] = 106;
            CoordinatesX[3, 5] = 373;
            CoordinatesY[3, 5] = 249;
            CoordinatesX[5, 5] = 373;
            CoordinatesY[5, 5] = 393;
            CoordinatesX[5, 3] = 229;
            CoordinatesY[5, 3] = 393;
            CoordinatesX[5, 1] = 86;
            CoordinatesY[5, 1] = 393;
            CoordinatesX[3, 1] = 86;
            CoordinatesY[3, 1] = 249;
            #endregion

            #region third square
            CoordinatesX[2, 2] = 155;
            CoordinatesY[2, 2] = 175;
            CoordinatesX[2, 3] = 230;
            CoordinatesY[2, 3] = 175;
            CoordinatesX[2, 4] = 305;
            CoordinatesY[2, 4] = 175;
            CoordinatesX[3, 4] = 305;
            CoordinatesY[3, 4] = 250;
            CoordinatesX[4, 4] = 305;
            CoordinatesY[4, 4] = 325;
            CoordinatesX[4, 3] = 230;
            CoordinatesY[4, 3] = 325;
            CoordinatesX[4, 2] = 155;
            CoordinatesY[4, 2] = 325;
            CoordinatesX[3, 2] = 155;
            CoordinatesY[3, 2] = 250;
            #endregion
        }

        public void DrawLines(Grid grid)
        {
            #region upper vertical line
            grid.Children.Add(new Line()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                X1 = CoordinatesX[0, 3],
                Y1 = CoordinatesY[0, 3],
                X2 = CoordinatesX[2, 3],
                Y2 = CoordinatesY[2, 3]
            });
            #endregion

            #region lower vertical line
            grid.Children.Add(new Line()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                X1 = CoordinatesX[4, 3],
                Y1 = CoordinatesY[4, 3],
                X2 = CoordinatesX[6, 3],
                Y2 = CoordinatesY[6, 3]
            });
            #endregion

            #region left horizontal line
            grid.Children.Add(new Line()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                X1 = CoordinatesX[3, 0],
                Y1 = CoordinatesY[3, 0],
                X2 = CoordinatesX[3, 2],
                Y2 = CoordinatesY[3, 2]
            });
            #endregion

            #region right horizontal line
            grid.Children.Add(new Line()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                X1 = CoordinatesX[3, 4],
                Y1 = CoordinatesY[3, 4],
                X2 = CoordinatesX[3, 6],
                Y2 = CoordinatesY[3, 6]
            });
            #endregion
        }

        public void DrawPlaystones(PlaystoneState[,] playstones)
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    _workGrid.Children.Add(GenerateEllipse(CoordinatesX[i, j], CoordinatesY[i, j], playstones[i, j], _ellipsEvent));
                }
            }
        }

        private Ellipse GenerateEllipse(double left, double top, PlaystoneState playstoneState, OnClickEvent clickEvent)
        {
            double radiusToUse = 0;
            Ellipse ellipse = new Ellipse
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            #region get radius for ellipse
            switch (playstoneState)
            {
                case PlaystoneState.Neutral:
                    radiusToUse = nonselectablePlaystoneRadius;
                    break;
                case PlaystoneState.Player1:
                case PlaystoneState.Player2:
                case PlaystoneState.Selectable:
                    radiusToUse = selectablePlaystoneRadius;
                    break;
                default:
                    radiusToUse = 0;
                    break;
            }
            ellipse.Width = radiusToUse;
            ellipse.Height = radiusToUse;
            #endregion

            #region set color
            switch (playstoneState)
            {
                case PlaystoneState.Player1:
                    ellipse.Fill = Brushes.Crimson;
                    break;
                case PlaystoneState.Player2:
                    ellipse.Fill = Brushes.DarkCyan;
                    break;
                case PlaystoneState.Selectable:
                    ellipse.Fill = Brushes.Gray;
                    break;
                default:
                    ellipse.Fill = Brushes.Black;
                    break;
            }
            #endregion

            #region assign event
            switch (playstoneState)
            {
                case PlaystoneState.Player1:
                case PlaystoneState.Player2:
                case PlaystoneState.Selectable:
                    ellipse.MouseDown +=
                        new MouseButtonEventHandler(clickEvent);
                    break;
                default:
                    break;
            }
            #endregion

            #region set coordinates
            Thickness margin = new Thickness()
            {
                Left = (left + radiusToUse / 2) - radiusToUse,
                Top = (top + radiusToUse / 2) - radiusToUse
            };
            ellipse.Margin = margin;
            #endregion

            return ellipse;
        }

        public void GetIndexes(double coordinatesX, double coordinatesY, ref int index1, ref int index2)
        {
            index1 = -1;
            index2 = -1;

            int x = Convert.ToInt32(coordinatesX + selectablePlaystoneRadius + (selectablePlaystoneRadius / 2) * (-1));
            int y = Convert.ToInt32(coordinatesY + selectablePlaystoneRadius + (selectablePlaystoneRadius / 2) * (-1));

            for (int i = 0; i <= 6; i++)
            {
                for (int j = 0; j <= 6; j++)
                {
                    if (Convert.ToInt32(CoordinatesX[i, j]) == x &&
                        Convert.ToInt32(CoordinatesY[i, j]) == y)
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
