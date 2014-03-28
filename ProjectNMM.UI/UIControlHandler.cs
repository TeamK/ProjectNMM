using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Windows;
using ProjectNMM.Model;

namespace ProjectNMM.UI
{
   class UiControlHandler
   {
      public enum GameType { undefined, pvp, pve, eve, };
      private GameType typeofGame;

      public GameScreen Gamescreen { get; set; }
      /*
        Action newAc = () =>
         {
            startScreen.Close();
         };
         Application.Current.Dispatcher.Invoke(newAc);
       */

      public UiControlHandler(GameScreen gameScreen)
      {
         this.Gamescreen = gameScreen;
      }

      public void NewGame(GameType gameType)
      {
         typeofGame = gameType;
      }
   }
}
