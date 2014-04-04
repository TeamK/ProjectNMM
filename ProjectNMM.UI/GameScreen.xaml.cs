using System;
using System.Collections.Generic;
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
      UiControlHandler uicontrol;
      public GameScreen()
      {
         uicontrol = new UiControlHandler();

         InitializeComponent();
      }

      private void NewGamePvP_OnClick(object sender, RoutedEventArgs e)
        {
           uicontrol.NewGame(GameType.PlayerVsPlayer);
        }
   }
}
