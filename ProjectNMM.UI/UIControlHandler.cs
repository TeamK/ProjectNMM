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
        private GameScreen _gameScreen;
        private GameType _typeOfGame;

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
            _gameScreen = new GameScreen();

            
            

            // this.Gamescreen = gameScreen;
        }

        public void NewGame(GameType gameType)
        {
            _typeOfGame = gameType;
        }

        public void StartGame()
        {
            //_gameScreen.Show();
            //_gameScreen.Visibility = Visibility.Hidden;
            StartScreen startScreen = new StartScreen();
            startScreen.Show();
        }
    }
}
