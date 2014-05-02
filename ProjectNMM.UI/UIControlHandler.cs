using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Windows;
using ProjectNMM.Model;

namespace ProjectNMM.UI
{
    public delegate void StartNewGame(GameType gameType);
    
    class UiControlHandler
    {
        private GameScreen _gameScreen;
        private StartScreen _startScreen;
        private GameType _typeOfGame;

        public ShutdownApp ShutdownDelegate { get; set; }

        public UiControlHandler()
        {
            _gameScreen = new GameScreen();

            
            

            // this.Gamescreen = gameScreen;
        }

        public void NewGame(GameType gameType)
        {
            _typeOfGame = gameType;

            if (_startScreen != null)
            {
                _startScreen.Close();
                _startScreen = null;
            }
        }

        public void StartGame()
        {
            //_gameScreen.Show();
            //_gameScreen.Visibility = Visibility.Hidden;
            _startScreen = new StartScreen(NewGame);
            _startScreen.Show();
        }
    }
}
