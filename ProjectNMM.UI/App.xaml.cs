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
        private UiControl _uiControl;
        
        public App()
        {
            _uiControl = new UiControl();

            _uiControl.StartGame();
        }
    }
}
