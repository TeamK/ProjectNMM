using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace ProjectNMM.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            UiControlHandler uiHandler = new UiControlHandler();

            uiHandler.StartGame();
        }

    }
}
