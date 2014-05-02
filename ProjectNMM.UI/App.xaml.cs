using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace ProjectNMM.UI
{
    internal delegate void ShutdownApp();
    
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private UiControlHandler _uiHandler;
        
        public App()
        {
            _uiHandler = new UiControlHandler();

            _uiHandler.ShutdownDelegate = new ShutdownApp(Shutdown);

            _uiHandler.StartGame();
        }

    }
}
