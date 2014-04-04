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
        private GameType _typeofGame;

        public GameScreen Gamescreen { get; set; }
        /*
          Action newAc = () =>
           {
              startScreen.Close();
           };
           Application.Current.Dispatcher.Invoke(newAc);
         */

        public UiControlHandler()
        {
           // this.Gamescreen = gameScreen;
        }

        public void NewGame(GameType gameType)
        {
            _typeofGame = gameType;
        }

        public void StartGame()
        {

        }
    }
}
